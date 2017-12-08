using System;
using System.Collections.Generic;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.UI.ViewModel;
using Xamarin.Forms;

namespace Device2DeviceFileIO.UI.View
{
    public partial class TransferFileUploadPage : ContentPage
    {
        public TransferFileUploadVm ViewModel { get; set; }
        
        public TransferFileUploadPage(TransferFile uploadTransferFile)
        {
            InitializeComponent();

            Title = "Upload";

            ViewModel = new TransferFileUploadVm(uploadTransferFile);
            ViewModel.Navigation = Navigation;

            BindingContext = ViewModel;

        }
    }
}
