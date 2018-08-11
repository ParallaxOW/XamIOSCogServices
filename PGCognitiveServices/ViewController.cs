using System;
using Foundation;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using UIKit;

namespace PGCognitiveServices
{
    public partial class ViewController : UIViewController, IUIAlertViewDelegate
    {

        private MediaFile image;
        private CameraDevice cameraDevice;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            SetSendButtonStates(false);

            Front.TouchUpInside += ToggleCameraSwitches;
            Back.TouchUpInside += ToggleCameraSwitches;

            if (!SettingsService.FaceAPIConfigured || !SettingsService.VisionAPIConfigured)
            {
                DisplayMessage("Settings Missing!",
                               "One or more required settings values missing. Some features disabled. Set all fields in the settings screen to perform all functions.",
                               "OK");
            }

            CaptureImage.TouchUpInside += async (s, e) =>
            {
                await CrossMedia.Current.Initialize();

                CameraDevice camera = (Front.On ? CameraDevice.Front : CameraDevice.Rear);

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    DisplayMessage("Unavailable!", "Camera unavailable on this device! Cannot continue!", "OK");
                    return;
                }

                image = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    DefaultCamera = camera
                });

                NSData imageData = NSData.FromStream(image.GetStream());
                CapturedImage.Image = UIImage.LoadFromData(imageData).ResizeImageWithAspectRatio((float)CapturedImage.Frame.Width, (float)CapturedImage.Frame.Height);

                SetSendButtonStates(true);
            };

            Describe.TouchUpInside += async (s, e) =>
            {
                if (!SettingsService.VisionAPIConfigured)
                {
                    DisplayMessage(
                    "Not configured!",
                    "The Vision API is not correctly setup in the settings screen. Please set API Key and Endpoint.",
                    "OK");
                    return;
                }

                VisionServiceClient client = new VisionServiceClient(SettingsService.VisionAPIKey, SettingsService.VisionAPIEndpoint);
                VisualFeature[] features = { VisualFeature.Tags, VisualFeature.Categories, VisualFeature.Description };
                var result = await client.AnalyzeImageAsync(image.GetStream(), features);

                var analysisResult = "";

                analysisResult += "Categories:\n";

                foreach (Category c in result.Categories)
                {
                    analysisResult += string.Format("{0}  ::  {1}%\n", c.Name, Math.Round(c.Score * 100, 2).ToString());
                }

                analysisResult += "\n\nDescriptions:\n";

                foreach (Caption c in result.Description.Captions)
                {
                    analysisResult += string.Format("{0}  ::  {1}%\n", c.Text, Math.Round(c.Confidence * 100, 2).ToString());
                }

                analysisResult += "\n\nTags:\n";

                foreach (Tag t in result.Tags)
                {
                    analysisResult += string.Format("{0}  ::  {1}%\n", t.Name, Math.Round(t.Confidence * 100, 2).ToString());
                }

                ImageData.Text = analysisResult;
            };

            OCR.TouchUpInside += async (s, e) =>
            {
                if (!SettingsService.VisionAPIConfigured)
                {
                    DisplayMessage(
                    "Not configured!",
                    "The Vision API is not correctly setup in the settings screen. Please set API Key and Endpoint.",
                    "OK");
                    return;
                }

                VisionServiceClient client = new VisionServiceClient(SettingsService.VisionAPIKey, SettingsService.VisionAPIEndpoint);
                var result = await client.RecognizeTextAsync(image.GetStream());

                var readText = "";

                foreach(Region r in result.Regions)
                {
                    foreach(Line l in r.Lines)
                    {
                        foreach(Word w in l.Words)
                        {
                            readText += w.Text + " ";
                        }
                        readText += "\n";
                    }
                }

                ImageData.Text = readText;
            };

        }

        private void ToggleCameraSwitches(object sender, EventArgs e)
        {
            UISwitch theSwitch = (UISwitch)sender;

            if(theSwitch == this.Front)
            {
                if (theSwitch.On)
                {
                    cameraDevice = CameraDevice.Front;
                    Back.SetState(false, true);
                }else
                {
                    cameraDevice = CameraDevice.Rear;
                    Back.SetState(true, true);
                }
            }else if(theSwitch == this.Back)
            {
                if (theSwitch.On)
                {
                    cameraDevice = CameraDevice.Front;
                    Front.SetState(false, true);
                }
                else
                {
                    cameraDevice = CameraDevice.Rear;
                    Front.SetState(true, true);
                }
            }
        }


        private void SetSendButtonStates(bool enabled)
        {
            Describe.Enabled = enabled;
            OCR.Enabled = enabled;
            Emotion.Enabled = enabled;
        }

        private void DisplayMessage(string title, string message, string buttonText)
        {
            UIAlertView uav = new UIAlertView(title, message, this, buttonText, null);
            uav.Show();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
