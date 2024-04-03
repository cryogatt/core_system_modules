using Infrastructure.Users.Entites;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.History.Entities
{
    /// <summary>
    ///     Checkpoint.
    /// </summary>
    public class Point
    {
        public Point()
        {

        }

        public Point(int id, int containerId, string uid, string description, int containerIdentId,
            int? parentContainerStorageId, int storageIndex, int? parentContainerLocationId, int locationIndex,
            int flags, DateTime inceptDate, int userId, DateTime mark, string reason, string location, User user = null)
        {
            Id = id;
            ContainerId = containerId;
            Uid = uid;
            Description = description;
            ContainerIdentId = containerIdentId;
            ParentContainerStorageId = parentContainerStorageId;
            StorageIndex = storageIndex;
            ParentContainerLocationId = parentContainerLocationId;
            LocationIndex = locationIndex;
            Flags = flags;
            InceptDate = inceptDate;
            UserId = userId;
            Mark = mark;
            Reason = reason;
            Location = location;
            User = user;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        [JsonProperty]
        public int Id { get; private set; }

        /// <summary>
        ///     Container being audited.
        /// </summary>
        [JsonProperty]
        public int ContainerId { get; private set; }

        /// <summary>
        ///     Uid of container being auidted.
        /// </summary>
        [Required]
        [JsonProperty]
        public string Uid { get; private set; }

        /// <summary>
        ///     Label of container.
        /// </summary>
        [Required(ErrorMessage = "Container Description Required")]
        [JsonProperty]
        public string Description { get; private set; }

        /// <summary>
        ///     Ident of container being audited.
        /// </summary>
        [Required(ErrorMessage = "Container Ident Required!")]
        [JsonProperty]
        public int ContainerIdentId { get; private set; }

        /// <summary>
        ///     Container in whcih the container being stored is stored within.
        /// </summary>
        [JsonProperty]
        public int? ParentContainerStorageId { get; private set; }

        /// <summary>
        ///     Position in storage item.
        /// </summary>
        [JsonProperty]
        public int StorageIndex { get; private set; }

        /// <summary>
        ///     N/A?
        /// </summary>
        [JsonProperty]
        public int? ParentContainerLocationId { get; private set; }

        /// <summary>
        ///     N/A?
        /// </summary>
        [JsonProperty]
        public int LocationIndex { get; private set; }

        /// <summary>
        ///     N/A?
        /// </summary>
        [JsonProperty]
        public int Flags { get; private set; }

        /// <summary>
        ///     Date stored.
        /// </summary>
        [JsonProperty]
        public DateTime InceptDate { get; private set; }

        /// <summary>
        ///     Who?
        /// </summary>
        [ForeignKey("User")]
        [JsonProperty]
        public int UserId { get; private set; }

        /// <summary>
        ///     When?
        /// </summary>
        [JsonProperty]
        public DateTime Mark { get; private set; } = DateTime.Now;

        /// <summary>
        ///     What? Why?
        /// </summary>
        [JsonProperty]
        public string Reason { get; private set; }

        /// <summary>
        ///     Where?
        /// </summary>
        [JsonProperty]
        public string Location { get; private set; }

        public virtual User User { get; private set; }

        /// <summary>
        ///     Set the incept date.
        /// </summary>
        public void IsBeingStoredForFirstTime()
        {
            this.InceptDate = DateTime.Now;
        }
    }
}
