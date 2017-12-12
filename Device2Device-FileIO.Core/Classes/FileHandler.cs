using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.Interfaces;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(FileHandler))]
namespace Device2DeviceFileIO.Classes
{
    public class FileHandler : IFileHandler
    {
        readonly string RootPath = System.IO.Path.GetTempPath();
        //System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        public void Load(TransferFile file)
        {
            var filePath = Path.Combine(RootPath, file.Name);
            file.Content = File.ReadAllBytes(filePath);
            file.StoragePath = filePath;
        }


        /// <summary>
        /// Stores file in local store, by overwriting file if exist with same name
        /// </summary>
        /// <param name="transfer"></param>
        public void Save(TransferFile file)
        {
            var filePath = Path.Combine(RootPath, file.Name);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllBytes(filePath, file.Content);
            file.StoragePath = filePath;
        }

        
    }


}