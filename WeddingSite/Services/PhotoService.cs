using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using WeddingSite.Data;
using WeddingSite.Data.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace WeddingSite.Services
{
    public interface IPhotoService
    {
        Task<List<Photo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Photo> SaveAsync(IBrowserFile file, long maxFileSize, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    }
    public class PhotoService(IDbContextFactory<WeddingDbContext> factory, Cloudinary cloudinary) : IPhotoService
    {
        private readonly IDbContextFactory<WeddingDbContext> _factory = factory;
        private readonly Cloudinary _cloudinary = cloudinary;
        private readonly string[] _allowedContentTypes = ["image/png", "image/jpeg", "image/gif", "image/webp"];

        public async Task<List<Photo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            return await db.Photos
                .OrderByDescending(p => p.UploadedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Photo> SaveAsync(IBrowserFile file, long maxFileSize, CancellationToken cancellationToken = default)
        {
            if (file is null) throw new ArgumentNullException(nameof(file));
            if (file.Size <= 0) throw new ArgumentException("Empty file.", nameof(file));
            if (file.Size > maxFileSize) throw new ArgumentException($"File exceeds {maxFileSize} bytes.", nameof(file));
            if (!_allowedContentTypes.Contains(file.ContentType)) throw new ArgumentException($"Unsupported content type {file.ContentType}.", nameof(file));

            using var stream = file.OpenReadStream(maxFileSize, cancellationToken);
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.Name, stream),
                Folder = "wedding-site",
                UseFilename = true,
                UniqueFilename = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);
            
            if (uploadResult.Error != null)
                throw new InvalidOperationException($"Cloudinary upload failed: {uploadResult.Error.Message}");

            var photo = new Photo
            {
                Url = uploadResult.SecureUrl.ToString(),
                PublicId = uploadResult.PublicId,
                FileName = file.Name,
                ContentType = file.ContentType,
                UploadedAt = DateTime.UtcNow
            };

            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            db.Photos.Add(photo);
            await db.SaveChangesAsync(cancellationToken);

            return photo;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var photo = await db.Photos.FindAsync(new object[] { id }, cancellationToken);
            if (photo is null) return;

            try
            {
                var deleteParams = new DeletionParams(photo.PublicId);
                await _cloudinary.DestroyAsync(deleteParams);
            }
            catch
            {
                // Continue with database deletion even if Cloudinary delete fails
            }

            db.Photos.Remove(photo);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
