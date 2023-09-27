

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]InventroyController inventroyController;
    ItemGrid itemGrid;

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventroyController.SelectedItemGrid = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventroyController.SelectedItemGrid = null;
    }

    private void Awake()
    {
        inventroyController = FindObjectOfType(typeof(InventroyController)) as InventroyController;
        itemGrid = GetComponent<ItemGrid>();
    }
}
