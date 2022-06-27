using System.Collections.Generic;

namespace ImageProcessor.Models
{
    public class ImageInfo
    {
        public List<string> Text { get; set; }
        public List<string> Emails { get; set; }
        public List<string> Urls { get; set; }
        public List<string> PhoneNumbers { get; set; }
    }
}
