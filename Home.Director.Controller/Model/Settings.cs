using System;
namespace Home.Director.Controller.Model
{
    public class Settings
    {
        public int Frequency { get; set; }
        public bool FlipVertical { get; set; }
        public bool FlipHorizontal { get; set; }
        public string RawPath { get; set; }
        public string ReadyToUploadPath { get; set; }
        public string ArchivePath { get; set; }
        public bool Enabled { get; set; }
    }
}
