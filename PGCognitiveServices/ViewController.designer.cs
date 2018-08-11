// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace PGCognitiveServices
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch Back { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView CapturedImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CaptureImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Describe { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Emotion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch Front { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView ImageData { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton OCR { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Back != null) {
                Back.Dispose ();
                Back = null;
            }

            if (CapturedImage != null) {
                CapturedImage.Dispose ();
                CapturedImage = null;
            }

            if (CaptureImage != null) {
                CaptureImage.Dispose ();
                CaptureImage = null;
            }

            if (Describe != null) {
                Describe.Dispose ();
                Describe = null;
            }

            if (Emotion != null) {
                Emotion.Dispose ();
                Emotion = null;
            }

            if (Front != null) {
                Front.Dispose ();
                Front = null;
            }

            if (ImageData != null) {
                ImageData.Dispose ();
                ImageData = null;
            }

            if (OCR != null) {
                OCR.Dispose ();
                OCR = null;
            }
        }
    }
}