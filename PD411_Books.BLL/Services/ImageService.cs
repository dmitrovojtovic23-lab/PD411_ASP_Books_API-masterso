using Microsoft.AspNetCore.Http;

namespace PD411_Books.BLL.Services
{
    public class ImageService
    {
        public async Task<ServiceResponse> SaveAsync(IFormFile file, string dirPath)
        {
            try
            {
                var types = file.ContentType.Split("/");

                if(types.Length != 2 || types[0] != "image")
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = $"Файл '{file.FileName}' не є зображенням"
                    };
                }

                //string imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string imageName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string imagePath = Path.Combine(dirPath, imageName);

                using var fileStream = File.OpenWrite(imagePath);
                await file.CopyToAsync(fileStream);

                return new ServiceResponse
                {
                    Message = $"Зображення '{file.FileName}' успішно збережено",
                    Payload = imageName
                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public ServiceResponse Delete(string imagePath)
        {
            if(File.Exists(imagePath))
            {
                File.Delete(imagePath);
                return new ServiceResponse
                {
                    Message = "Зображення успішно видалено"
                };
            }

            return new ServiceResponse
            {
                Success = false,
                Message = $"Зображення '{imagePath}' не знайдено"
            };
        }
    }
}
