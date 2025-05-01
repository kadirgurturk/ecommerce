namespace basic_ecommerce.Domain;

public class Product
{
    public string Id { get; set; } = new Guid().ToString();
    public string Name { get; set; } 
    public string Description { get; set; }
    public decimal Price { get; set; } 
    public int StockQuantity { get; set; } 
}