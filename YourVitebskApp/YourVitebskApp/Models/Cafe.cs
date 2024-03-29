﻿using System.Collections.Generic;

namespace YourVitebskApp.Models
{
    public class Cafe
    {
        public int? CafeId { get; set; }
        public string CafeType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string WorkingTime { get; set; }
        public string Address { get; set; }
        public string ExternalLink { get; set; }
        public int RecommendCount { get; set; }
        public int UnrecommendCount { get; set; }
        public string TitleImage { get; set; }
        public IEnumerable<string> Images { get; set; }
    }
}
