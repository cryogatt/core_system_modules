using System;
using System.Collections.Generic;
using System.Data.Entity;
using History.Entities;
using Infrastructure.Container.Entities;
using Infrastructure.Distribution.Entities;
using Infrastructure.History.Entities;
using Infrastructure.Material.Entities;
using Infrastructure.PickList.Entities;
using Infrastructure.Users.Entites;

namespace CommonEF
{
    public interface ICryogatt : IDisposable
    {
        DbSet<Aliquot> Aliquots { get; set; }
        DbSet<AttributeField> AttributeFields { get; set; }
        DbSet<AttributeValue> AttributeValues { get; set; }
        DbSet<Batch> Batches { get; set; }
        DbSet<ContainerIdent> ContainerIdents { get; set; }
        DbSet<Container> Containers { get; set; }
        DbSet<ContainerType> ContainerTypes { get; set; }
        DbSet<Contents> Contents { get; set; }
        DbSet<Courier> Couriers { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<PickListItem> PickListItems { get; set; }
        DbSet<PickList> PickLists { get; set; }
        DbSet<Point> Points { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Shipment> Shipments { get; set; }
        DbSet<Site> Sites { get; set; }
        DbSet<Status> Statuses { get; set; }
        DbSet<ContainerStatus> ContainerStatuses { get; set; }
        DbSet<BatchType> BatchTypes { get; set; }
        DbSet<BatchInfo> BatchInfos { get; set; }
        DbSet<User> Users { get; set; }

        T GetByPrimaryKey<T>(int id) where T : class;
        IEnumerable<T> GetAll<T>() where T : class;
        void Add<T>(T value) where T : class;
        void SaveChanges();
        void AddRange<T>(IEnumerable<T> entities) where T : class;
        void Remove<T>(T entity) where T : class;
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;
        void Update<T>(T entity) where T : class;
        void Attach<T>(T entity) where T : class;
    }
}