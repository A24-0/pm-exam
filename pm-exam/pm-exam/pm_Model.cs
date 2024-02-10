using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace pm_exam
{
    public partial class pm_Model : DbContext
    {
        public pm_Model()
            : base("name=pm_Model")
        {
        }

        public virtual DbSet<EncryptedData> EncryptedData { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EncryptedData>()
                .Property(e => e.ServiceName)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.EncryptedData)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.PasswordUserID)
                .WillCascadeOnDelete(false);
        }
    }
}
