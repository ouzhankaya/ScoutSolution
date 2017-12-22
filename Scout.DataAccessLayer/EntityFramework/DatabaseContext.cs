using Scout.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.DataAccessLayer.EntityFramework
{
   public class DatabaseContext : DbContext
    {
        public DbSet<Manager> Managers { get; set; }
        public DbSet<OtherPosition> OtherPositions { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Footballer> Footballers { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Liked> Likes { get; set; }
        public DbSet<Foot> Feet { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Admin> Admins { get; set; }


        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }


    }
}
