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


#if MAINPAGE
            MainPage = new NavigationPage(new MainPage()) {
#else
            MainPage = new NavigationPage(new TransferFileOverviewPage()) {
#endif

                BarBackgroundColor = Color.FromHex("#2ebced"),
                BarTextColor = Color.White
            };

            App.GetCloudFileService();
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

        public static TransferFile CurrentUploadFile
        {
            get => App.GetCloudFileService().CurrentUpload.File;
        }

        public static TransferFile CurrentDownloadFile
        {
            get => App.GetCloudFileService().CurrentDownload.File;
        }

        public static QRCode CurrentUploadQRCode
        {
            get => App.GetCloudFileService().CurrentUpload.Code;
        }

        public static QRCode CurrentDownloadQRCode
        {
            get => App.GetCloudFileService().CurrentDownload.Code;
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
            if (!CrossConnectivity.IsSupported) return true;

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
