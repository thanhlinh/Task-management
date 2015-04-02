using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.BussinessLogic
{
	public class BlCards : BussinessLogicBase
	{
		public BlCards(DataStorage storage, int loggedUser)
			:base(storage, loggedUser)
		{
		}

		public CardInfo Add(NameValueCollection post, out string message)
		{
			message = "";

			if(post["name"] != null && post["name"].Trim() != "" && post["boardId"] != null & post["columnId"] != null && this.LoggedUser != 0)
			{
				string name = post["name"].Trim();
				string description = post["description"].Trim();
				Guid boardId = Guid.Empty;
				Guid columnId = Guid.Empty;
				if(Guid.TryParse(post["boardId"], out boardId) && Guid.TryParse(post["columnId"], out columnId))
				{
					CardInfo cardInfo = this.DataStorage.AddCard(columnId, name, description);
					return cardInfo;
				}
			}

			message = __("Parameters is invalid");
			return null;
		}

		public CardInfo SaveData(CardInfo cardInfo, out string message)
		{
			message = "";
			if (this.LoggedUser > 0 && cardInfo != null)
			{
				if (!this.DataStorage.CheckCardInData(cardInfo.Id))
				{
					cardInfo = DataStorage.SyncCard(cardInfo, this.LoggedUser, true);
					return cardInfo;
				}
				else
				{
					cardInfo = DataStorage.SyncCard(cardInfo, this.LoggedUser, false);
					return cardInfo;
				}
			}
			message = __("Parameters is invalid.");
			return null;
		}

		public List<CardInfo> GetAll(NameValueCollection post, out string message)
		{
			message = "";
			if(post["columnId"] != null && this.LoggedUser != 0)
			{
				Guid columnId = Guid.Empty;
				if(Guid.TryParse(post["columnId"], out columnId))
				{
					List<CardInfo> cardInfos = this.DataStorage.GetAllCardOfColumn(columnId);
					return cardInfos;
				}
			}

			message = __("Parameters is invalid");
			return null;
		}

		public CardInfo GetOne(NameValueCollection post, out string message)
		{
			message = "";
			if(post["cardId"] != null && this.LoggedUser != 0)
			{
				Guid cardId = Guid.Empty;
				if(Guid.TryParse(post["cardId"], out cardId))
				{
					CardInfo cardInfo = this.DataStorage.GetCardInfo(cardId);
					return cardInfo;
				}
			}

			message = __("Parameters is invalid");
			return null;
		}

		public CardInfo Update(NameValueCollection post, out string message)
		{
			message = "";

			if (post["name"] != null && post["name"].Trim() != "" && post["cardId"] != null && this.LoggedUser != 0)
			{
				string name = post["name"].Trim();
				string description = post["description"].Trim();
				Guid cardId = Guid.Empty;
				if(Guid.TryParse(post["cardId"], out cardId))
				{
					CardInfo cardInfo = this.DataStorage.UpdateCard(cardId, name, description);
					return cardInfo;
				}
			}

			message = __("Parameters is invalid");
			return null;
		}

		public bool Delete(NameValueCollection post, out string message)
		{
			message = "";

			if(post["cardId"] != null && this.LoggedUser != 0)
			{
				Guid cardId = Guid.Empty;
				if(Guid.TryParse(post["cardId"], out cardId))
				{
					bool isDeleted = this.DataStorage.DeleteCard(cardId);
					return isDeleted;
				}
			}

			message = __("Parameters is invalid");
			return false;
		}

		public bool HasPermission(string permission, CardInfo cardInfo)
		{
			if (permission == "edit")
			{
				// if the logger is owner
				if (this.LoggedUser == cardInfo.Owner)
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
