using inmobiliaryApi.Domain.Entities;
using inmobiliaryApi.Domain.Interfaces;
using inmobiliaryApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace inmobiliaryApi.Infrastructure.Repositories;

public class PropertyRepository :  IPropertyRepository
{
    private readonly AppDbContext _context;
    public PropertyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Property>> GetAllProperties()
    {
        return await _context.Properties.Include(p => p.Images).ToListAsync();
    }

    public async Task<Property?> GetPropertyById(int propertyId)
    {
        return await _context.Properties.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == propertyId);
    }

    public async Task CreateProperty(Property property)
    {
        _context.Properties.Add(property);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProperty(Property property)
    {
        _context.Properties.Update(property);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProperty(Property property)
    {
        _context.Properties.Remove(property);
        await _context.SaveChangesAsync();
    }
}