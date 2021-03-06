﻿using System;
using System.Collections.Generic;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.UI.ViewModel;
using Xamarin.Forms;

namespace Device2DeviceFileIO.UI.View
{
    public partial class TransferFileUploadPage : ContentPage
    {
        public TransferFileUploadVm ViewModel { get; set; }
        
        public TransferFileUploadPage(TransferFile uploadTransferFile, QRCode qRCode)
        {
            InitializeComponent();

            Title = "Datei hochladen";

            ViewModel = new TransferFileUploadVm(uploadTransferFile, qRCode);
            ViewModel.Navigation = Navigation;

            BindingContext = ViewModel;

            App.GetCloudFileService().UploadFinished += ViewModel.TransferFileUploadVm_UploadFinished;

            this.Disappearing += TransferFileUploadPage_Disappearing;
        }

        private void TransferFileUploadPage_Disappearing(object sender, EventArgs e)
        {
            App.GetCloudFileService().UploadFinished -= ViewModel.TransferFileUploadVm_UploadFinished;
        }
    }
}
