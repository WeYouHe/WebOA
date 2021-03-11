using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebOA.Models
{
    public class UIChangPwd
    {
        [Required]
        public string username
        {
            get; set;
        }
        [Required]
        public string oldpass
        {
            get; set;
        }
        [Required]
        public string newpass
        {
            get; set;
        }
        public string repass
        {
            get; set;
        }
    }
}