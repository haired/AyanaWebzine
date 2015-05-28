using System;
using Windows.System;

namespace AyanaWebzinePhone.Components.Sharing
{
    public class External
    {     
        public static void LaunchBrowser(Uri url)
        {
            Launcher.LaunchUriAsync(url);                
        }
    }
}
