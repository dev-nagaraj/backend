namespace Domain.Entities;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public string Region { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string CustomerEmail { get; set; } = null!;

    public string CustomerAddress { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
