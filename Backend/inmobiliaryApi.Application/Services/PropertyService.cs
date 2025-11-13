using inmobiliaryApi.Domain.Entities;
using inmobiliaryApi.Domain.Interfaces;

namespace inmobiliaryApi.Application.Services;

public class PropertyService
{
    private readonly IPropertyRepository _propertyRepository;

    public PropertyService(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task<IEnumerable<Property>> GetAll()
    {
        return await _propertyRepository.GetAllProperties();
    }

    public async Task<Property> GetById(int id)
    {
        return await _propertyRepository.GetPropertyById(id);
    }

    public async Task CreateProperty(Property property)
    {
        await _propertyRepository.CreateProperty(property);
    }

    public async Task UpdateProperty(Property property)
    {
        await _propertyRepository.UpdateProperty(property);
    }

    public async Task DeleteProperty(Property property)
    {
        await _propertyRepository.DeleteProperty(property);
    }
}