using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat.YsdActivity.Library;
using Dapper;

namespace WeChat.YsdActivity.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string WeChatOpenId { get; set; }
        public string WeChatName { get; set; }
        public string WeChatAvatar { get; set; }
        public int VotedCount { get; set; }
        public static void Init()
        {
            var sql = @"
if not exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Players')
begin
create table Players(
    Id int identity(1,1) primary key,
    Avatar varchar(200) null,
    Name nvarchar(500) null,
    WeChatOpenId varchar(200) null,
    WeChatName nvarchar(500) null,
    WeChatAvatar varchar(200) null
)
end
";
            using (var connection = SqlUtils.GetConnection())
            {
                connection.Execute(sql);
            }

        }
        public void Insert()
        {
            SqlUtils.GetConnection().Execute(String.Format(@"
insert into Players(Avatar, Name, WeChatOpenId, WeChatName, WeChatAvatar)
values(@Avatar, @Name, @WeChatOpenId, @WeChatName, @WeChatAvatar)"), this);
        }
    }
}