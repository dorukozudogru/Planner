using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;


namespace Planner.DataAccess
{
    public class BasicRepository<TEntity> : Repository<TEntity> where TEntity : new()
    {
        public static void Save(TEntity entity)
        {
            Database.Save(entity);
        }
    }

    public class Repository<TEntity> where TEntity : new()
    {
        public static DBContextDB Database { get { return DBContextDB.GetInstance(); } }

        public static object Insert(TEntity entity)
        {
            return Database.Insert(entity);
        }

        public static int Update(TEntity entity)
        {
            return Database.Update(entity);
        }

        public static int Delete(TEntity entity)
        {
            return Database.Delete(entity);
        }

        public static int Delete(int primaryKey)
        {
            var poco = SingleOrDefault(primaryKey);
            if (poco != null)
                return Delete(poco);

            return 0;
        }

        public static bool IsNew(TEntity entity) { return Database.IsNew(entity); }

        public static TEntity SingleOrDefault(object primaryKey) { return Database.SingleOrDefault<TEntity>(primaryKey); }
        public static TEntity SingleOrDefault(string sql, params object[] args) { return Database.SingleOrDefault<TEntity>(sql, args); }
        public static TEntity SingleOrDefault(Sql sql) { return Database.SingleOrDefault<TEntity>(sql); }
        public static TEntity FirstOrDefault(string sql, params object[] args) { return Database.FirstOrDefault<TEntity>(sql, args); }
        public static TEntity FirstOrDefault(Sql sql) { return Database.FirstOrDefault<TEntity>(sql); }

        public static List<TEntity> Fetch() { return Database.Fetch<TEntity>(""); }
        public static List<TEntity> Fetch(string sql, params object[] args) { return Database.Fetch<TEntity>(sql, args); }
        public static List<TEntity> Fetch(long page, long itemsPerPage, string sql, params object[] args) { return Database.Fetch<TEntity>(page, itemsPerPage, sql, args); }
        public static List<TEntity> SkipTake(long skip, long take, string sql, params object[] args) { return Database.SkipTake<TEntity>(skip, take, sql, args); }
        public static Page<TEntity> Page(long page, long itemsPerPage, string sql, params object[] args) { return Database.Page<TEntity>(page, itemsPerPage, sql, args); }
        public static IEnumerable<TEntity> Query(string sql, params object[] args) { return Database.Query<TEntity>(sql, args); }
        public static int Execute(string sql, params object[] args) { return Database.Execute(sql, args); }
    }
}
