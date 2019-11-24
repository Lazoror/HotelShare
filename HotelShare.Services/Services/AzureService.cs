using HotelShare.Domain.Models.SqlModels;
using HotelShare.Interfaces.DAL.Data;
using HotelShare.Interfaces.DAL.RepositorySql;
using HotelShare.Interfaces.Services;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HotelShare.Services.Services
{
    public class AzureService : IAzureService
    {
        private static string _connectionString = "DefaultEndpointsProtocol=https;AccountName=hotelstorebl;AccountKey=3ZwcWtNjMBQloR5AjoIAp3zQ+W3bImtVdjx13i8fTt5hcwHQuaJ/JlUSv03iniBHMPCfKBtfNy22scNb+e56Cg==;EndpointSuffix=core.windows.net";
        private CloudBlobContainer _cloudBlobContainer;
        private readonly IRepository<Image> _imageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AzureService(IUnitOfWork unitOfWork)
        {
            //Task.Run(() => SetupConnection(_connectionString)).GetAwaiter().GetResult();
            _unitOfWork = unitOfWork;
            _imageRepository = unitOfWork.GetRepository<Image>();
        }

        public async Task<byte[]> DownloadAndRead(string blobFileName)
        {
            CloudBlockBlob blockBlob = _cloudBlobContainer.GetBlockBlobReference(blobFileName);
            byte[] content = new byte[blockBlob.StreamWriteSizeInBytes];
            await blockBlob.DownloadToByteArrayAsync(content, 0);

            return content;
        }

        public async Task<string> UploadAsync(Stream file, string fileName)
        {
            AddImage(fileName);

            CloudBlockBlob cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName);
            await cloudBlockBlob.UploadFromStreamAsync(file);

            return fileName;
        }

        public string UploadSync(Stream file, string fileName)
        {
            AddImage(fileName);

            CloudBlockBlob cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName);
            cloudBlockBlob.UploadFromStream(file);

            return fileName;
        }

        private void AddImage(string fileName)
        {
            var image = new Image
            {
                Id = Guid.NewGuid(),
                Name = fileName
            };

            _imageRepository.Insert(image);
            _unitOfWork.Commit();
        }

        private async Task SetupConnection(string connectionString)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var cloudBlobCLient = account.CreateCloudBlobClient();
            _cloudBlobContainer = cloudBlobCLient.GetContainerReference("students");

            if (await _cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await _cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
        }
    }
}