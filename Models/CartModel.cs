using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KENDO_PRACTICE.Models
{
    public class CartModel
    {
         public int c_id { get; set; }
    public int c_album_id { get; set; }
    public string c_title { get; set; }
    public string c_album_art { get; set; }
    public string c_price { get; set; }
    public int c_quantity { get; set; }
    public string c_total { get; set; }
    public int? c_user_id { get; set; }
    }
}