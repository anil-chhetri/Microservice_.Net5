using System;

namespace Play.Inventory.Service.Properties
{
    public record GrantItemDto(Guid UserId, Guid CatalogItemId, int Quantity);

    public record InventoryItemDtos(Guid CatalogItemId, int Quantity, DateTimeOffset AcquiredDate);
}