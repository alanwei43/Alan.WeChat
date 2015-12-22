using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Alan.Utils.ExtensionMethods;
using Dapper;

namespace WeChat.Example.Library
{
    public static class SqlUtils
    {

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(MyConfig.Current.SqlConnection);
        }


        public static T GetValue<T>(string sql, object parameters)
        {
            return GetConnection().ExecuteScalar<T>(sql, parameters);
        }

        public static T GetValue<T>(string sql, object parameters, int time)
        {
            return GetConnection().ExecuteScalar<T>(sql: sql, param: parameters, commandTimeout: time);
        }

        public static T TryGetValue<T>(string sql, object parameters)
        {
            try
            {
                return GetConnection().ExecuteScalar<T>(sql, parameters);
            }
            catch
            {
                return default(T);
            }
        }
        public static T TryGetValue<T>(string sql, object parameters, T defaultWhenFail)
        {
            try
            {
                return GetConnection().ExecuteScalar<T>(sql, parameters);
            }
            catch
            {
                return defaultWhenFail;
            }
        }

        public static T TryGetValue<T>(string sql, object parameters, int time, T defaultWhenFail)
        {
            try
            {
                return GetValue<T>(sql, parameters, time);
            }
            catch
            {
                return defaultWhenFail;
            }
        }



        public static IEnumerable<T> Query<T>(
            string sql,
            object parameter = null,
            CommandType cmdType = CommandType.Text,
            int? timeOut = null,
            SqlTransaction transaction = null)
        {
            var result = GetConnection().Query<T>(
                sql: sql,
                param: parameter,
                commandType: cmdType,
                commandTimeout: timeOut,
                transaction: transaction
                );
            return result;
        }
        public static IEnumerable<T> Query<T>(
            string[] fields,
            string from,
            string where,
            string orderby,
            int skip,
            int take)
        {
            var templateSql = String.Format(@"
declare @skip int, @take int
set @skip = {0}
set @take = {1}
select {2} from 
(select row_number() over (order by {5}) as _SortNumber, {2} from [{3}] where {4}) as InnerTable
where _SortNumber between (@skip+1) and @skip + @take 
order by {5}
", skip, take, String.Join(",", fields), from, where, orderby);

            return Query<T>(templateSql);
        }


        /// <summary>
        /// 对SQL执行结果进行分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="order"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="autoFix">是否自动添加 top 100 percent来修复SQL不允许子查询使用order by</param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(string sql, string order, int start, int end, bool autoFix = true)
        {
            if (autoFix)
            {
                var startPrefix = "^[ \n\r\t]*";

                if (Regex.IsMatch(sql, startPrefix + "select[ \n\r\t]* distinct", RegexOptions.IgnoreCase))
                {
                    sql = Regex.Replace(sql, startPrefix + "select[ \n\r\t]* distinct", " select distinct top 100 percent ", RegexOptions.IgnoreCase);
                }
                else if (Regex.IsMatch(sql, startPrefix + "select", RegexOptions.IgnoreCase))
                {
                    sql = Regex.Replace(sql, startPrefix + "select", " select top 100 percent ", RegexOptions.IgnoreCase);
                }
                else
                {
                    throw new Exception("SQL statement formate has error.");
                }
            }

            var templateSql = String.Format(@"
select * from (
	select row_number() over (order by {1}) as _SortNumber, * from (
{0}
	)  as SortTable
) as FullTable
where _SortNumber between {2} and {3}
", sql, order, start, end);
            return Query<T>(templateSql);
        }


        public static int Execute(Func<SqlConnection, SqlTransaction, int> exexuteSqlCallback)
        {
            var effectedRows = -1;
            var connection = GetConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                effectedRows = exexuteSqlCallback(connection, transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return effectedRows;
        }

        public static int Execute(string sql, object parameters)
        {
            return Execute((connection, transaction) =>
            {
                var rows = 0;
                rows += connection.Execute(sql, parameters, transaction);
                return rows;
            });
        }

        public static int Execute(string sql, int time, object parameters)
        {
            return Execute((connection, transaction) =>
            {
                var rows = 0;
                rows += connection.Execute(
                    sql: sql,
                    param: parameters,
                    transaction: transaction,
                    commandTimeout: time);
                return rows;
            });
        }
        public static int Execute(List<string> sql)
        {
            return Execute((connection, transaction) =>
            {
                var rows = 0;
                sql.ForEach(s =>
                {
                    rows += connection.Execute(s, transaction);
                });
                return rows;
            });
        }
        public static int Execute(string sql, Dapper.DynamicParameters parameter, CommandType cmdType)
        {
            return Execute((connection, transaction) =>
            {
                var rows = 0;
                rows = connection.Execute(
                    sql: sql,
                    param: parameter,
                    commandType: cmdType,
                    transaction: transaction);
                return rows;
            });
        }

        public static IDataReader Read(
            string sql,
            object parameters = null,
            CommandType cmdType = CommandType.Text,
            int? cmdTimeout = null)
        {
            return GetConnection().ExecuteReader(
                sql: sql,
                param: parameters,
                commandType: cmdType,
                commandTimeout: cmdTimeout);
        }


        public static bool IsColumnExist(string table, string column)
        {
            return TryGetValue<int>(
                "select count(*) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME=@Table and COLUMN_NAME=@Column",
                new { Table = table, Column = column },
                -1) == 1;
        }
        public static int GetMaxId(string table, string column)
        {
            return TryGetValue<int>("select max(@column) + 1 from [table]".Replace("[table]", table), new { column }, 1);
        }


        public static int GetRecordsCount(string table, string where = "1=1", object parameters = null, int whenFail = -1)
        {
            return TryGetValue<int>(String.Format("select count(*) from {0} where {1}", table, where), parameters, -1);
        }

        public static bool IsExist(string sql, object parameters)
        {
            var value = TryGetValue<object>(sql, parameters, null);

            return value != null && value != DBNull.Value;
        }

        public static int Insert<T>(T model)
        {
            var tableName = typeof(T).Name;
            var properties = model.ExGetPropNames();
            var fileds = String.Join(", ", properties.Select(prop => "[" + prop + "]"));
            var values = String.Join(", ", properties.Select(prop => "@" + prop));
            var sql = String.Format("insert into [{0}] ({1}) values({2})", tableName, fileds, values);
            return Execute(sql, model);
        }

        public static int Insert(string tableName, object model)
        {
            var properties = model.ExGetPropNames();
            var fileds = String.Join(", ", properties.Select(prop => "[" + prop + "]"));
            var values = String.Join(", ", properties.Select(prop => "@" + prop));
            var sql = String.Format("insert into [{0}] ({1}) values({2})", tableName, fileds, values);
            return Execute(sql, model);
        }


    }

}
