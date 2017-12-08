using System;
using System.Collections.Generic;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.UI.ViewModel;
using Xamarin.Forms;

namespace Device2DeviceFileIO.UI.View
{
    public partial class TransferFileDownloadPage : ContentPage
    {
        public TransferFileDownloadVm ViewModel { get; set; }

        public TransferFileDownloadPage(TransferFile downloadTransferFile, QRCode qRCode)
        {
            InitializeComponent();

            Title = "Datei herunterladen";

            ViewModel = new TransferFileDownloadVm(downloadTransferFile, qRCode);
            ViewModel.Navigation = Navigation;

            BindingContext = ViewModel;

            //Subscribe
            MessagingCenter.Subscribe<object, string>(this, "QRCodeScanned", (sender, text) => {
                Console.WriteLine(text);
            });
        }
    }
}
