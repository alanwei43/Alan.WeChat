using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.ContensManage
{
    public class MediumContentsModel : ContentsBaseModel
    {
        public List<MediumContentModel> item { get; set; }
    }

    public class MediumContentModel
    {
        public string media_id { get; set; }
        public string name { get; set; }
        public long update_time { get; set; }
        public string url { get; set; }
    }
}
