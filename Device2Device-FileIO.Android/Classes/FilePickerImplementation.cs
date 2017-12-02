using Android.Content;
using Device2DeviceFileIO.Droid.Classes;
using Device2DeviceFileIO.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(FilePickerImplementation))]
namespace Device2DeviceFileIO.Droid.Classes
{
    public class FilePickerImplementation : IFilePicker
    {
        public FilePickerImplementation()
        {
        }

        public Task<string> GetFilePathAsync()
        {
            // Create the intent to get content
            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("*/*");

            // Get the MainActivity instance
            MainActivity activity = Forms.Context as MainActivity;

            // Start the picture-picker activity (resumes in MainActivity.cs)
            activity.StartActivityForResult(Intent.CreateChooser(intent, "Select File"), MainActivity.PickImageId);

            // Save the TaskCompletionSource object as a MainActivity property
            activity.PickImageTaskCompletionSource = new TaskCompletionSource<String>();

            // Return Task object
            return activity.PickImageTaskCompletionSource.Task;
        }
    }
}
