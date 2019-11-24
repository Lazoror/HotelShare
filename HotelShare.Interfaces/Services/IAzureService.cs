using System.IO;
using System.Threading.Tasks;

namespace HotelShare.Interfaces.Services
{
    public interface IAzureService
    {
        Task<byte[]> DownloadAndRead(string blobFileName);

        Task<string> UploadAsync(Stream file, string fileName);

        string UploadSync(Stream file, string fileName);
    }
}