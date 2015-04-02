using Sioux.TaskManagement.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.Models
{
    class TaskManagementDBEntities : DbContext
    {
        public TaskManagementDBEntities()
            : base("DefaultConnection")
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<TaskManagementDBEntities, Configuration>());
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Board> Boards { get; set; }
		public DbSet<Column> Columns { get; set; }
        public DbSet<Card> Cards { get; set; }
		public DbSet<ColumnSetting> ColumnSettings { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

			//modelBuilder.Entity<ColumnSetting>()
			//	.HasRequired(c => c.CurrentColumn)
			//	.WithMany()
			//	.HasForeignKey(c => c.ColumnId);

			//modelBuilder.Entity<ColumnSetting>()
			//	.HasRequired(c => c.NextColumn)
			//	.WithMany()
			//	.HasForeignKey(c => c.Next).WillCascadeOnDelete(false);
        }
    }
}
