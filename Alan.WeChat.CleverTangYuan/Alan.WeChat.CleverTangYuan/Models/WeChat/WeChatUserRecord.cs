using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alan.WeChat.CleverTangYuan.Models
{
    public static class WeChatUserRecordEx
    {
        public static global::WeChat.Core.Messages.Normal.TextResponse ToTextResponse(this IEnumerable<WeChatUserRecord> models)
        {
            var records = models
                        .OrderByDescending(record => record.CreateDate)
                        .ToList()
                        .Select(record => String.Format("{0}: {1}", record.CreateDate.ToUniversalTime(), record.Content));

            var content = String.Join(Environment.NewLine, records);
            var contentCount = System.Text.Encoding.UTF8.GetByteCount(content);
            if (contentCount > 2000)
            {
                content = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(content).Take(2000).ToArray());
            }

            content = String.IsNullOrWhiteSpace(content) ? "Empty" : content;

            return new global::WeChat.Core.Messages.Normal.TextResponse()
            {
                Content = content,
                MsgType = global::WeChat.Core.Utils.Configurations.Current.MessageType.Text
            };
        }
    }
    public partial class WeChatUserRecord
    {
        public void Push()
        {
            WeChatUser user = new WeChatUser()
            {
                OpenId = this.OpenId,
                Category = "Todo"
            };
            user.InsertIfNotExist();

            this.Category = user.Category;
            this.CreateDate = DateTime.Now;
            var context = new AliSqlContext();
            context.WeChatUserRecords.InsertOnSubmit(this);
            context.SubmitChanges();
        }

        public IEnumerable<WeChatUserRecord> Query(string openId, int skip, int take)
        {
            var user = new WeChatUser { OpenId = openId };
            user.InsertIfNotExist();
            var context = new AliSqlContext();
            return context.WeChatUserRecords.Where(rec => rec.OpenId == openId && rec.Category == user.Category)
                .Skip(skip)
                .Take(take);
        }
        public IEnumerable<WeChatUserRecord> Query(string openId, string keywords)
        {
            var user = new WeChatUser { OpenId = openId };
            user.InsertIfNotExist();
            var context = new AliSqlContext();
            return context.WeChatUserRecords.Where(rec => rec.OpenId == openId && rec.Category == user.Category && rec.Content.Contains(keywords));
        }

    }
}