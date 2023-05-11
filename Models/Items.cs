namespace ConsumeInventaory.Models
{
    public class Items
    {
        public Guid Id { get; set; }
        public string ItemsName { get; set; }
        public int ItemsQuantity { get; set; }
        public string ItemsType { get; set; }
        public int Price { get; set; }
    }
}
