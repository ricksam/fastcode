using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories.Generics
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : IEntity
    {
        public RepositoryBase(Context context)
        {
            this.Context = context;
        }

        public Context Context { get; set; }

        public virtual List<T> List() { return this.Context.Query<T>(@"select * from " + string.Format("[{0}]", typeof(T).Name)).ToList(); }



        public static string[] GetAttributes(object _o)
        {
            List<string> lst = new List<string>();
            string[] fields = _o.GetType().GetFields().Select(q => q.Name).ToArray<string>();
            string[] properties = _o.GetType().GetProperties().Select(q => q.Name).ToArray<string>();
            lst.AddRange(fields);
            lst.AddRange(properties);
            return lst.ToArray();
        }

        private static string getCondition(object condition)
        {
            string[] attrs = GetAttributes(condition);

            string result = "";
            foreach (var item in attrs)
            {
                result += (string.IsNullOrEmpty(result) ? "" : " and ") + string.Format("{0} = @{0}", item);
            }
            return "where " + result;
        }

        public virtual List<T> FilterBy(object param)
        {
            return this.Context.Query<T>(
                    @"select * 
                    from " + string.Format("[{0}]", typeof(T).Name) + " " + getCondition(param), param).ToList();
        }

        public virtual T FindBy(object param)
        {
            return this.Context.Query<T>(
                    @"select * 
                    from " + string.Format("[{0}]", typeof(T).Name) + " " + getCondition(param), param).FirstOrDefault();
        }

        public virtual T FindById(int Id)
        {
            return this.Context.Query<T>(@"select * from " + string.Format("[{0}]", typeof(T).Name) + " where id = " + Id).FirstOrDefault();
        }

        public virtual int DeleteBy(object param)
        {
            return this.Context.Execute(
                    @"delete from " + string.Format("[{0}]", typeof(T).Name) + " " + getCondition(param), param);
        }

        public virtual int DeleteById(int Id)
        {
            return this.Context.Execute(@"delete from " + string.Format("[{0}]", typeof(T).Name) + " where Id = " + Id);
        }

        /*public virtual int Insert(T entity)
        {
            return entity.Id;
        }*/
        public virtual int Insert(T entity)
        {
            string[] Columns = GetAttributes(entity).Where(q => q.ToLower() != "id").ToArray();

            string sql_insert = @"insert into [{table}] (
                        {columns}
                        ) {0} values (
                        {@columns}
                    ) {1}";

            sql_insert = sql_insert
                .Replace("{table}", typeof(T).Name)
                .Replace("{columns}", string.Join(",", Columns))
                .Replace("{@columns}", string.Join(",", Columns.Select(s => string.Format("@{0}", s))));

            return this.Context.Query<int>(this.Context.PrepareInsert(sql_insert, "Id"), entity).FirstOrDefault();
        }

        /*public virtual int Update(T entity)
        {
            return entity.Id;
        }*/
        public virtual int Update(T entity)
        {
            string[] Columns = GetAttributes(entity).Where(q => q.ToLower() != "id").ToArray();

            string sql_update = @"update [{table}] set 
                    {columns}
                where Id = @Id";

            sql_update = sql_update
                .Replace("{table}", typeof(T).Name)
                .Replace("{columns}", string.Join(",", Columns.Select(s => string.Format("{0}=@{0}", s))));

            return this.Context.Execute(
                sql_update, entity);
        }

        public virtual T Save(T entity)
        {
            if (entity.Id == 0)
                entity.Id = Insert(entity);
            else
                Update(entity);
            return entity;
        }
    }
}
