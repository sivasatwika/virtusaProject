using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotnetapp.Models
{
    public class ThemeModel
    {
        public int themeId { get; set; }
        public string themeName { get; set; }
        public string themeDetails { get; set; }
        public int themePrice { get; set; }
    }
}