using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat.YsdActivity.Library;

namespace WeChat.YsdActivity.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string PlayerOpenId { get; set; }
        public string VoterOpenId { get; set; }

        public static void Init()
        {
            var sql = @"
if not exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Votes')
begin
create table Votes(
    Id int identity(1,1) primary key,
    PlayerId int not null,
    PlayerOpenId varchar(100) not null,
    VoterOpenId varchar(100) not null,
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
insert into Votes(PlayerId, PlayerOpenId, VoterOpenId)
values(@PlayerId, @PlayerOpenId, @VoterOpenId)"), this);
        }
    }
}