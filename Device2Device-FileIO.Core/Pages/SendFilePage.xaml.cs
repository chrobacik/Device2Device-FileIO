using System;
using System.Collections.Generic;
using Device2DeviceFileIO.Interfaces;
using Xamarin.Forms;

namespace Device2DeviceFileIO.Pages
{
    public partial class SendFilePage : ContentPage
    {
        public SendFilePage()
        {
            InitializeComponent();
            Title = "Send File";

            btnSelectFile.Clicked += SelectFileClicked;
        }

        async void SelectFileClicked(object sender, EventArgs e)
        {
            String _path = await DependencyService.Get<IFilePicker>().GetFilePathAsync();
            Console.WriteLine("Selected file path: {0}", _path);

            imgFile.Source = ImageSource.FromFile(_path);
        }
    }
}
