namespace MoviesAdmin.Services
{
    public interface IAzureStorageService
    {
        // Service to upload file to azure blob service
        // returns: URL of the uploaded file as string
        public Task<string> UploadFileAsync(IFormFile file, string fileName);
    }
}
