using Microsoft.AspNetCore.Hosting;   
using Microsoft.AspNetCore.Http;
namespace EduLink.Repositories.Interfaces
{
    public interface IFile
    {
        Task<string> SaveFileAsync(IFormFile? file , string[] allowedFileExtentions);
        Task DeleteFileAsync(string fileNameWithExtentions);



    }
}
