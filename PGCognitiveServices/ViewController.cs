using System;
using Foundation;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using UIKit;
using PGCognitiveServices.Helpers;
using CoreGraphics;
using CoreAnimation;

namespace PGCognitiveServices
{
    public partial class ViewController : UIViewController, IUIAlertViewDelegate
    {

        private MediaFile image;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            this.Title = "Using Microsoft Cognitive Services API with Xamarin IOS";

            SetSendButtonStates(false);

            var width = UIScreen.MainScreen.Bounds.Width * 0.45;
            var height = UIScreen.MainScreen.Bounds.Height * 0.45;

            CGSize imageSize = new CGSize(width, height);
            CGPoint imageLoc = CapturedImage.Frame.Location;

            CapturedImage.Frame = new CGRect(imageLoc, imageSize);

            CapturedImage.Layer.BorderColor = UIColor.Purple.CGColor;
            CapturedImage.Layer.BorderWidth = 2.0f;

            ImageData.Layer.BorderColor = UIColor.Purple.CGColor;
            ImageData.Layer.BorderWidth = 2.0f;

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
                    PhotoSize = PhotoSize.Custom, 
                    CustomPhotoSize = 45,
                    DefaultCamera = camera
                });


                if (image != null)
                {
                    NSData imageData = NSData.FromStream(image.GetStream());
                    CapturedImage.Image = UIImage.LoadFromData(imageData);

                    SetSendButtonStates(true);
                }
            };

            ClearPicture.TouchUpInside += (s, e) => 
            {
                image = null;
                CapturedImage.Image = null;
                SetSendButtonStates(false);

                ImageData.Text = string.Empty;
            };

            Describe.TouchUpInside += async (s, e) =>
            {
                ImageData.Text = string.Empty;

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
                ImageData.Text = string.Empty;

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

            Emotion.TouchUpInside += async (sender, e) => 
            {
                ImageData.Text = string.Empty;

                FaceService faceClient = new FaceService();
                var analysis = await faceClient.DetectAsync(image.GetStream());

                var data = "";

                UIColor[] borderColors = { UIColor.Red, UIColor.Green, UIColor.Purple, UIColor.Yellow };
                string[] borderColorStrings = { "Red", "Green", "Purple", "Yellow" };

                //counter to select the correct border color
                int count = 0;
                foreach(Helpers.Face f in analysis)
                {
                    var scale = UIScreen.MainScreen.Scale;
                    CGRect faceBorder = new CGRect(f.FaceRectangle.Left, f.FaceRectangle.Top, f.FaceRectangle.Width, f.FaceRectangle.Height);

                    CapturedImage.Image = CapturedImage.Image.DrawRectangleOnImage(faceBorder, borderColors[count]);

                    data += "Face ID: " + f.FaceId + "\n";
                    data += "Border Color: " + borderColorStrings[count] + "\n";
                    data += f.FaceAttributes.age.ToString() + "yo " + f.FaceAttributes.gender + "\n\n";

                    data += "Anger: " + Math.Round(f.FaceAttributes.emotion.anger * 100, 2).ToString() + "%\n";
                    data += "Contempt: " + Math.Round(f.FaceAttributes.emotion.contempt * 100, 2).ToString() + "%\n";
                    data += "Disgust: " + Math.Round(f.FaceAttributes.emotion.disgust * 100, 2).ToString() + "%\n";
                    data += "Fear: " + Math.Round(f.FaceAttributes.emotion.fear * 100, 2).ToString() + "%\n";
                    data += "Happiness: " + Math.Round(f.FaceAttributes.emotion.happiness * 100, 2).ToString() + "%\n";
                    data += "Neutral: " + Math.Round(f.FaceAttributes.emotion.neutral * 100, 2).ToString() + "%\n";
                    data += "Sadness: " + Math.Round(f.FaceAttributes.emotion.sadness * 100, 2).ToString() + "%\n";
                    data += "Surprise: " + Math.Round(f.FaceAttributes.emotion.surprise * 100, 2).ToString() + "%\n";

                    data += "\n\n";

                    //increment counter for border colors
                    count++;
                }

                ImageData.Text = data;
            };
        }

        private void ToggleCameraSwitches(object sender, EventArgs e)
        {
            UISwitch theSwitch = (UISwitch)sender;

            if(theSwitch == this.Front)
            {
                if (theSwitch.On)
                {
                    Back.SetState(false, true);
                }else
                {
                    Back.SetState(true, true);
                }
            }else if(theSwitch == this.Back)
            {
                if (theSwitch.On)
                {
                    Front.SetState(false, true);
                }
                else
                {
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
