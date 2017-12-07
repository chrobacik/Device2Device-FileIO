using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Device2DeviceFileIO.UI.ViewModel;

namespace Device2DeviceFileIO.UI.View
{
    public partial class TransferFileOverviewPage : ContentPage
    {
        public TransferFileOverviewVm ViewModel { get; set; }

        public TransferFileOverviewPage()
        {
            InitializeComponent();

            Title = "Device2Device FileIO";

            ViewModel = new TransferFileOverviewVm();
            ViewModel.Navigation = Navigation;

            BindingContext = ViewModel;
        }
    }
}
