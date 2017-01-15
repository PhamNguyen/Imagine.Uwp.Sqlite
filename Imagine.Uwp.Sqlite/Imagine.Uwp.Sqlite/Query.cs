using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.Uwp.Sqlite
{
    public class Query<T> : BaseContext, IQuery<T> where T : class
    {
        public string Name { get; set; } = "";

        public Query(string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                Name = tableName;
            }
        }

        public async Task<int> CommitAsync(T input)
        {
            string id = !String.IsNullOrEmpty(Name) ? string.Format("{0}Id", Name) : "Id";

            Type type = typeof(T);

            var properties = type.GetProperties();

            var objectIdProperty = properties.FirstOrDefault(p => p.Name.Equals(id));

            var idProperty = properties.FirstOrDefault(p => p.Name.Equals("Id"));

            var value = objectIdProperty.GetValue(input);

            string query = string.Format("{0} = '{1}'", id, value);

            var existItem = await FirstOrDefault(query);

            idProperty.SetValue(input, idProperty.GetValue(existItem));

            //0 in default;
            int index; 

            if (existItem == null)
            {
                //TODO: enable or disable write to output
                //Debug.WriteLine("Commit call Insert");
                index = await AsyncConnection.InsertAsync(input);
            }
            else
            {
                //TODO: enable or disable write to output
                //Debug.WriteLine("Commit call Update");
                index = await AsyncConnection.UpdateAsync(input);
            }

            return index;
        }

        #region Select

        public virtual async Task<IEnumerable<T>> SelectAsync(string query = "")
        {
            StringBuilder queryBuilder = new StringBuilder("SELECT * FROM ");
            queryBuilder.Append(Name);

            if (!string.IsNullOrEmpty(query))
            {
                queryBuilder.Append(" WHERE ");
                queryBuilder.Append(query.Trim());
            }
#if DEBUG
            //TODO: enable or disable write to output
            //Debug.WriteLine(queryBuilder.ToString());
#endif
            IEnumerable<T> results = await AsyncConnection.QueryAsync<T>(queryBuilder.ToString());
            return results;
        }

        public virtual async Task<IEnumerable<T>> QueryAsync(string stringSql)
        {
            StringBuilder queryBuilder = new StringBuilder(stringSql);
            IEnumerable<T> results = await AsyncConnection.QueryAsync<T>(queryBuilder.ToString());
            return results;
        }

        public async Task<IEnumerable<T>> SelectAsync(int page, int size, string query = "")
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append(@"SELECT * FROM (SELECT * FROM ");
            queryBuilder.Append(Name);
            queryBuilder.Append(" ");

            if (!string.IsNullOrEmpty(query))
            {
                queryBuilder.Append(query);
            }

            queryBuilder.Append(" ORDER BY Tick DESC) DATAS LIMIT ");
            queryBuilder.Append(size);
            queryBuilder.Append(" OFFSET ");
            queryBuilder.Append(size*page);
#if DEBUG
            //TODO: enable or disable write to output
            //Debug.WriteLine(queryBuilder.ToString());
#endif
            var data = await AsyncConnection.QueryAsync<T>(queryBuilder.ToString());
            return data;
        }

        public async Task<IEnumerable<T>> SelectAsync(string format, params object[] parametters)
        {
            StringBuilder queryBuilder = new StringBuilder("SELECT * FROM ");
            queryBuilder.Append(Name);

            if (!string.IsNullOrEmpty(format) && parametters != null && parametters.Any())
            {
                queryBuilder.Append(" WHERE ");
                queryBuilder.Append(string.Format(format, parametters));
            }
#if DEBUG
            //TODO: enable or disable write to output
            //string query = queryBuilder.ToString();
            //Debug.WriteLine(query);
#endif
            IEnumerable<T> results = await AsyncConnection.QueryAsync<T>(queryBuilder.ToString());
            return results;
        }

        public async Task<T> FirstOrDefault(string query = "")
        {
            var data = await SelectAsync(query);

            var result = data.FirstOrDefault();

            return result;
        }

        #endregion

        #region Insert

        public virtual async Task<int> InsertAsync(T input)
        {
            if (input != null)
            {
                return await AsyncConnection.InsertAsync(input);
            }
            return 0;
        }

        public async Task<int> InsertAllAsync(IEnumerable<T> input)
        {
            Connection.BeginTransaction();
            int row = Connection.InsertAll(input);
            Connection.Commit();

            return row;
        }

        #endregion

        #region Update

        public async Task<int> UpdateAllAsync(IEnumerable<T> input)
        {
            return await AsyncConnection.UpdateAllAsync(input);
        }

        public virtual async Task<int> UpdateAsync(T input)
        {
            if (input != null)
            {
                return await AsyncConnection.UpdateAsync(input);
            }
            return 0;
        }

        #endregion

        #region Delete or Clear

        public async Task<int> DeleteAsync(T input)
        {
            if (input != null)
            {
                return await AsyncConnection.DeleteAsync(input);
            }
            return 0;
        }

        public async Task<int> Clear()
        {
            return await AsyncConnection.DeleteAllAsync<T>();
        }

        #endregion
    }
}