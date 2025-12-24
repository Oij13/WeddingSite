using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using WeddingSite.Data;
using WeddingSite.Data.Models;

namespace WeddingSite.Services
{
    public interface IPhotoService
    {
        Task<List<Photo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Photo> SaveAsync(IBrowserFile file, long maxFileSize, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    }
    public class PhotoService : IPhotoService
    {
        private readonly WeddingDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedContentTypes = new[] { "image/png", "image/jpeg", "image/gif", "image/webp" };

        public PhotoService(WeddingDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<List<Photo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Photos
                .OrderByDescending(p => p.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<Photo> SaveAsync(IBrowserFile file, long maxFileSize, CancellationToken cancellationToken = default)
        {
            if (file is null) throw new ArgumentNullException(nameof(file));
            if (file.Size <= 0) throw new ArgumentException("Empty file.", nameof(file));
            if (file.Size > maxFileSize) throw new ArgumentException($"File exceeds {maxFileSize} bytes.", nameof(file));
            if (!_allowedContentTypes.Contains(file.ContentType)) throw new ArgumentException($"Unsupported content type {file.ContentType}.", nameof(file));

            var webRoot = _env.WebRootPath;
            if (string.IsNullOrEmpty(webRoot))
                throw new InvalidOperationException("Web root path is not available.");

            var imagesFolder = Path.Combine(webRoot, "images");
            Directory.CreateDirectory(imagesFolder);

            var ext = Path.GetExtension(file.Name);
            var uniqueName = $"{Guid.NewGuid():N}{ext}";
            var physicalPath = Path.Combine(imagesFolder, uniqueName);

            await using (var fs = File.Create(physicalPath))
            {
                await file.OpenReadStream(maxFileSize, cancellationToken).CopyToAsync(fs, cancellationToken);
            }

            var relativePath = Path.Combine("images", uniqueName).Replace("\\", "/");
            var photo = new Photo
            {
                Path = relativePath,
                FileName = file.Name,
                ContentType = file.ContentType
            };

            _db.Photos.Add(photo);
            await _db.SaveChangesAsync(cancellationToken);

            return photo;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var photo = await _db.Photos.FindAsync(new object[] { id }, cancellationToken);
            if (photo is null) return;

            // remove physical file if exists
            try
            {
                var webRoot = _env.WebRootPath;
                if (!string.IsNullOrEmpty(webRoot))
                {
                    var physical = Path.Combine(webRoot, photo.Path.Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (File.Exists(physical))
                        File.Delete(physical);
                }
            }
            catch
            {
                // ignore file system errors for delete
            }

            _db.Photos.Remove(photo);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
