using Device2DeviceFileIO.Pages;
using Xamarin.Forms;

namespace Device2DeviceFileIO
{
    public partial class Device2Device_FileIOPage : ContentPage
    {
        public Device2Device_FileIOPage()
        {
            InitializeComponent();
            Title = "Device2Device File.IO";

            btnGoToSelectFile.Clicked += async (object sender, System.EventArgs e) => {

                await Navigation.PushAsync(new SendFilePage());
            };
        }
    }
}
