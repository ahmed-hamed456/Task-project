namespace OrderManagement.Domain.Entities;

public class Customer
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    // EF Core constructor
    private Customer() 
    {
        Name = string.Empty;
        Email = string.Empty;
    }

    public Customer(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Customer email cannot be empty.", nameof(email));

        Name = name;
        Email = email;
    }
}
