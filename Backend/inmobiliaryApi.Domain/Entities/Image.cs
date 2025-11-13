namespace inmobiliaryApi.Domain.Entities;

public class Image
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string PublicId { get; set; }
    public int PropertyId { get; set; }
    
    public Property Property { get; set; }
}