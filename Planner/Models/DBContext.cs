namespace Planner.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<UserProject> UserProject { get; set; }
        public virtual DbSet<UserCV> UserCV { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<UserProject>()
        //        .HasOptional(e => e.UserProject1)
        //        .WithRequired(e => e.UserProject2);
        //}

        internal void SaveChanges(User user)
        {
            throw new NotImplementedException();
        }
    }
}
