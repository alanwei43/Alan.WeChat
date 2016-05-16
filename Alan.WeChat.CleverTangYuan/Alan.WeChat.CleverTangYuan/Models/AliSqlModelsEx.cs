using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//sqlmetal /conn:"..." /code:AliSqlModels.cs /namespace:"Alan.WeChat.CleverTangYuan.Models" /context:AliSqlContext /pluralize 

namespace Alan.WeChat.CleverTangYuan.Models
{
    public partial class AliSqlContext
    {
        public AliSqlContext() : base(Library.GlobalConfigs.Current.AliHostSql) { }
    }
}