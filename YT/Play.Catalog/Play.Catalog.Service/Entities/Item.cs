using System;
using Play.Common.Interface;

namespace Play.Catalog.Service.Entities
{

    public class Item : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreatedDateTime { get; set; }

    }
}