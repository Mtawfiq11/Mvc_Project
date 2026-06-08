using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {

        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        private readonly string[] allowedextensions = { ".jpg", ".jpeg", ".png" };
        private readonly long maxfilesize = 5 * 1024 * 1024;
        private readonly IWebHostEnvironment _webHost;

        public string? Upload(string foldername, IFormFile file)
        {
            try
            {
                if (foldername is null || file is null || file.Length == 0) return null;
                if (file.Length > maxfilesize) return null;
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedextensions.Contains(extension)) return null;
                var folderPath = Path.Combine(_webHost.WebRootPath, "images", foldername /*member*/);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(folderPath, fileName);
                using var filestream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(filestream);
                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Upload File To folder {foldername} : {ex}");
                return null;
            }

        }

        public bool Delete(string filename, string foldername)
        {
            try
            {
                if (string.IsNullOrEmpty(foldername) || string.IsNullOrEmpty(filename))
                    return false;


                var filePath = Path.Combine(_webHost.WebRootPath, "images", foldername, filename);


                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Delete File With Name {filename} : {ex}");
                return false;

            }

        }

       
    }
}
