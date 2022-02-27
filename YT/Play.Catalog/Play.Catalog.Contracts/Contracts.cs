using System;

namespace Play.Catalog.Contracts
{
    public record CatalogItemCreated(Guid CatalogId, string name, string description);

    public record CatalogItemUpdated(Guid CatalogId, string name, string description);

    public record CatalogItemDeleted(Guid CatalogId);
}