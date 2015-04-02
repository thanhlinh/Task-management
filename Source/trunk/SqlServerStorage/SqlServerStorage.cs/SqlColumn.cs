using Sioux.TaskManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement
{
	public partial class SqlServerStorage : DataStorage
	{
		public override ColumnInfo GetColumnInfo(Guid columnId)
		{
			Column column = dbContext.Columns.Where(c => c.Id.Equals(columnId) && c.Status == Status.Enable).FirstOrDefault();

			return (ColumnInfo)column;
		}

		public override List<ColumnInfo> GetAllColumnOfBoard(Guid boardId)
		{
			List<Column> columns = dbContext.Columns.Where(c => c.BoardId.Equals(boardId) && c.Status == Status.Enable).ToList();
			List<ColumnInfo> columnInfos = new List<ColumnInfo>();
			foreach(Column column in columns)
			{
				columnInfos.Add(column);
			}
			return columnInfos;
		}

		public override ColumnInfo AddColumn(string name, int ownerId, Guid boardId)
		{
			Column column = new Column()
			{
				Id = Guid.NewGuid(),
				Name = name,
				Owner = ownerId,
				BoardId = boardId,
				Priority = 0,
				CardCount = 0,
				Created = DateTime.UtcNow,
				Updated = DateTime.UtcNow,
				Status = Status.Enable
			};


			ColumnInfo columnInfo = (ColumnInfo)column;

			dbContext.Columns.Add(column);

			Board board = dbContext.Boards.Where(b => b.Id.Equals(boardId) && b.Status == Status.Enable).FirstOrDefault();

			board.Priority++;

			try
			{
				dbContext.SaveChanges();
				return columnInfo;
			}
			catch (DbEntityValidationException) { }

			return null;
		}

		public override ColumnInfo UpdateColumn(Guid columnId, string name)
		{
			Column column = dbContext.Columns.Where(c => c.Id.Equals(columnId) && c.Status == Status.Enable).FirstOrDefault();

			ColumnInfo columnInfo = null;
			if (column != null)
			{
				column.Name = name;
				column.Updated = DateTime.UtcNow;

				columnInfo = (ColumnInfo)column;
			}

			return null;
		}

		public override bool DeleteColumn(Guid columnId)
		{
			Column column = dbContext.Columns.Where(c => c.Id.Equals(columnId) && c.Status == Status.Enable).First();

			if (column != null)
			{
				column.Status = Status.Deleted;
				column.Updated = DateTime.UtcNow;

				List<Card> cards = dbContext.Cards.Where(c => c.ColumnId.Equals(columnId) && c.Status == Status.Enable).ToList();

				foreach (Card card in cards)
				{
					card.Status = Status.Deleted;
					card.Updated = DateTime.Now;
				}

				try
				{
					dbContext.SaveChanges();
					return true;
				}
				catch (DbEntityValidationException)
				{
				}
			}

			return false;
		}

		public override ColumnInfo SyncColumn(ColumnInfo columnInfo, int owner, bool isAdd)
		{
			if(isAdd)
			{
				Column column = new Column()
				{
					Id = columnInfo.Id,
					Name = columnInfo.Name,
					Owner = owner,
					BoardId = columnInfo.BoardId,
					Priority = columnInfo.Priority,
					CardCount = columnInfo.CardCount,
					Created = columnInfo.Created,
					Updated = columnInfo.Updated,
					Status = Status.Enable
				};

				try
				{
					dbContext.Columns.Add(column);
					dbContext.SaveChanges();

					return (ColumnInfo)column;
				}
				catch (Exception) {
					return null;
				}
			}
			else
			{
				Column column = dbContext.Columns.Where(c => c.Id.Equals(columnInfo.Id)).FirstOrDefault();
				if(column != null)
				{
					column.Id = columnInfo.Id;
					column.Name = columnInfo.Name;
					column.Owner = columnInfo.Owner;
					column.BoardId = columnInfo.BoardId;
					column.Priority = columnInfo.Priority;
					column.CardCount = columnInfo.CardCount;
					column.Created = columnInfo.Created;
					column.Updated = columnInfo.Updated;
					column.Status = columnInfo.Status;

					try
					{
						dbContext.SaveChanges();
						return (ColumnInfo)column;
					}
					catch (Exception) { }
				}

				return null;
			}
		}

		public override bool CheckColumnInData(Guid columnId)
		{
			Column column = dbContext.Columns.FirstOrDefault(c => c.Id.Equals(columnId) && c.Status == Status.Enable);

			return column != null;
		}
	}
}
