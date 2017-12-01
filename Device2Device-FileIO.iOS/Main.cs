using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace Device2DeviceFileIO.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // Generellen Error-Handler einbinden und Fehler auf Konsole ausgeben
            try {
                // if you want to use a different Application Delegate class from "AppDelegate"
                // you can specify it here.
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
