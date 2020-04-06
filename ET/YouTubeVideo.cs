using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ET
{
    public class YouTubeVideo
    {
        public string id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string BannerLink { get; set; }

        [Required]
        public string Tags { get; set; }

        public DateTime PublishedAt { get; set; }

        [Display(Name = "Video")]
        [Required]
        public Stream VideoData { get; set; }

        [Required]
        public string VideoExt { get; set; }
    }
}
