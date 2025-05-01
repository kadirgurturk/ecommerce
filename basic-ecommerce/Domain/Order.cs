namespace basic_ecommerce.Domain;

public class Order
{
    public string Id { get; set; } = new Guid().ToString();
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; } 
    public decimal TotalAmount { get; set; } 
    public OrderStatus Status { get; set; } 
    
    public User User { get; set; } 
    public List<OrderItem> OrderItems { get; set; } 
}

public enum OrderStatus
{
    Pending,
    Completed,
    Canceled
}