using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using CommonEF.Services;
using Cryogatt.RFID.Trace;

namespace CommonEF
{
    /// <summary>
    ///     Generic repository for entities.
    /// </summary>
    public class Repository : IRepository
    {
        #region Constructors

        public Repository(IContextFactory contextFactory) : base()
        {
            ContextFactory = contextFactory;
        }

        #endregion

        protected IContextFactory ContextFactory;

        #region Public Methods

        /// <summary>
        ///     Retrieve entity from it primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<T>(int id) where T : class
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.GetByPrimaryKey<T>(id);                    
                }                    
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                // Rethrow
                throw;
            }
        }

        /// <summary>
        ///     Get all items of TEntity.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>() where T : class
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.GetAll<T>().ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                // Rethrow
                throw;
            }
        }

        /// <summary>
        ///     Get the number of records for a given entity.
        /// </summary>
        /// <returns></returns>
        public int GetTotalNumberOfRecords<T>() where T: class
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.GetAll<T>().Count();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                // Rethrow
                throw;
            }
        }
        
        /// <summary>
        ///     Add generic etity to database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public T Add<T>(T value) where T : class
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    context.Add(value);

                    context.SaveChanges();

                    return value;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Add range of entities. 
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    context.AddRange(entities);

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                // Rethrow
                throw;
            }
        }

        /// <summary>
        ///     Delete Entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Remove<T>(T entity) where T : class
        {
            try
            {
                using (var context = ContextFactory.Create())
                {                    
                    context.Remove(entity);

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                // Rethrow
                throw;
            }
        }

        /// <summary>
        ///     Delete range of Entities.
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    context.RemoveRange(entities);

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                // Rethrow
                throw;
            }
        }

        /// <summary>
        ///     Update entity (stage only).
        /// </summary>
        /// <param name="entity"></param>
        public void Update<T>(T entity) where T :class
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    context.Attach(entity);

                    context.Update(entity);

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                // Rethrow
                throw;
            }
        }

        /// <summary>
        ///     Update many.
        /// </summary>
        /// <param name="entities"></param>
        public void UpdateEntities<T>(IEnumerable<T> entities) where T : class
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    context.Attach(entities.First());

                    entities
                        .ToList()
                        .ForEach(entity => context.Update(entity));

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                // Rethrow
                throw;
            }
        }
        #endregion
    }
}

