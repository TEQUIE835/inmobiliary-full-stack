using inmobiliaryApi.Domain.Entities;

namespace inmobiliaryApi.Domain.Interfaces;

public interface IImageRepository
{
    public Task<Image?> GetImage(int id);
    public Task CreateImage(Image image);
    public Task DeleteImage(Image image);
}