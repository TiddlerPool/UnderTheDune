using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class InventoryController :MonoBehaviour
{
    //Inputs
    StarterAssetsInputs _input;
    InputMapManager _mapManager;

    private bool openState;
    private bool isStore;
    public bool InStore;

    public float TradePressTime;
    public float DiscardPressTime;
    public int NewItemID;

    InventoryHighlight inventoryHighlight;
    [SerializeField] protected ItemGrid selectedItemGrid;
    [SerializeField] protected ItemGrid oldSelectedGrid;

    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }

    public InventoryItem selectedItem;
    public InventoryItem pointedItem;
    public ItemDetailInfo storePanel;

    ItemDetailInfo detailPanel;
    InventoryItem overlapItem;
    RectTransform rectTransform;
    ItemDataLibrary library;
    public WorldRunningMachine database;
    
    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] protected Transform canvasTransform;
    [SerializeField] Transform handinTransform;
    [SerializeField] Transform excavationTransform;
    public Excavation_Toggle ExcavationToggle;

    public virtual void Awake()
    {
        _input = FindObjectOfType<StarterAssetsInputs>();
        _mapManager = FindObjectOfType<InputMapManager>();
        inventoryHighlight = GetComponent<InventoryHighlight>();
        library = GetComponent<ItemDataLibrary>();
        detailPanel = GetComponent<ItemDetailInfo>();
    }

    private void Update()
    {   if(!canvasTransform.gameObject.activeSelf)
        {
            selectedItemGrid = null;
            pointedItem = null;
            
        }
        GetItemPoint();
        ItemIconDrag();
        OpenInventory();
        UpdateItemInfoNControl();
/*
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (selectedItem == null)
            {
                CreateItemByID(NewItemID);
            }

        }
*/
        if (Input.GetKeyDown(KeyCode.Y))
        {
            InsertRandomItem();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            isStore = false;
            return;
        }
        else
        {
            isStore = selectedItemGrid.IsStore;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
            LeftMouseButtonPress();
        }
    }

    private void GetItemPoint()
    {
        
        if (selectedItemGrid == null)
        {
            pointedItem = null;
            return;
        }

        InventoryItem ItemToPoint = selectedItemGrid.GetItem(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
        if (ItemToPoint != null && ItemToPoint !=selectedItem)
        {
            pointedItem = ItemToPoint;
        }
        else
        {
            pointedItem = null;
        }
    }

    [SerializeField] bool startControl;
    private void UpdateItemInfoNControl()
    {
        detailPanel.InStore = InStore;
        TrackControl();
        SellItem();
        UseItem();
        DiscardItem();
        HandleItemControl(InStore, isStore);
        if (selectedItem != null)
        {
            startControl = true;
            detailPanel.ShowDetailInfo(selectedItem.Data, selectedItem);
            storePanel.HideDetailInfo();
        }
        else if(pointedItem != null)
        {
            if (!isStore)
                detailPanel.ShowDetailInfo(pointedItem.Data);
            else
                storePanel.ShowDetailInfo(pointedItem.Data);
        }     
        else
        {
            detailPanel.HideDetailInfo();
            storePanel.HideDetailInfo();
        }
    }

    private void TrackControl()
    {
        if(pointedItem != null || selectedItem != null)
        {
            startControl = true;
        }
        else
        {
            startControl = false;
        }
    }

    private void HandleItemControl(bool inStore, bool isStore)
    {
        if (startControl)
        {
            if (_input.trade && inStore && !isStore)
            {
                if(pointedItem != null && pointedItem.Tradable)
                {
                    TradePressTime = Mathf.Clamp(TradePressTime += Time.deltaTime, 0f, 1.5f);
                    DiscardPressTime = Mathf.Clamp(DiscardPressTime -= Time.deltaTime * 5f, 0f, 1f);
                }
                else if(selectedItem != null && selectedItem.Tradable)
                {
                    TradePressTime = Mathf.Clamp(TradePressTime += Time.deltaTime, 0f, 1.5f);
                    DiscardPressTime = Mathf.Clamp(DiscardPressTime -= Time.deltaTime * 5f, 0f, 1f);
                }
            }
            else if (_input.discard && !isStore)
            {
                if (pointedItem != null && pointedItem.Tradable)
                {
                    DiscardPressTime = Mathf.Clamp(DiscardPressTime += Time.deltaTime, 0f, 1.5f);
                    TradePressTime = Mathf.Clamp(TradePressTime -= Time.deltaTime * 5f, 0f, 1f);
                }
                else if (selectedItem != null && selectedItem.Tradable)
                {
                    DiscardPressTime = Mathf.Clamp(DiscardPressTime += Time.deltaTime, 0f, 1.5f);
                    TradePressTime = Mathf.Clamp(TradePressTime -= Time.deltaTime * 5f, 0f, 1f);
                }
            }
            else
            {
                DiscardPressTime = Mathf.Clamp(DiscardPressTime -= Time.deltaTime * 5f, 0f, 1f);
                TradePressTime = Mathf.Clamp(TradePressTime -= Time.deltaTime * 5f, 0f, 1f);
            }
        }
        else
        {
            DiscardPressTime = Mathf.Clamp(DiscardPressTime -= Time.deltaTime * 5f, 0f, 1f);
            TradePressTime = Mathf.Clamp(TradePressTime -= Time.deltaTime * 5f, 0f, 1f);
        }
    }

    private void DiscardItem()
    {
        if(DiscardPressTime == 1.5f && selectedItem != null)
        {
            Destroy(selectedItem.gameObject);
            AudioManager.PlaySound("Discard", AudioManager.library, false);
            rectTransform = null;
            startControl = false;
        }
        else if(DiscardPressTime == 1.5f && pointedItem != null)
        {
            Destroy(selectedItemGrid.PickUpItem(pointedItem.onGridPositionX, pointedItem.onGridPositionY));
            Destroy(pointedItem.gameObject);
            AudioManager.PlaySound("Discard", AudioManager.library, false);
            rectTransform = null;
            startControl = false;
        }
    }

    private void SellItem()
    {
        if (TradePressTime == 1.5f && selectedItem != null)
        {
            database.Money += selectedItem.Data.price;
            Destroy(selectedItem.gameObject);
            AudioManager.PlaySound("Sell", AudioManager.library, false);
            rectTransform = null;
            startControl = false;
        }
        else if (TradePressTime == 1.5f && pointedItem != null)
        {
            database.Money += pointedItem.Data.price;
            Destroy(selectedItemGrid.PickUpItem(pointedItem.onGridPositionX, pointedItem.onGridPositionY));
            Destroy(pointedItem.gameObject);
            AudioManager.PlaySound("Sell", AudioManager.library, false);
            rectTransform = null;
            startControl = false;
        }
    }

    private void UseItem()
    {
        if(_input.use && selectedItem != null)
        {
            if(selectedItem.Usable)
            {
                _input.use = false;
                if(selectedItem.ItemType == "Food")
                {
                    Debug.Log("Food Served");
                    database.CurrentHunger += selectedItem.UseValue;
                    AudioManager.PlaySound("Eat", AudioManager.library, false);

                }
                else if(selectedItem.ItemType == "Consumables")
                {
                    Debug.Log("Consumables Used");
                    database.CurrentHealth += selectedItem.UseValue;
                    AudioManager.PlaySound("Drink", AudioManager.library, false);
                }
                Destroy(selectedItem.gameObject);
            }
            else
            {
                _input.use = false;
            }
        }
        else if(_input.use && pointedItem != null)
        {
            if (pointedItem.Usable)
            {
                _input.use = false;
                if (pointedItem.ItemType == "Food")
                {
                    Debug.Log("Food Served");
                    database.CurrentHunger += pointedItem.UseValue;
                    AudioManager.PlaySound("Eat", AudioManager.library, false);
                }
                else if (pointedItem.ItemType == "Consumables")
                {
                    Debug.Log("Consumables Used");
                    database.CurrentHealth += pointedItem.UseValue;
                    AudioManager.PlaySound("Drink", AudioManager.library, false);
                }
                Destroy(selectedItemGrid.PickUpItem(pointedItem.onGridPositionX, pointedItem.onGridPositionY));
                Destroy(pointedItem.gameObject);
            }
            else
            {
                _input.use = false;
            }
        }
    }

    private void RotateItem()
    {
        if (selectedItem == null) { return; }
        AudioManager.PlaySound("Dialog", AudioManager.library, false);
        selectedItem.Rotate();
    }

    private void InsertRandomItem()
    {
        if (selectedItemGrid == null) { return; }

        CreateItemByID(NewItemID);
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        if (selectedItemGrid == null) { return; }
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

        if (posOnGrid == null)
        {
            Debug.Log("No Avaliable Space");
            Destroy(itemToInsert.gameObject);
            return;
        }

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    Vector2Int oldPosition;
    InventoryItem itemToHighlight;


    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPosition == positionOnGrid) { return; }

        oldPosition = positionOnGrid;
        if (selectedItem == null)
        {
            if (selectedItemGrid == null) { return; }
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.WIDTH,
                selectedItem.HEIGHT)
                );

            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    public InventoryItem CreateItemByData(InventoryItemData data)
    {
        if (library == null)
        {
            Debug.Log("Item Library Not Found");
            return null;
        }

        InventoryItem inventoryItem = Instantiate(itemPrefab, canvasTransform).GetComponent<InventoryItem>();
        inventoryItem.Set(library.Library[data.itemDataID]);
        inventoryItem.Rotated = data.rotated;
        if(inventoryItem.Rotated)
        {
            RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
            rectTransform.rotation = Quaternion.Euler(0, 0, inventoryItem.Rotated? 90f : 0f);
        }
        return inventoryItem;
    }

    private void CreateItemByID(int id)
    {
        if(library == null)
        {
            Debug.Log("Item Library Not Found");
            return;
        }

        InventoryItem inventoryItem = Instantiate(itemPrefab, canvasTransform).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;


        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        inventoryItem.Set(library.Library[id]);
    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        if(pointedItem != null && selectedItem == null)
        {
            if(InStore && isStore)
            {
                if(database.Money >= pointedItem.Data.price)
                {
                    PickUpItem(tileGridPosition);
                    database.Money -= pointedItem.Data.price;
                    AudioManager.PlaySound("Buy", AudioManager.library, false);
                }
                else
                {
                    Debug.Log("Do not have Enough Budget");
                    NoticeEvent.current.NoticeTrigger(0);
                }
            }
            else
            {
                oldSelectedGrid = selectedItemGrid;
                PickUpItem(tileGridPosition);
                AudioManager.PlaySound("Click", AudioManager.library, false);
            }
        }
        else if(pointedItem == null && selectedItem != null)
        {
            if(InStore && isStore)
            {
                Debug.Log("Can not put you stuff in the store");
            }
            else
            {
                PlaceItem(tileGridPosition);
                AudioManager.PlaySound("Click", AudioManager.library, false);
                oldSelectedGrid = null;
            }
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }

        return selectedItemGrid.GetTileGridPosition(position);
    }

    public virtual void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (complete)
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }
    }

    public virtual void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
            rectTransform.SetParent(canvasTransform);
        }
    }

    private void OpenInventoryController(bool openState)
    {
        if(openState)
        {
            _mapManager.SwitchMap(2);
            AudioManager.PlaySound("BagOpen", AudioManager.library, false);
        }    
        else
        {
            _mapManager.SwitchMap(0);
            handinTransform.gameObject.SetActive(openState);
            excavationTransform.gameObject.SetActive(openState);
            AudioManager.PlaySound("BagClose", AudioManager.library, false);
        }
            

        _mapManager.SetCursorState(!openState);
        canvasTransform.gameObject.SetActive(openState); 
    }

    private void OpenInventory()
    {
        if (_input.inventory)
        {
            openState = !openState;
            PutSelectItemBack();
            OpenInventoryController(openState);
            _input.inventory = false;
        }
        else if(_input.closeInventory && !InStore)
        {
            openState = false;
            PutSelectItemBack();
            OpenInventoryController(openState);
            _input.closeInventory = false;
            if(ExcavationToggle != null)
                ExcavationToggle.HandleClose();
        }
    }

    private void PutSelectItemBack()
    {
        if (selectedItem != null && oldSelectedGrid != null)
        {
            oldSelectedGrid.PlaceItem(selectedItem, selectedItem.onGridPositionX, selectedItem.onGridPositionY);
            selectedItem = null;
        }
    }
}
