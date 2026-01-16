namespace Domain.Entities
{
    public partial class Product
    {
        public string ProductId { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string Category { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
