using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.IO;
using System.Web.Hosting;
using WeChat.YsdActivity.Models;

namespace WeChat.YsdActivity.Library
{
    public static class SqlUtils
    {
        private static string _SqlConnection;
        public static void Init()
        {
            var configFilePath = HostingEnvironment.MapPath("~/App_Data/Config.json");
            var configJson = File.ReadAllText(configFilePath);
            dynamic jObj = Newtonsoft.Json.Linq.JObject.Parse(configJson);
            var sqlCnString = (string)jObj.SqlConnection;
            if (String.IsNullOrWhiteSpace(sqlCnString))
                throw new Exception("Sql Connection is empty");

            _SqlConnection = sqlCnString;

            Player.Init();
            Vote.Init();
        }
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_SqlConnection);
        }

    }
}