using System;
using CoreGraphics;
using UIKit;

namespace PGCognitiveServices.Helpers
{
    public static class ImageExtension
    {
        public static UIImage DrawRectangleOnImage(this UIImage image, CGRect rect, UIColor strokeColor)
        {
            var imageSize = image.Size;
            var scale = (float)0;

            //UIGraphics.BeginImageContextWithOptions(imageSize, false, scale);
            UIGraphics.BeginImageContext(imageSize);
            var context = UIGraphics.GetCurrentContext();

            image.Draw(new CGPoint(0, 0));
            strokeColor.SetStroke();

            context.AddRect(rect);
            context.DrawPath(CGPathDrawingMode.Stroke);

            var newImage = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();
            return newImage;
        }
    }
}
