using System;
using System.Collections.Generic;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.UI.ViewModel;
using Xamarin.Forms;

namespace Device2DeviceFileIO.UI.View
{
    public partial class QRCodeScanPage : ContentPage
    {
        public QRCodeScanVm ViewModel { get; set; }

        public QRCodeScanPage(TransferFile uploadTransferFile, QRCode qRCode)
        {
            InitializeComponent();

            Title = "Scan";

            ViewModel = new QRCodeScanVm(uploadTransferFile, qRCode);
            ViewModel.Navigation = Navigation;

            BindingContext = ViewModel;
        }
    }
}
