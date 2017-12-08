using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.Interfaces;
using Device2DeviceFileIO.UI.View;
using Plugin.Connectivity;
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

            MainPage = new NavigationPage(new TransferFileOverviewPage()) {
                BarBackgroundColor = Color.FromHex("#2ebced"), BarTextColor = Color.White
            };
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

        public static bool HasConnectivity()
        {
            if (!CrossConnectivity.IsSupported)
                return true;

            //Do this only if you need to and aren't listening to any other events as they will not fire.
            var connectivity = CrossConnectivity.Current;
            try
            {
                return connectivity.IsConnected;
            }
            finally
            {
                CrossConnectivity.Dispose();
            }
        }
    }
}
