using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using EduLink.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace EduLink.Repositories.Services
{
    public class FileService : IFile
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> SaveFileAsync(IFormFile file, string[] allowedFileExtentions)
        {
            if(file == null)
            {
                return null; 
            }
            var ContentPath = _environment.ContentRootPath;
            var path = Path.Combine(ContentPath, "Uploads");
            if (!Directory.Exists(path)) { 
               Directory.CreateDirectory(path);
             }
            var ext = Path.GetExtension(file.FileName);
            if (!allowedFileExtentions.Contains(ext)) { 
                throw new Exception("This Extension Not Allowed ):");

            }
            var fileName = $"{Guid.NewGuid().ToString()}{ext}";
            var fileNameWithPath = Path.Combine(path, fileName);
             using  var fileStream = new FileStream(fileNameWithPath , FileMode.Create);
            await file.CopyToAsync(fileStream);
            return fileName;
        }
        public async Task DeleteFileAsync(string fileNameWithExtensions)
        {
            if (string.IsNullOrEmpty(fileNameWithExtensions))
            {
                throw new ArgumentNullException(nameof(fileNameWithExtensions));
            }

            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads", fileNameWithExtensions);

           

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found.", fileNameWithExtensions);
            }

            try
            {
                // Attempt to delete the file
                File.Delete(path);
            }
            catch (Exception ex)
            {
                throw;
            }

            await Task.CompletedTask; 
        }


    }
}
