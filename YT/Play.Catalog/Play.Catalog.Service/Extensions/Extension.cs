using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Extensions
{
    public static class Extensions
    {
        public static ItemDtos AsDTO(this Item item)
        {
            return new ItemDtos(item.Id, item.Name, item.Description, item.Price, item.CreatedDateTime);
        }
    }
}