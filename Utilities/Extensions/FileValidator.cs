using ProniaMVCProject.Utilities.Enums;

namespace ProniaMVCProject.Utilities.Extensions
{
    public static class FileValidator
    {   //                                              istenilen fyle
        public static bool ValidateType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }
        public static bool ValidateSize(this IFormFile file, FlieSize fileSize, int size)
        {
            switch (fileSize)
            {
                case FlieSize.KB:
                    return file.Length <= size * 1024;
                case FlieSize.MB:
                    return file.Length <= size * 1024 * 1024;
                case FlieSize.GB:
                    return file.Length <= size * 1024 * 1024 * 1024;
            }
            return false;
        }
        //                                                                folders
        public static async Task<string> CreateFileAsync(this IFormFile file, params string[] roots)
        {
            string fileName = string.Concat(Guid.NewGuid().ToString(), file.FileName.Substring(file.FileName.LastIndexOf(".")));
            string path = _getPath(roots);
            path = Path.Combine(path, fileName);


            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);

            }
            return fileName;
            //gonderilen phtonu yuxaridaki streamin adresine yukleyir
        }

        public static void DeleteFile(this string fileName, params string[] roots)
        {
            string path = _getPath(roots);
            path = Path.Combine(path, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        //private methodlarin destxet adi
        private static string _getPath(params string[] roots)
        {
            string path = string.Empty;

            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }
            return path;
        }


    }

}
