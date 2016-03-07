using Alan.Utils.ExtensionMethods;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Alan.Log.Core;

namespace WeChat.Example.Library
{
    public class DbLog : Alan.Log.Core.ILog
    {
        public void Write(
            string id,
            DateTime date,
            string level,
            string logger,
            string category,
            string message,
            string note,
            string request,
            string response,
            string position)
        {
            if (date == default(DateTime)) date = DateTime.Now;

            try
            {
                SqlUtils.Insert(tableName: "LogInfos", model: new
                {
                    Id = id,
                    Date = date,
                    Level = level,
                    Category = category,
                    Message = message,
                    Request = request,
                    Response = response,
                    Note = note,
                    Position = position
                });
            }
            catch (Exception ex)
            {
                var logFile = new Alan.Log.ILogImplement.LogSingleFile(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/sql-error-log.log"));
                logFile.Log(
                    date: DateTime.Now,
                    message: ex.Message,
                    note: ex.StackTrace,
                    position: ex.Source,
                    logger: MyConfig.Current.SqlConnection);
            }
        }
    }

}
