using System.Net.Mime;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using inmobiliaryApi.Domain.Entities;
using inmobiliaryApi.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace inmobiliaryApi.Application.Services;

public class ImageService
{
    private readonly Cloudinary _cloudinary;
    private readonly IImageRepository _imageRepository;

    public ImageService(IConfiguration configuration,  IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
        
        var cloudName = configuration["Cloudinary:CloudName"];
        var apiKey = configuration["Cloudinary:ApiKey"];
        var apiSecret = configuration["Cloudinary:ApiSecret"];
        
        var account = new  Account(apiKey, apiSecret, cloudName);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<Image> uploadImage(IFormFile file, int propertyId)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Debe seleccionar un archivo valido.");

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "inmobiliary"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        var image = new Image
        {
            Url = uploadResult.SecureUrl.ToString(),
            PublicId = uploadResult.PublicId,
            PropertyId = propertyId
        };
        await _imageRepository.CreateImage(image);
        return image;
    }

    public async Task<bool> deleteImage(int id)
    {
        var image = await _imageRepository.GetImage(id);
        if (image == null)
            return false;

        var deleteParams = new DeletionParams(image.PublicId);
        await _cloudinary.DestroyAsync(deleteParams);
        
        await _imageRepository.DeleteImage(image);
        return true;
    }
}