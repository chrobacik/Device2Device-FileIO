using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.Interfaces;
using Device2DeviceFileIO.UI.View;
using Xamarin.Forms;

namespace Device2DeviceFileIO
{
    public partial class App : Application
    {
        private static ICloudFileService mFileServiceInstance;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new TransferFileOverviewPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static ICloudFileService GetCloudFileService()
        {
            if (mFileServiceInstance == null)
            {
                mFileServiceInstance = new FileIO.FileService(new PlaceboFileCryptor());
            }

            return mFileServiceInstance;
        }
    }
}
