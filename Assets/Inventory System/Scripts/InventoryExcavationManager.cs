using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryExcavationManager : MonoBehaviour
{
    [SerializeField] protected ItemGrid _storeGrid;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<InventoryItem> items;

    public List<InventoryItem> Items
    {
        get { return items; }
        set { items = value; }
    }
    ItemDataLibrary _library;

    private void Awake()
    {
        _storeGrid = GetComponent<ItemGrid>();
        _library = FindObjectOfType<ItemDataLibrary>();
    }

    private void Update()
    {
        items = _storeGrid.GridList;
    }

    public void PlaceItemsOnShelf(ItemData[] itemsData)
    {
        _storeGrid.ClearGrid();
        for (int i = 0; i < itemsData.Length; i++)
        {
            InventoryItem item = CreateNewItem(itemsData[i]);
            InsertItem(item);
        }
    }

    private InventoryItem CreateNewItem(ItemData data)
    {
        if (_library == null)
        {
            Debug.Log("Item Library Not Found");
            return null;
        }

        InventoryItem item = Instantiate(itemPrefab, _storeGrid.transform).GetComponent<InventoryItem>();
        item.Set(_library.Library[data.ItemID]);
        return item;
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        if (_storeGrid == null) { return; }
        Vector2Int? posOnGrid = _storeGrid.FindSpaceForObject(itemToInsert);

        if (posOnGrid == null)
        {
            Destroy(itemToInsert.gameObject);
            return;
        }

        _storeGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }
}
