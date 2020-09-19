using System;
using MMALSharp.Common.Utility;

namespace Home.Director.Api
{
    public class Settings
    {
        public const string EnabledKeyName = "Settings:Enabled";
        public const string FrequencyKeyName = "Settings:Frequency";
        public const string FlipVerticalKeyName = "Settings:FlipVertical";
        public const string FlipHorizontalKeyName = "Settings:FlipHorizontal";
        public const string RawPathKeyName = "Settings:RawPath";
        public const string ReadyToUploadPathKeyName = "Settings:ReadyToUploadPath";
        public const string ArchivePathKeyName = "Settings:ArchivePath";
        public const string Label = "Home";

        public int Frequency { get; set; }
        public Resolution Resolution { get; set; }
        public bool FlipVertical { get; set; }
        public bool FlipHorizontal { get; set; }
        public string RawPath { get; set; }
        public string ReadyToUploadPath { get; set; }
        public string ArchivePath { get; set; }
        public bool Enabled { get; set; }
    }
}
