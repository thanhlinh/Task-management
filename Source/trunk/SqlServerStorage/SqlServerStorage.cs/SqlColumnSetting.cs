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
		public override List<ColumnSettingInfo> GetAllSettingOfBoard(Guid boardId)
		{
			List<ColumnSetting> columnSettings = dbContext.ColumnSettings.Where(s => s.BoardId.Equals(boardId) && s.Status == Status.Enable).ToList();
			List<ColumnSettingInfo> columnSettingInfos = new List<ColumnSettingInfo>();
			foreach(ColumnSettingInfo settingInfo in columnSettings)
			{
				columnSettingInfos.Add(settingInfo);
			}
			return columnSettingInfos;
		}

		public override ColumnSettingInfo AddSetting(Guid columnId, Guid next, Guid boardId)
		{
			ColumnSetting columnSetting = dbContext.ColumnSettings.Where(s => s.ColumnId.Equals(columnId) &&
				s.Next.Equals(next) && s.Status == Status.Enable).FirstOrDefault();

			if (columnSetting != null)
			{
				columnSetting.Status = Status.Enable;
				columnSetting.Updated = DateTime.UtcNow;

			}
			else
			{
				columnSetting = new ColumnSetting()
				{
					Id = Guid.NewGuid(),
					ColumnId = columnId,
					Next = next,
					BoardId = boardId,
					Created = DateTime.UtcNow,
					Updated = DateTime.UtcNow,
					Status = Status.Enable
				};
			}

			ColumnSettingInfo columnSettingInfo = (ColumnSettingInfo)columnSetting;

			try
			{
				dbContext.SaveChanges();
				return columnSettingInfo;
			}
			catch (DbEntityValidationException) { }

			return null;
		}

		public override bool DeleteSetting(Guid columnId, Guid next)
		{
			ColumnSetting columnSetting = dbContext.ColumnSettings.Where(s => s.ColumnId.Equals(columnId) &&
				s.Next.Equals(next) && s.Status == Status.Enable).FirstOrDefault();

			if (columnSetting != null)
			{
				columnSetting.Status = Status.Disabled;
				columnSetting.Updated = DateTime.UtcNow;

				try
				{
					dbContext.SaveChanges();
					return true;
				}
				catch (DbEntityValidationException) { }
			}

			return false;
		}

		public override ColumnSettingInfo SyncColumnSetting(ColumnSettingInfo settingInfo, bool isAdd)
		{
			if(isAdd)
			{
				ColumnSetting columnSetting = new ColumnSetting()
				{
					Id = Guid.NewGuid(),
					ColumnId = settingInfo.ColumnId,
					Next = settingInfo.Next,
					BoardId = settingInfo.BoardId,
					Created = DateTime.UtcNow,
					Updated = DateTime.UtcNow,
					Status = Status.Enable
				};
				try
				{
					dbContext.ColumnSettings.Add(columnSetting);
					dbContext.SaveChanges();

					return (ColumnSetting)columnSetting;
				}
				catch (Exception) {
					return null;
				}
			}
			else
			{
				ColumnSetting columnSetting = dbContext.ColumnSettings.FirstOrDefault(s => s.ColumnId.Equals(settingInfo.ColumnId) && s.Next.Equals(settingInfo.Next));
				if(columnSetting != null)
				{
					columnSetting.ColumnId = settingInfo.ColumnId;
					columnSetting.Next = settingInfo.Next;
					columnSetting.BoardId = settingInfo.BoardId;
					columnSetting.Updated = DateTime.UtcNow;
					columnSetting.Status = settingInfo.Status;

					try
					{
						dbContext.SaveChanges();

						return (ColumnSetting)columnSetting;
					}
					catch (Exception)
					{
					}
				}
				return null;
			}
		}

		public override bool CheckSettingInData(Guid columnId, Guid next)
		{
			ColumnSetting columnSetting = dbContext.ColumnSettings.FirstOrDefault(s => s.ColumnId.Equals(columnId) && s.Next.Equals(next));
			return columnSetting != null;
		}
	}
}
