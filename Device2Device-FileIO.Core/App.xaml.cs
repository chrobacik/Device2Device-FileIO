using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.Interfaces;
using Device2DeviceFileIO.Pages;
using Xamarin.Forms;

namespace Device2DeviceFileIO
{
    public partial class App : Application
    {
        private static ICloudFileService mFileServiceInstance;

        private IShareHandler _ShareHandler;
        public IShareHandler ShareHandler
        {
            get
            {
                return _ShareHandler ?? (_ShareHandler = DependencyService.Get<IShareHandler>());
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
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
                mFileServiceInstance = new FileIOFileService();
            }

            return mFileServiceInstance;
        }
    }
}
