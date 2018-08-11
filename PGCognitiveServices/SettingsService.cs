using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PGCognitiveServices
{
    public static class SettingsService
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string VisionAPIKey
        {
            get => AppSettings.GetValueOrDefault(nameof(VisionAPIKey), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(VisionAPIKey), value);
        }

        public static string VisionAPIEndpoint
        {
            get => AppSettings.GetValueOrDefault(nameof(VisionAPIEndpoint), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(VisionAPIEndpoint), value);
        }

        public static bool VisionAPIConfigured
        {
            get
            {
                return (!String.IsNullOrEmpty(VisionAPIKey) && !String.IsNullOrEmpty(VisionAPIEndpoint));
            }
        }

        public static string FaceAPIKey
        {
            get => AppSettings.GetValueOrDefault(nameof(FaceAPIKey), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(FaceAPIKey), value);
        }

        public static string FaceAPIEndpoint
        {
            get => AppSettings.GetValueOrDefault(nameof(FaceAPIEndpoint), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(FaceAPIEndpoint), value);
        }

        public static bool FaceAPIConfigured
        {
            get
            {
                return (!String.IsNullOrEmpty(FaceAPIKey) && !String.IsNullOrEmpty(FaceAPIEndpoint));
            }
        }
    }
}
