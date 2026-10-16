using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MoviesAdmin.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient blobServiceClient;
        private string containerName;

        //Constructor
        public AzureStorageService(IConfiguration configuration)
        {
            //configuration gets appsettings, user-secrets, etc.
            _configuration = configuration;

            // get azure container name From appsettings.json
            containerName = _configuration["AzureStorage:ContainerName1"] ?? throw new InvalidOperationException("Missing configuration: AzureStorage;ContainerName");

            // get azure connection string from appsettings
            string connectionString = _configuration["AzureStorage:ConnectionString"] ?? throw new InvalidOperationException("Missing configuration: AzureStorage:ConnectionString");

            //initialize aszure blob client
            blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            //get client for containerName
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);


            var blobClient = containerClient.GetBlobClient(fileName);

            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                }
            };

            //File I/O
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, options);
            }

            //return url for saved file as string
            return blobClient.Uri.ToString();
        }
    }
}
