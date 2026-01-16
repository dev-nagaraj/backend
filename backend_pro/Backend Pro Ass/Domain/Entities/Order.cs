namespace Domain.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public string ProductId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public DateTime? DateOfSale { get; set; }

    public int? QuantitySold { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? Discount { get; set; }

    public decimal? ShippingCost { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
