using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api
{

    public class QueryMenusWrapperModel
    {
        public List<QueryMenuModel> Button { get; set; }
    }

    public class QueryMenuModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }
        public List<QueryMenuModel> Sub_Button { get; set; }
    }
}
