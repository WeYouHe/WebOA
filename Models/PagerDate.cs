using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebOA.Models
{
    public class PagerDate
    {
        public static string PagedData<T>(List<T> list, int count, String dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            StringBuilder strJson = new StringBuilder();  // StringBuilder是个动态构建字符串对象的容器，最简单的使用就是通过Append方法，不断地向容器中追加字符串。
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            settings.DateFormatString = dateTimeFormat;
            settings.MaxDepth = 1; //设置序列化外键对象时的最大层数，1表示只序列化当前对象
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; //忽略外键引用
            settings.NullValueHandling = NullValueHandling.Ignore;
            foreach (var item in list)
            {
                strJson.Append(JsonConvert.SerializeObject(item, settings) + ","); //JsonConvert.SerializeObject方法是newtonsoft组件中的方法，用于将对象转为json格式的字符串
            }
            return "{\"code\":0,\"msg\":\"\",\"count\":" + count + ",\"data\":[" + strJson.ToString().TrimEnd(',') + "]}"; //TrimEnd(',')方法，可以去掉最后多拼接进去的逗号。
        }
    }
}