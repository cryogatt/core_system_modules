using Infrastructure.Container.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Container.Entities
{
    public class Container
    {
        #region Constructors

        /// <summary>
        ///     Empty Constructor required by entity framework. 
        /// </summary>
        public Container()
        { }
            
        public Container(int id, string uid, string description, int containerIdentId, 
            int? parentContainerStorageId, int storageIndex, 
            int? parentContainerLocationId = null, int locationIndex = 0, int flags = 0,
            DateTime? inceptDate = null, IList<Container> storageChildren = null)
        {
            Id = id;
            Uid = uid;
            Description = description;
            ContainerIdentId = containerIdentId;
            ParentContainerStorageId = parentContainerStorageId;
            StorageIndex = storageIndex;
            ParentContainerLocationId = parentContainerLocationId;
            LocationIndex = locationIndex;
            Flags = flags;
            InceptDate = inceptDate;
            StorageChildren = storageChildren;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///     Tag uid
        /// </summary>
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Uid { get; set; }

        /// <summary>
        ///     Label information.
        /// </summary>
        [Required(ErrorMessage = "Container Description Required")]
        public string Description { get; set; }

        /// <summary>
        ///     Tag Ident on block 0.
        /// </summary>
        [Required(ErrorMessage = "Container Ident Required!")]
        public int ContainerIdentId { get; set; }

        /// <summary>
        ///     Location of the container.
        /// </summary>
        public int? ParentContainerStorageId { get; set; } = null;

        /// <summary>
        ///     Position within parent.
        /// </summary>
        public int StorageIndex { get; set; } = 0;

        /// <summary>
        ///     Parent container.
        /// </summary>
        //[ForeignKey("Container")]
        public int? ParentContainerLocationId { get; set; } = null;

        /// <summary>
        ///     location position.
        /// </summary>
        public int LocationIndex { get; set; } = 0;

        /// <summary>
        ///     N/A
        /// </summary>
        public int Flags { get; set; } = 0;

        /// <summary>
        ///     Storage date.
        /// </summary>
        public DateTime? InceptDate { get; set; }
        #endregion

        #region Navigation Properties
        [ForeignKey("ContainerIdentId")]
        public virtual ContainerIdent ContainerIdent { get; set; }
        [ForeignKey("ParentContainerStorageId")]
        public virtual ICollection<Container> StorageChildren { get; set; }
        //    public virtual ICollection<Container> LocationChildren { get; set; }
        #endregion

        public Container Clone()
        {
            return new Container
            {
                Id = this.Id,
                Uid = this.Uid,
                Description = this.Description,
                ContainerIdentId = this.ContainerIdentId,
                ParentContainerStorageId = this.ParentContainerStorageId,
                StorageIndex = this.StorageIndex,
                ParentContainerLocationId = this.ParentContainerLocationId,
                LocationIndex = this.LocationIndex,
                Flags = this.Flags,
                InceptDate = this.InceptDate
            };
        }

        public override bool Equals(object cont)
        {
            if (cont == null)
            {
                return false;
            }

            return (cont is Container container)
                && (Id == container.Id)
                && (Uid == container.Uid)
                && (Description == container.Description)
                && (ContainerIdentId == container.ContainerIdentId)
                && (ParentContainerStorageId == container.ParentContainerStorageId)
                && (StorageIndex == container.StorageIndex)
                && (ParentContainerLocationId == container.ParentContainerLocationId)
                && (LocationIndex == container.LocationIndex)
                && (Flags == container.Flags)
                && (InceptDate == container.InceptDate);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        ///     Update the container record with only ceratin properties.
        /// </summary>
        /// <param name="container"></param>
        public void Update(ContainerResponseBody container)
        {
            Description = container.Description;
            InceptDate = container.InceptDate;
        }

        /// <summary>
        ///     Restore the container.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="storageIndex"></param>
        public void SetStorage(int parentId, int storageIndex)
        {
            ParentContainerStorageId = parentId;
            StorageIndex = storageIndex;
        }

        public void SetAsDisposed()
        {
            this.Flags = 1;
        }
    }
}
