namespace CommonEF.Migrations
{
    using ContainerTypes;
    using Infrastructure.Container.Entities;
    using Infrastructure.Material.Entities;
    using Infrastructure.Users.Entites;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CommonEF.Cryogatt>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Cryogatt context)
        {
           using (var db = new Cryogatt())
           {
                //  Add container types
                //foreach (var type in ContainerIdentTypes.Types)
                //{
                //    // Add container types to db
                //    db.ContainerTypes.AddOrUpdate(
                //        new ContainerType((int)type.Ident, type.Description));

                //    db.SaveChanges();

                //    var subtypes = ContainerIdentTypes.Subtypes
                //        .Where(s => type.Ident == ((int)s.TagIdent >> 16));

                //    foreach (var s in subtypes)
                //    {
                //        // Add container idents to db
                //        db.ContainerIdents.AddOrUpdate(
                //            new ContainerIdent(0, s.Description, (int)s.TagIdent, type.Ident));
                //    }
                //    db.SaveChanges();
                //}

                //// Add location container
                //var location = new Container(
                //    0,
                //    "Cryogatt",
                //    "Cryogatt HQ",
                //    73,
                //    null,
                //    0);
                //// Stage
                //db.Containers.AddOrUpdate(location);

                //    //db.SaveChanges();

                //// Add site 
                //var site = new Site(0, "Kings Langley", "Cryogatt", location.Id);
                //// Stage
                //db.Sites.AddOrUpdate(site);

                //db.SaveChanges();

                //// Add Group
                //var group = new Group("Cryogatt Employees", "Cryogatt Staff Members", DateTime.Now);
                //// Stage
                //db.Groups.AddOrUpdate(group);

                //db.SaveChanges();

                // Add role
                //var role = new Role("Admin", "Admin", DateTime.Now);
                //// Stage
                //db.Roles.AddOrUpdate(role);

                ////db.SaveChanges();
                //var site = context.Set<Site>().Find(1);
                //var group = context.Set<Group>().Find(1);
                //// Add user
                //context.Add(new User(0,
                //    "System",
                //    "Administrator",
                //    "Admin",
                //    "info@cryogatt.com",
                //    "password",
                //    1,
                //    new List<Site> { site },
                //    new List<Group> { group },
                //    null,
                //    null,
                //    DateTime.Now));

                //    db.SaveChanges();

                // Dummy material data
                //db.AddRange(new List<AttributeField>
                //        {
                //            new AttributeField(0, "Sample Type", ""),
                //            new AttributeField(0, "Reference No", ""),
                //            new AttributeField(0, "External No", ""),
                //            new AttributeField(0, "Reception Date", ""),
                //            new AttributeField(0, "Collection Date", ""),
                //            new AttributeField(0, "Freeze Date", ""),
                //        });

              //  db.SaveChanges();

            }
        }
    }
}
