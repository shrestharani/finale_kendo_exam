using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KENDO_PRACTICE.Models
{
    public class CustomerModel
    {
        public int c_id { get; set; }   
        public string c_username {get; set;}
        public string c_email { get; set; }
        public string c_password { get; set; }
        public string c_role { get; set; }
        public string ConfirmPassword { get; set; }
    }
}