using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.AttachmentService
{
    public interface IAttachmentService
    {

        string? Upload(string foldername, IFormFile file);
        bool Delete(string filename, string foldername);

    }
}
