using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Device2DeviceFileIO.Interfaces;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.iOS.Classes;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.DependencyAttribute(typeof(ShareHandler))]
namespace Device2DeviceFileIO.iOS.Classes
{
    class ShareHandler : IShareHandler
    {
        public event EventHandler ShareFileRequestReceived = delegate { };

        public IFileHandler FileHandler { protected get; set; }
        private TransferFile SharedFile { get; set; }

        private void OnShareFileRequestReceived()
        {
            ShareFileRequestReceived(this, new EventArgs());
        }

        // MUST BE CALLED FROM THE UI THREAD
        public async void ProvideFile(TransferFile transferFile)
        {
            var items = new NSObject[] { NSObject.FromObject(transferFile.Name), NSUrl.FromFilename(transferFile.StoragePath) };
            var activityController = new UIActivityViewController(items, null);
            var vc = GetPresentedViewController();

            NSString[] excludedActivityTypes = null;

            if (excludedActivityTypes != null && excludedActivityTypes.Length > 0)
                activityController.ExcludedActivityTypes = excludedActivityTypes;

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                if (activityController.PopoverPresentationController != null)
                {
                    activityController.PopoverPresentationController.SourceView = vc.View;
                }
            }
            await vc.PresentViewControllerAsync(activityController, true);
        }

        public TransferFile ReceiveFile()
        {
            OnShareFileRequestReceived();
            throw new NotImplementedException();
        }

        UIViewController GetPresentedViewController()
        {
            var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (rootController.PresentedViewController == null)
                return rootController;

            if (rootController.PresentedViewController is UINavigationController)
            {
                return ((UINavigationController)rootController.PresentedViewController).TopViewController;
            }

            if (rootController.PresentedViewController is UITabBarController)
            {
                return ((UITabBarController)rootController.PresentedViewController).SelectedViewController;
            }

            return rootController.PresentedViewController;
        }

    }
}