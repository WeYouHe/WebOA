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
    
    public partial class ControllerAction
    {
        public int ID { get; set; }
        public string ActionTitle { get; set; }
        public int ParentID { get; set; }
        public string href { get; set; }
        public int RoleID { get; set; }
        public Nullable<int> ChildID { get; set; }
        public Nullable<int> ControllID { get; set; }
        public System.DateTime Update_time { get; set; }
    }
}
