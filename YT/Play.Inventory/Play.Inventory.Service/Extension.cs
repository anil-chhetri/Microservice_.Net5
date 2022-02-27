using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Properties;

namespace Play.Inventory.Service
{
    public static class Extension
    {
        public static InventoryItemDtos AsDto(this InventoryItem item)
        {
            return new InventoryItemDtos(item.CatalogItemId, item.Quantity, item.AcquiredDate);
        }
    }
}