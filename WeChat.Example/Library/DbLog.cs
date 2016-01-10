using Alan.Utils.ExtensionMethods;
using System;
using System.Diagnostics;

namespace WeChat.Example.Library
{
    public class DbLog : WeChat.Core.Log.ILog
    {
        public long AutoId { get; set; }
        public string Id { get; set; }
        public string Category { get; set; }
        public DateTime? Date { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Note { get; set; }

        public void Write()
        {
            this.Write(this.Id, this.Date.GetValueOrDefault(), this.Category, this.Request, this.Response, this.Note);
        }

        public void Write(string id, DateTime date, string category, string request, string response, string note)
        {
            Trace.WriteLine(new
            {
                Id = id,
                Date = date,
                Category = category,
                Request = request,
                Response = response,
                Note = note
            }.ExToJson());
            return;

            SqlUtils.Insert(tableName: "LogInfos", model: new
            {
                Id = id,
                Date = date,
                Category = category,
                Request = request,
                Response = response,
                Note = note
            });
        }
    }
}
