using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KENDO_PRACTICE.Models
{
    public class CheckoutModel
    {
         public int c_id { get; set; }
        public string c_firstname { get; set; }
        public string c_lastname { get; set; }
        public string c_address { get; set; }
        public string c_city { get; set; }
        public string c_state { get; set; }
        public string c_postal_code { get; set; }
        public string? c_country { get; set; }
        public string? c_phone { get; set; }
        public string? c_email_address { get; set; }
        public string? c_shipping_priority { get; set; }
        public DateTime? c_shipping_date { get; set; }
        public int? c_user_id { get; set; }
        public string? c_total { get; set; }
    }
}