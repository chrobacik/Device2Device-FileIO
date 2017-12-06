using System;
using System.Collections.Generic;
using Device2DeviceFileIO.UI.ViewModel;
using Xamarin.Forms;

namespace Device2DeviceFileIO.UI.View
{
    public partial class QRCodeScanPage : ContentPage
    {
        public QRCodeScanVm ViewModel { get; set; }

        public QRCodeScanPage()
        {
            InitializeComponent();

            ViewModel = new QRCodeScanVm();
            ViewModel.Navigation = Navigation;

            BindingContext = ViewModel;
        }
    }
}
