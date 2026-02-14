using TNK.Core;
using TNK.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace TNK.Data
{
    public class TNKObjectContext : DbContext, IDbContext
    {
        #region Ctor

        public TNKObjectContext() : base(GetConnectionString())
        {
        }
        public TNKObjectContext(string connectionString) : base(connectionString)
        {
        }
        public static string GetConnectionString()
        {
            var storeId = GetStoreIdFromSession();
            var storeService = new StoreService();
            return storeService.GetConnectionStringForStore(storeId);
        }

        public static string GetStoreIdFromSession()
        {
            // Lấy cookie từ HttpContext
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                // Giải mã cookie để lấy ticket
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null)
                {
                    var userData = ticket.UserData;
                    // Phân tích UserData để lấy storeId
                    var parts = userData.Split('|');
                    if (parts.Length == 2)
                    {
                        return parts[1]; // storeId là phần thứ hai
                    }
                }
            }

            // Nếu không tìm thấy trong cookie, kiểm tra trong session
            return HttpContext.Current.Session["StoreId"]?.ToString();
        }

        #endregion

        #region Utilities

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //dynamically load all configuration
            //System.Type configType = typeof(LanguageMap);   //any of your configuration classes here
            //var typesToRegister = Assembly.GetAssembly(configType).GetTypes()
            //...or do it manually below. For example,
            //modelBuilder.Configurations.Add(new LanguageMap());
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(TNKEntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }


            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : Core.BaseEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }

            //entity is already loaded
            return alreadyAttached;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create database script
        /// </summary>
        /// <returns>SQL to generate database</returns>
        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : Core.BaseEntity
        {
            return base.Set<TEntity>();
        }

        public DataTable ExecuteStoredProcedureDataTableSqlHelper(string connectionString, string commandText, params object[] parameters)
        {
            SqlParameter[] param = new SqlParameter[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                param[i] = new SqlParameter();
                param[i].ParameterName = ((SqlParameter)parameters[i]).ParameterName;
                param[i].Value = ((SqlParameter)parameters[i]).Value;
            }
            //SqlConnection con = new SqlConnection();
            SqlCommand com = new SqlCommand();
            com.Connection = new SqlConnection(connectionString);
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as SqlParameter;
                    com.Parameters.Add(p);
                }
            }
            com.CommandText = commandText;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 900;
            DataSet ds = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter(com);
            adap.Fill(ds);
            var dt = ds.Tables[0];
            return dt;
        }
        public DataTableCollection ExecuteStoredProcedureDataTableMulti(string connectionString, string commandText, params object[] parameters)
        {
            SqlParameter[] param = new SqlParameter[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                param[i] = new SqlParameter();
                param[i].ParameterName = ((SqlParameter)parameters[i]).ParameterName;
                param[i].Value = ((SqlParameter)parameters[i]).Value;
            }
            //SqlConnection con = new SqlConnection();
            SqlCommand com = new SqlCommand();
            com.Connection = new SqlConnection(connectionString);
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as SqlParameter;
                    com.Parameters.Add(p);
                }
            }
            com.CommandText = commandText;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 900;
            DataSet ds = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter(com);
            adap.Fill(ds);
            var dt = ds.Tables;
            return dt;
        }
        public DataTable ExecuteStoredProcedureDataTableSqlHelper(string commandText, params object[] parameters)
        {
            SqlParameter[] param = new SqlParameter[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                param[i] = new SqlParameter();
                param[i].ParameterName = ((SqlParameter)parameters[i]).ParameterName;
                param[i].Value = ((SqlParameter)parameters[i]).Value;
            }
            //return AF_ORM.CORE.SqlHelper.ExecuteDataTable(((System.Data.SqlClient.SqlConnection)this.Database.Connection).ConnectionString
            //    , CommandType.StoredProcedure
            //    , commandText
            //    , param
            //    );
            SqlCommand com = new SqlCommand();
            com.Connection = (System.Data.SqlClient.SqlConnection)this.Database.Connection;
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as SqlParameter;
                    com.Parameters.Add(p);
                }
            }
            com.CommandText = commandText;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 900;
            DataSet ds = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter(com);
            adap.Fill(ds);
            var dt = ds.Tables[0];
            return dt;
        }
        public DataTable ExecuteStoredProcedureDataTable(string commandText, params object[] parameters)
        {
            SqlCommand com = new SqlCommand();
            com.Connection = (System.Data.SqlClient.SqlConnection)this.Database.Connection;
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as SqlParameter;
                    com.Parameters.Add(p);
                }
            }
            com.CommandText = commandText;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 900;
            DataSet ds = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter(com);
            adap.Fill(ds);
            var dt = ds.Tables[0];
            return dt;
        }
        public DataSet ExecuteStoredProcedureDataSet(string commandText, params object[] parameters)
        {
            SqlCommand com = new SqlCommand();
            com.Connection = (System.Data.SqlClient.SqlConnection)this.Database.Connection;
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as SqlParameter;
                    com.Parameters.Add(p);
                }
            }
            com.CommandText = commandText;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 900;
            DataSet ds = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter(com);
            adap.Fill(ds);           
            return ds;
        }
        public string ExecuteStoredProcedureToString(string commandText, SqlParameter outputparameter, params object[] parameters)
        {
            
            SqlCommand com = new SqlCommand();
            com.Connection = (System.Data.SqlClient.SqlConnection)this.Database.Connection;
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as SqlParameter;
                    com.Parameters.Add(p);
                }
            }
            com.Parameters.Add(outputparameter);
            com.CommandText = commandText;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 900;
            DataSet ds = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter(com);
            adap.Fill(ds);
            string outputValue = Convert.ToString(outputparameter.Value);
            return outputValue;
        }

        public void ExecuteStoredProcedure(string commandText, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }
            this.Database.CommandTimeout = 1200;
            this.Database.ExecuteSqlCommand(commandText, parameters);

        }


        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }

            return  this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();

            //bool acd = this.Configuration.AutoDetectChangesEnabled;
            //try
            //{
            //    this.Configuration.AutoDetectChangesEnabled = false;

            //    for (int i = 0; i < result.Count; i++)
            //        result[i] = AttachEntityToContext(result[i]);
            //}
            //finally
            //{
            //    this.Configuration.AutoDetectChangesEnabled = acd;
            //}

        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }

        /// <summary>
        /// Detach an entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        public IList<T> ExecuteStoredProcedureObjs<T>(string commandText, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";
                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }
            return this.Database.SqlQuery<T>(commandText, parameters).ToList();

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether proxy creation setting is enabled (used in EF)
        /// </summary>
        public virtual bool ProxyCreationEnabled
        {
            get
            {
                return this.Configuration.ProxyCreationEnabled;
            }
            set
            {
                this.Configuration.ProxyCreationEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether auto detect changes setting is enabled (used in EF)
        /// </summary>
        public virtual bool AutoDetectChangesEnabled
        {
            get
            {
                return this.Configuration.AutoDetectChangesEnabled;
            }
            set
            {
                this.Configuration.AutoDetectChangesEnabled = value;
            }
        }

        #endregion
    }
}
