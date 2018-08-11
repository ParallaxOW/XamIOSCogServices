using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PGCognitiveServices.Helpers
{
    public class Face
    {
        public Guid FaceId { get; set; }
        public FaceRectangle FaceRectangle { get; set; }
        public FaceAttributes FaceAttributes { get; set; }
    }

    public class FaceRectangle
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

    }

    public class HeadPose
    {
        public float pitch { get; set; }
        public float roll { get; set; } 
        public float yaw { get; set; }
    }

    public class FacialHair
    {
        public float moustache { get; set; }
        public float beard { get; set; }
        public float sideburns { get; set; }
    }

    public class Emotion
    {
        public float anger { get; set; }
        public float contempt { get; set; }
        public float disgust { get; set; }
        public float fear { get; set; }
        public float happiness { get; set; }
        public float neutral { get; set; }
        public float sadness { get; set; }
        public float surprise { get; set; }
    }

    public class Blur 
    {
        public string blurLevel { get; set; }
        public float value { get; set; }
    }

    public class Exposure
    {
        public string exposureLevel { get; set; }
        public float value { get; set; }
    }

    public class Noise
    {
        public string noiseLevel { get; set; }
        public float value { get; set; }
    }

    public class Makeup
    {
        public bool eyeMakeup { get; set; }
        public bool lipMakeup { get; set; }
    }

    public class Occlusion
    {
        public bool foreheadOccluded { get; set; }
        public bool eyeOccluded { get; set; }
        public bool mouthOccluded { get; set; }
    }

    public class HairColor
    {
        public string color { get; set; }
        public float confidence { get; set; }
    }

    public class Hair
    {
        public float bald { get; set; }
        public bool invisible { get; set; }
        public List<HairColor> hairColor { get; set; }
    }

    public class FaceAttributes
    {
        public float smile { get; set; }
        public HeadPose headPose { get; set; }
        public string gender { get; set; }
        public float age { get; set; }
        public FacialHair facialHair { get; set; }
        public bool glasses { get; set; }
        public Emotion emotion { get; set; }
        public Blur blur { get; set; }
        public Exposure exposure { get; set; }
        public Noise noise { get; set; }
        public Makeup makeup { get; set; }
        public Occlusion occlusion { get; set; }
        public Hair hair { get; set; }
    }

}