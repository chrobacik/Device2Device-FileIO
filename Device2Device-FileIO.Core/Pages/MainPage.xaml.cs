using Device2DeviceFileIO.Pages;
using Xamarin.Forms;

namespace Device2DeviceFileIO
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Title = "Device2Device File.IO";

            btnGoToSelectFile.Clicked += async (object sender, System.EventArgs e) => {

                await Navigation.PushAsync(new SendFilePage());
            };

            btnGoToBarcodeScan.Clicked += async (object sender, System.EventArgs e) => {

                await Navigation.PushAsync(new BarcodeScanPage());
            };

            btnGoToBarcodeShow.Clicked += async (object sender, System.EventArgs e) => {

                await Navigation.PushAsync(new BarcodePage());
            };

        }
    }
}
