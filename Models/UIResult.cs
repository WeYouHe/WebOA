using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOA.Models
{
    public class UIResult
    {
        public bool success
        {
            get; set;
        }
        public string message
        {
            get; set;
        }
        public string remake
        {
            get; set;
        }
        public UIResult (bool suc, string msg)
        {
            this.success = suc;
            this.message = msg;
        }
        public UIResult (bool suc, string msg, string rmk)
        {
            this.success = suc;
            this.message = msg;
            this.remake = rmk;
        }
    }
}