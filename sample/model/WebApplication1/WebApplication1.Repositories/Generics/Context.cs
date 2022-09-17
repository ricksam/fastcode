using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Repositories.Models;

namespace WebApplication1.Repositories.Generics
{
    public class Context : IDisposable, IContext
    {
        public Context(DbConfig databaseConfiguration)
        {
            if (_conn == null)
            {
                _conn = new System.Data.SqlClient.SqlConnection(databaseConfiguration.ConnectionString);
            }
        }

        private IDbConnection _conn { get; set; }
        private IDbTransaction _transaction { get; set; }

        public IEnumerable<T> Query<T>(
            string sql, object param = null)
        {
            return _conn.Query<T>(sql, param, _transaction);
        }

        public int Execute(string sql, object param = null)
        {
            return _conn.Execute(sql, param, _transaction);
        }

        public IDataReader ExecuteReader(string sql, object param = null)
        {
            return _conn.ExecuteReader(sql, param, _transaction);
        }

        public string PrepareInsert(string sql_insert, string primarykey_field)
        {
            if (_conn is System.Data.SqlClient.SqlConnection)
            {
                return string.Format(sql_insert, "OUTPUT Inserted." + primarykey_field, "");
            }
            return sql_insert;
        }

        public void BeginTransaction()
        {
            _conn.Open();
            _transaction = _conn.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_conn != null)
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                    _conn.Dispose();
                }
                _conn = null;
            }
        }

        public static int LocalGMT()
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return info.BaseUtcOffset.Hours;//HttpContext.Current.Request.ClientCertificate.ValidFrom.Subtract(DateTime.UtcNow).Hours;
        }

        public static DateTime GetLocalDateTime(DateTime datetime)
        {
            try
            {
                if (datetime != DateTime.MinValue)
                {
                    return datetime.AddHours(LocalGMT());
                }

                return DateTime.MinValue;
            }
            catch
            {
                return datetime;
            }

        }

        public static DateTime? GetLocalDateTime(DateTime? datetime)
        {
            try
            {
                if (datetime != null)
                {
                    return (datetime ?? DateTime.MinValue).AddHours(LocalGMT());
                }

                return null;
            }
            catch
            {
                return datetime;
            }

        }

        public static DateTime GetLocalDateTime()
        {
            try
            {
                return GetLocalDateTime(DateTime.UtcNow);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static string Quoted(string s)
        {
            if (!string.IsNullOrEmpty(s))
            { return string.Format("'{0}'", s.Replace("'", "''")); }
            else
            { return "''"; }
        }
    }
}
