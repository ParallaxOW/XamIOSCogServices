using Foundation;
using System;
using UIKit;

namespace PGCognitiveServices
{
    public partial class SettingsViewController : UIViewController
    {
        public SettingsViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            VisionKey.Text = SettingsService.VisionAPIKey;
            VisionEndpoint.Text = SettingsService.VisionAPIEndpoint;

            FaceKey.Text = SettingsService.FaceAPIKey;
            FaceEndpoint.Text = SettingsService.FaceAPIEndpoint;

            btnSaveSettings.TouchUpInside += (sender, e) => 
            {
                SettingsService.VisionAPIKey = VisionKey.Text;
                SettingsService.VisionAPIEndpoint = VisionEndpoint.Text;

                SettingsService.FaceAPIKey = FaceKey.Text;
                SettingsService.FaceAPIEndpoint = FaceEndpoint.Text;

                this.DismissViewController(true, null);
            };

        }
    }
}