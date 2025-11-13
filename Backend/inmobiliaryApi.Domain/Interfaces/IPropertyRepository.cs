using inmobiliaryApi.Domain.Entities;

namespace inmobiliaryApi.Domain.Interfaces;

public interface IPropertyRepository
{
    public Task<IEnumerable<Property>> GetAllProperties();
    public Task<Property?> GetPropertyById(int propertyId);
    public Task CreateProperty(Property property);
    public Task UpdateProperty(Property property);
    public Task DeleteProperty(Property property);
}