using inmobiliaryApi.Domain.Entities;
using inmobiliaryApi.Domain.Interfaces;
using inmobiliaryApi.Infrastructure.Persistence;

namespace inmobiliaryApi.Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly AppDbContext _context;
    public ImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Image?> GetImage(int id)
    {
        return await _context.Images.FindAsync(id);
    }

    public async Task CreateImage(Image image)
    {
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteImage(Image image)
    {
        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
    }
}