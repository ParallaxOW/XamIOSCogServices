// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace PGCognitiveServices
{
    [Register ("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSaveSettings { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField FaceEndpoint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField FaceKey { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField VisionEndpoint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField VisionKey { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnSaveSettings != null) {
                btnSaveSettings.Dispose ();
                btnSaveSettings = null;
            }

            if (FaceEndpoint != null) {
                FaceEndpoint.Dispose ();
                FaceEndpoint = null;
            }

            if (FaceKey != null) {
                FaceKey.Dispose ();
                FaceKey = null;
            }

            if (VisionEndpoint != null) {
                VisionEndpoint.Dispose ();
                VisionEndpoint = null;
            }

            if (VisionKey != null) {
                VisionKey.Dispose ();
                VisionKey = null;
            }
        }
    }
}