namespace inmobiliaryApi.Domain.Entities;

public class Property
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Location { get; set; }
    
    public List<Image>? Images { get; set; }
}