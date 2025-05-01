namespace basic_ecommerce.Domain;

public class User
{
    public string Id { get; set; } = new Guid().ToString();
    public string Name { get; set; } 
    public string Email { get; set; } 
    public DateTime CreatedAt { get; set; } 
}