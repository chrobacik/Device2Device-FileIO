using System;
using System.Collections.Generic;
using Device2DeviceFileIO.UI.ViewModel;
using Xamarin.Forms;

namespace Device2DeviceFileIO.UI.View
{
    public partial class TransferFileDownloadPage : ContentPage
    {
        public TransferFileDownloadVm ViewModel { get; set; }

        public TransferFileDownloadPage()
        {
            InitializeComponent();

            Title = "Scan tranfer file";

            ViewModel = new TransferFileDownloadVm();
            ViewModel.Navigation = Navigation;

            BindingContext = ViewModel;

            //Subscribe
            MessagingCenter.Subscribe<object, string>(this, "QRCodeScanned", (sender, text) => {
                Console.WriteLine(text);
            });
        }
    }
}
