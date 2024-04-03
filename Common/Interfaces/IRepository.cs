using System.Collections.Generic;

namespace Common
{
    /// <summary>
    ///     Generic repository.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        ///     Retrieve entity from it primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity</returns>
        T Get<T>(int id) where T : class;

        /// <summary>
        ///     Get all items of TEntity.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>() where T : class;

        /// <summary>
        ///     Get all items of TEntity
        /// </summary>
        /// <returns></returns>
        int GetTotalNumberOfRecords<T>() where T : class;

        /// <summary>
        ///     Add generic etity to database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        T Add<T>(T value) where T : class;

        /// <summary>
        ///     Add range of entities. 
        /// </summary>
        /// <param name="entities"></param>
        void AddRange<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        ///     Delete Entity.
        /// </summary>
        /// <param name="entity"></param>
        void Remove<T>(T entity) where T : class;

        /// <summary>
        ///     Delete range of Entities.
        /// </summary>
        /// <param name="entities"></param>
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        ///     Update entity.
        /// </summary>
        /// <param name="entity"></param>
        void Update<T>(T entity) where T : class;

        /// <summary>
        ///     Update many.
        /// </summary>
        /// <param name="entities"></param>
        void UpdateEntities<T>(IEnumerable<T> entities) where T : class;
    }
}

