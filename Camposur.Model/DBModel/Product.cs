namespace Camposur.Model.DBModel
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public int Stock { get; set; }

        public decimal Price { get; set; }

        public byte[] Image { get; set; }

        public int ProductTypeId { get; set; }

        public ProductType ProductType { get; set; }
    }
}