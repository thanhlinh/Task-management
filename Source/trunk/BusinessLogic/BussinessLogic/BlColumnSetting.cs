using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.BussinessLogic
{
	public class BlColumnSetting :BussinessLogicBase
	{
		public BlColumnSetting(DataStorage storage, int loggedUser)
			:base(storage, loggedUser)
		{
		}

		public ColumnSettingInfo Add(NameValueCollection post, out string message)
		{
			message = "";
			if(post["columnId"] != null && post["next"] != null && post["boardId"] != null && this.LoggedUser != 0)
			{
				Guid columnId = Guid.Empty;
				Guid next = Guid.Empty;
				Guid boardId = Guid.Empty;
				if(Guid.TryParse(post["columnId"], out columnId) && Guid.TryParse(post["next"],out next)
					&& Guid.TryParse(post["boardId"], out boardId))
				{
					ColumnSettingInfo columnSettingInfo = this.DataStorage.AddSetting(columnId, next, boardId);
					return columnSettingInfo;
				}
			}

			message = __("Parameters is invalid");
			return null;
		}


		public ColumnSettingInfo SaveData(ColumnSettingInfo settingInfo, out string message)
		{
			message = "";
			if(this.LoggedUser != 0 && settingInfo != null)
			{
				if(!this.DataStorage.CheckSettingInData(settingInfo.ColumnId, settingInfo.Next))
				{
					ColumnSettingInfo columnSettingInfo = this.DataStorage.SyncColumnSetting(settingInfo, true);
					return columnSettingInfo;
				}
				else
				{
					ColumnSettingInfo columnSettingInfo = this.DataStorage.SyncColumnSetting(settingInfo, false);
					return columnSettingInfo;
				}
			}
			return null;
		}

		public List<ColumnSettingInfo> GetAll(NameValueCollection post, out string message)
		{
			message = "";

			if (post["boardId"] != null && this.LoggedUser != 0)
			{
				Guid boardId = Guid.Empty;
				if(Guid.TryParse(post["boardId"], out boardId))
				{
					List<ColumnSettingInfo> columnSettingInfos = this.DataStorage.GetAllSettingOfBoard(boardId);
					return columnSettingInfos;
				}
			}

			message = __("Parameters is invalid");
			return null;
		}

		public bool Delete(NameValueCollection post, out string message)
		{
			message = "";

			if (post["columnId"] != null && post["next"] != null && this.LoggedUser != 0)
			{
				Guid columnId = Guid.Empty;
				Guid next = Guid.Empty;
				if (Guid.TryParse(post["columnId"], out columnId) && Guid.TryParse(post["next"], out next))
				{
					bool isDeleted = this.DataStorage.DeleteSetting(columnId, next);
					return isDeleted;
				}
			}

			message = __("Parameters is invalid");
			return false;
		}

		public bool HasPermission(string permission, ColumnSettingInfo settingInfo)
		{
			if (permission == "edit")
			{
				// if the logger is owner
				if (this.LoggedUser== 0)
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
