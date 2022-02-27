using System;

namespace Play.Inventory.Service
{
    public record GrantItemDto(Guid UserId, Guid CatalogItemId, int Quantity);

    public record InventoryItemDtos(Guid CatalogItemId, int Quantity, DateTimeOffset AcquiredDate, string name, string descriptions);

    // public record CatalogItem(Guid Id, string name, string description);
}