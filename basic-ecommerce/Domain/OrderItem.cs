namespace basic_ecommerce.Domain;

public class OrderItem
{
    public string Id { get; set; }  = new Guid().ToString();
    public string OrderId { get; set; }
    public string ProductId { get; set; } 
    public int Quantity { get; set; } 
    public decimal Price { get; set; } 

    public Order Order { get; set; } 
    public Product Product { get; set; }
}