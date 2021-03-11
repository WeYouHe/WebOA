using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOA.Models
{
    public class PersonView
    {
        public string ActionTitle
        {
            get; set;
        }
        public int ID
        {
            get; set;
        }
        public string href
        {
            get; set;
        }
        public string RoleName
        {
            get; set;
        }
        public string Title
        {
            get; set;
        }
        public DateTime Update_time
        {
            get; set;
        }
    }
}