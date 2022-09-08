namespace AuthAPI.Domain.Models; 

public class Address {
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }
    public string Country { get; set; }
    public int Zip { get; set; }
}