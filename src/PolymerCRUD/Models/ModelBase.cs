using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using Npgsql;
using Microsoft.AspNet.Mvc;

namespace PolymerCRUD.Models
{
    public abstract class ModelBase<T> : IDisposable where T : new()
    {
        [Editable(false)]
        public abstract int id { get; set; }

        [Editable(false)]
        public abstract string Description { get; }

        protected readonly NpgsqlConnection dbConnection;
        private readonly string table;

        protected string sqlSelect = "";
        protected string sqlSelectById = "";

        public ModelBase(string table)
        {
            //if (connString == "")
            //{
            //    lock(connString)
            //    {
            //        if (connString == "")
            //        {
            //            ConfigurationManager.ConnectionStrings["dapper"].ToString();
            //        }
            //    }
            //}

            this.table = table;

            dbConnection = new NpgsqlConnection(Startup.ConnectionString);
        }

        public IEnumerable<T> Get()
        {
            IEnumerable<T> list = null;

            if (sqlSelect == "")
            {
                sqlSelect = "Select * From " + table;
            }

            try
            {
                dbConnection.Open();

                list = dbConnection.Query<T>(sqlSelect);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return list;
        }

        public static T Get<H>(int id) where H : ModelBase<T>, new()
        {
            H obj = new H();

            return obj.GetInternal(id);
        }

        public T GetInternal(int id)
        {
            object obj = null;

            if (id == 0)
            {
                obj = new T();
            }
            else
            {
                if (sqlSelectById == "")
                {
                    sqlSelectById = "select * from " + table + " where Id" + table + " = @Id";
                }

                try
                {
                    dbConnection.Open();

                    obj = dbConnection.QueryFirstOrDefault<T>(sqlSelectById, new { Id = id });
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (dbConnection.State == ConnectionState.Open)
                    {
                        dbConnection.Close();
                    }
                }
            }

            return (T)obj;
        }

        public virtual void PreSave()
        {
        }

        public virtual void PostSave()
        {
        }

        public JsonResult Save()
        {
            ModelResponse response = new ModelResponse();

            try
            {
                string whatAction;

                dbConnection.Open();
                PreSave();
                if (id == 0)
                {
                    id = (int) dbConnection.Insert(this);
                    whatAction = "created";
                }
                else
                {
                    dbConnection.Update(this);
                    whatAction = "saved";
                }
                PostSave();

                response.ok = true;
                response.model = this;
                response.message = Description + " " + whatAction + " succesfully";
            }
            catch(Exception exp)
            {
                response.ok = false;
                response.message = "Error saving " + Description + ": " + exp.Message;
            }
            finally
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return new JsonResult(response);
        }

        public void Dispose()
        {
            if (dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
            }
        }
    }
}
