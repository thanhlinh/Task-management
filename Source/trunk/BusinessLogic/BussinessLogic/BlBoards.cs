using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.BussinessLogic
{
	public class BlBoards : BussinessLogicBase
	{
		public BlBoards(DataStorage storage, int loggedUser)
			: base(storage, loggedUser)
		{
		}

		public BoardInfo Add(NameValueCollection post, out string message)
		{
			string name = null;
			try
			{
				name = post["name"].Trim();
			}
			catch (Exception) { }
			message = "";

			if ((name != null && name != "")  && this.LoggedUser != 0)
			{
				BoardInfo boardInfo = this.DataStorage.AddBoard(name, this.LoggedUser);
				return boardInfo;
			}

			message = __("Parameters is invalid.");
			return null;
		}

		public BoardInfo SaveData(BoardInfo boardInfo, out string message)
		{
			message = "";		

			if (this.LoggedUser > 0 && boardInfo != null)
			{
				if (!this.DataStorage.CheckBoardInData(boardInfo.Id))
				{
					boardInfo = DataStorage.SyncBoard(boardInfo, this.LoggedUser, true);
					return boardInfo;
				}
				else
				{
					boardInfo = DataStorage.SyncBoard(boardInfo, this.LoggedUser, false);
					return boardInfo;
				}
			}
			message = __("Parameters is invalid.");
			return null;
		}


		public BoardInfo GetOne(NameValueCollection post, out string message)
		{
			message = "";

			if (post["boardId"] != null && this.LoggedUser != 0)
			{
				Guid boardId = Guid.Empty;
				if (Guid.TryParse(post["boardId"], out boardId))
				{
					BoardInfo boardInfo = this.DataStorage.GetBoardWithId(boardId);
					return boardInfo;
				}
			}

			message = __("Parameters is invalid.");
			return null;
		}


		public List<BoardInfo> GetAll(NameValueCollection post, out string message)
		{
			message = "";

			if (this.LoggedUser != 0)
			{
				List<BoardInfo> boardInfos = this.DataStorage.GetAllBoardOfAccount(this.LoggedUser);
				return boardInfos;
			}

			message = __("Parameters is invalid.");
			return null;

		}


		public BoardInfo Update(NameValueCollection post, out string message)
		{
			string name = null, id = null;
			message = "";

			name = post["name"].Trim();
			id = post["boardId"].Trim();

			if ((name != null && name != "") && id != null && this.LoggedUser != 0)
			{
				Guid boardId = Guid.Empty;
				if (Guid.TryParse(id, out boardId))
				{
					BoardInfo boardInfo = this.DataStorage.UpdateBoard(boardId, name);
					return boardInfo;
				}
			}

			message = __("Parameters is invalid");
			return null;
		}


		public bool Delete(NameValueCollection post, out string message)
		{
			message = "";
			if (post["boardId"] != null && this.LoggedUser != 0)
			{
				Guid boardId = Guid.Empty;
				if (Guid.TryParse(post["boardId"], out boardId))
				{
					bool isDeleted = this.DataStorage.DeleteBoard(boardId);
					return isDeleted;
				}
			}

			message = __("Parameters is invalid");
			return false;
		}

		public List<BoardInfo> SyncToDataStorage(List<BoardInfo> data)	
		{
			List<BoardInfo> result = new List<BoardInfo>();
			if (data == null || data.Count == 0)
				return result;
			if(this.LoggedUser > 0)
			{
				foreach (BoardInfo item in data)
				{
					if (!this.DataStorage.CheckBoardInData(item.Id))
						DataStorage.SyncBoard(item, this.LoggedUser, true);
					else
					{
						if (HasPermission("edit", item))
						{
							DataStorage.SyncBoard(item, this.LoggedUser, false);
						}
					}
				}
				result = this.DataStorage.GetAllBoardOfAccount(this.LoggedUser);
			}
			
			return result;
		}

		public bool HasPermission(string permission, BoardInfo board)
		{
			if (permission == "edit")
			{
				// if the logger is owner
				if (this.LoggedUser == board.Owner)
					return true;
				// if the logger is manager of owner
			}
			else if (permission == "delete")
			{
				// 
			}
			return false;
		}

		public BoardInfo HashCode(BoardInfo data)
		{

			if(data != null)
			{
				BoardInfo boardInfo = this.DataStorage.CalHashCode(data);
				return boardInfo;
			}
			return null;
		}
	}
}
