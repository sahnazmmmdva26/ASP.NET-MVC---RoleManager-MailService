using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data.SqlTypes;

namespace ProniaSite.Utilies
{
    public static class FileExtensions
    {
        public static bool CheckType(this IFormFile file, string type)
            => file.ContentType.Contains(type);
        public static bool CheckSize(this IFormFile file, int kb)
          => kb * 1024 < file.Length;

        public static string SaveFile(this IFormFile file, string path)
        {
            string filename = ChangeFileName(file.FileName);
            using (FileStream fs = new FileStream(Path.Combine(path, filename), FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return filename;
        }
        static string ChangeFileName(string oldname)
        {
            string extension= oldname.Substring(oldname.LastIndexOf('.'));
            if(oldname.Length<32)
            {
                oldname= oldname.Substring(0, oldname.LastIndexOf('.'));
            }
            else
            {
                oldname = oldname.Substring(0, 32);
            }
            return Guid.NewGuid() + oldname + extension;
        }

        public static void DeleteFile(this string Filename,string root, string folder)
        {
            string path= Path.Combine(root, folder,Filename);
            if(File.Exists(path))
                File.Delete(path);
        }
    }
}
