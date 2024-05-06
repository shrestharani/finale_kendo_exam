using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KENDO_PRACTICE.Models
{
    public class AlbumModel
    {
                public int c_id { get; set; }
        public string c_album { get; set; }
        public string c_genre { get; set; }
        public string c_artist { get; set; }
        public string c_title { get; set; }
        public string c_price { get; set; }
        public IFormFile image { get; set; }
    }
}