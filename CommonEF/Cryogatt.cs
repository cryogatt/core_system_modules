using Distribution.Entities;
using History.Entities;
using Infrastructure.Container.Entities;
using Infrastructure.Distribution.Entities;
using Infrastructure.History.Entities;
using Infrastructure.Material.Entities;
using Infrastructure.PickList.Entities;
using Infrastructure.Users.Entites;
using System.Collections.Generic;
using System.Data.Entity;

namespace CommonEF
{
    public class Cryogatt : DbContext, ICryogatt
    {
        #region  Constructors

        public Cryogatt() :
            base("Cryogatt")
        { }

        #endregion

        #region Public DataSets

        #region Users

        /// <summary>
        ///     Set of registered users of the system.
        /// </summary>
        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        ///     Roles in which users can have.
        /// </summary>
        public virtual DbSet<Role> Roles { get; set; }

        /// <summary>
        ///     Groups in which users can belong to.
        /// </summary>
        public virtual DbSet<Group> Groups { get; set; }

        /// <summary>
        ///     Sites in which users can belong to.
        /// </summary>
        public virtual DbSet<Site> Sites { get; set; }

        #endregion

        #region Container

        /// <summary>
        ///     All containers.
        /// </summary>
        public virtual DbSet<Container> Containers { get; set; }

        /// <summary>
        ///     Manufactures of containers e.g wheaton 2 ml vial etc.
        /// </summary>
        public virtual DbSet<ContainerIdent> ContainerIdents { get; set; }

        /// <summary>
        ///     Types of containers e.g vials, dewars etc.
        /// </summary>
        public virtual DbSet<ContainerType> ContainerTypes { get; set; }

        /// <summary>
        ///     Lists of items for withdrawal.
        /// </summary>
        public virtual DbSet<PickList> PickLists { get; set; }

        /// <summary>
        ///     Items on list to be withdrawn.
        /// </summary>
        public virtual DbSet<PickListItem> PickListItems { get; set; }

        #endregion

        #region Material

        /// <summary>
        ///     Batches of material.
        /// </summary>
        public virtual DbSet<Batch> Batches { get; set; }

        /// <summary>
        ///     Headers/Fields associated to the material.
        /// </summary>
        public virtual DbSet<AttributeField> AttributeFields { get; set; }

        /// <summary>
        ///     Values of the fields.
        /// </summary>
        public virtual DbSet<AttributeValue> AttributeValues { get; set; }

        /// <summary>
        ///     The samples holding the material.
        /// </summary>
        public virtual DbSet<Aliquot> Aliquots { get; set; }

        /// <summary>
        ///     A list of types to group batches by.
        /// </summary>
        public virtual DbSet<BatchType> BatchTypes { get; set; }

        public virtual DbSet<BatchInfo> BatchInfos { get; set; }

        #endregion

        #region History

        /// <summary>
        ///     Checkpoints.
        /// </summary>
        public virtual DbSet<Point> Points { get; set; }

        /// <summary>
        ///     Status of containers.
        /// </summary>
        public virtual DbSet<ContainerStatus> ContainerStatuses { get; set; }

        #endregion

        #region Distribution

        /// <summary>
        ///     Orders.
        /// </summary>
        public virtual DbSet<Shipment> Shipments { get; set; }

        /// <summary>
        ///     Status of orders.
        /// </summary>
        public virtual DbSet<Status> Statuses { get; set; }

        /// <summary>
        ///     The contents of a shipment.
        /// </summary>
        public virtual DbSet<Contents> Contents { get; set; }

        /// <summary>
        ///     Shipment couriers.
        /// </summary>
        public virtual DbSet<Courier> Couriers { get; set; }

        /// <summary>
        ///     Record of senders.
        /// </summary>
        public virtual DbSet<Sender> Senders { get; set; }

        /// <summary>
        ///     Record of recipients.
        /// </summary>
        public virtual DbSet<Recipient> Recipients { get; set; }
        #endregion

        #endregion
        
        #region Generic Methods

        public T GetByPrimaryKey<T>(int id) where T : class
        {
            return Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return Set<T>();
        }

        public void Add<T>(T value) where T : class
        {
            Set<T>().Add(value);
        }

        void ICryogatt.SaveChanges()
        {
            SaveChanges();
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            Set<T>().AddRange(entities);
        }

        public void Remove<T>(T entity) where T : class
        {
            Set<T>().Remove(entity);
        }

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
        {
            Set<T>().RemoveRange(entities);
        }

        public void Update<T>(T entity) where T : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void Attach<T>(T entity) where T : class
        {
            Set<T>().Attach(entity);
        }

        #endregion
    }
}