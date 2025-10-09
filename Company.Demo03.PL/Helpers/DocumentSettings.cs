namespace Company.Demo03.PL.Helpers
{
    public static class DocumentSettings 
    {
        // 1. Upload file
        public static string UploadFile(IFormFile file, String folderName)
        {
            //1.1 Get Folder Location
            //string folderPath = "C:\\Users\\hp\\Desktop\\MVC Demos\\Demo 03\\Company\\Company.Demo03.PL\\wwwroot\\files\\images\\" + folderName;
            //var folderPah = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\" + folderName;
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files",folderName);

            //1.2 Get fileName and make it unique
            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            //1.3 File Path
            var filePath = Path.Combine(folderPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
             
            file.CopyTo(fileStream);

            return fileName;

        }

        // 2. Delete file
        public static void DeleteFile(string fileName, String folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);

            if (File.Exists(filePath)) 
            {
                File.Delete(filePath);
            }
        }
    }
}
