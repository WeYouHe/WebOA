//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebOA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int Id { get; set; }
        public string PASSWORD { get; set; }
        public string PORTRAIT { get; set; }
        public string NAME { get; set; }
        public string SEX { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string ADDRESS { get; set; }
        public Nullable<System.DateTime> BIRTHDAY { get; set; }
        public int LOGINS { get; set; }
        public int DEPAR_ID { get; set; }
        public System.DateTime CREATE_DATE { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }
        public int ROLEID { get; set; }
        public int STATUS { get; set; }
    
        public virtual Role Role { get; set; }
    }
}
