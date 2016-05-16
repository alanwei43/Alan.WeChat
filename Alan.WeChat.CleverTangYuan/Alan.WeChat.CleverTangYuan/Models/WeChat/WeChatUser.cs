using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alan.Log.Core;
using Alan.Log.LogContainerImplement;

namespace Alan.WeChat.CleverTangYuan.Models
{
    public partial class WeChatUser
    {
        public void InsertIfNotExist()
        {
            var context = new AliSqlContext();
            var dbUser = context.WeChatUsers.FirstOrDefault(u => u.OpenId == this.OpenId);
            if (dbUser != null)
            {
                this.Category = dbUser.Category;
                this.CreateDate = dbUser.CreateDate;
                this.Id = dbUser.Id;
                return;
            }

            this.CreateDate = DateTime.Now;
            this.Category = "Todo";
            context.WeChatUsers.InsertOnSubmit(this);
            context.SubmitChanges();
        }
    }
}