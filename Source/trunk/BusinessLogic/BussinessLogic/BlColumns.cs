using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.BussinessLogic
{
	public class BlColumns : BussinessLogicBase
	{
		public BlColumns(DataStorage storage, int loggedUser)
			: base(storage, loggedUser)
		{
		}

		public ColumnInfo Add(NameValueCollection post, out string message)
		{
			message = "";
			
			if (post["name"] != null && post["name"].Trim() != "" && post["boardId"] != null)
			{
				string name = post["name"].Trim();
				Guid boardId = Guid.Empty;

				if (Guid.TryParse(post["boardId"], out boardId))
				{
					ColumnInfo columnInfo = this.DataStorage.AddColumn(name, this.LoggedUser, boardId);

					return columnInfo;
				}
			}

			message = __("Parametters is invalid.");

			return null;
		}

		public ColumnInfo SaveData(ColumnInfo columnInfo, out string message)
		{
			message = "";
			if (this.LoggedUser > 0 && columnInfo != null)
			{
				if (!this.DataStorage.CheckColumnInData(columnInfo.Id))
				{
					columnInfo = DataStorage.SyncColumn(columnInfo, this.LoggedUser, true);
					return columnInfo;
				}
				else
				{
					columnInfo = DataStorage.SyncColumn(columnInfo, this.LoggedUser, false);
					return columnInfo;
				}
			}
			message = __("Parameters is invalid.");
			return null;
		}
		public List<ColumnInfo> GetAll(NameValueCollection post, out string message)
		{
			message = "";
			if(post["boardId"] != null && this.LoggedUser != 0)
			{
				Guid boardId = Guid.Empty;
				if(Guid.TryParse(post["boardId"], out boardId))
				{
					List<ColumnInfo> columnInfos = this.DataStorage.GetAllColumnOfBoard(boardId);
					return columnInfos;
				}

			}
			message = __("Parametters is invalid.");
			return null;
		}

		public ColumnInfo GetOne(NameValueCollection post, out string message)
		{
			message = "";

			if (post["columnId"] != null && this.LoggedUser != 0)
			{
				Guid columnId = Guid.Empty;
				if (Guid.TryParse(post["columnId"], out columnId))
				{
					ColumnInfo columnInfo = this.DataStorage.GetColumnInfo(columnId);
					return columnInfo;
				}
			}

			message = __("Parameters is invalid.");
			return null;
		}

		public ColumnInfo Update(NameValueCollection post, out string message)
		{
			message = "";


			if (post["name"] != null && post["name"].Trim() != "" && post["columnId"] != null && this.LoggedUser != 0)
			{
				string name = post["name"].Trim();
				Guid columnId = Guid.Empty;
				if (Guid.TryParse(post["columnId"], out columnId))
				{
					ColumnInfo columnInfo = this.DataStorage.UpdateColumn(columnId, name);
				}
			}

			message = __("Parameters is invalid");
			return null;
		}

		public bool Delete(NameValueCollection post, out string message)
		{
			message = "";
			if (post["columnId"] != null && this.LoggedUser != 0)
			{
				Guid columnId = Guid.Empty;
				if (Guid.TryParse(post["columnId"], out columnId))
				{
					bool isDeleted = this.DataStorage.DeleteColumn(columnId);
					return isDeleted;
				}
			}

			message = __("Parameters is invalid");
			return false;
		}

		public List<ColumnInfo> SyncToDataStorage(List<ColumnInfo> data)
		{
			List<ColumnInfo> result = new List<ColumnInfo>();
			if (data == null || data.Count == 0)
				return result;

			foreach (ColumnInfo item in data)
			{
				if (!this.DataStorage.CheckColumnInData(item.Id))
					result.Add(this.DataStorage.SyncColumn(item, this.LoggedUser, true));
				else
				{
					if (HasPermission("edit", item))
					{
						result.Add(DataStorage.SyncColumn(item, this.LoggedUser, false));
					}
				}
			}
			return result;
		}

		public bool HasPermission(string permission, ColumnInfo column)
		{
			if (permission == "edit")
			{
				// if the logger is owner
				if (this.LoggedUser == column.Owner)
					return true;
				// if the logger is manager of owner
			}
			else if (permission == "delete")
			{
				// 
			}
			return false;
		}
	}
}
