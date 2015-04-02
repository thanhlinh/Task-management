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
		//select card with id
		public override CardInfo GetCardInfo(Guid cardId)
		{
			Card card = dbContext.Cards.Where(c => c.Id == cardId && c.Status == Status.Enable).FirstOrDefault();

			return (CardInfo)card;
		}

		public override List<CardInfo> GetAllCardOfColumn(Guid columnId)
		{

			List<Card> cards = dbContext.Cards.Where(c => c.ColumnId.Equals(columnId) && c.Status == Status.Enable).ToList();
			List<CardInfo> cardInfos = new List<CardInfo>();
			foreach(Card card in cards)
			{
				cardInfos.Add(card);
			}
			return cardInfos;
		}

		public override CardInfo AddCard(Guid columnId, string name, string description)
		{
			Column column = dbContext.Columns.Where(c => c.Id.Equals(columnId) && c.Status == Status.Enable).FirstOrDefault();
			if(column != null)
			{
				Card card = new Card() { 
					Id = Guid.NewGuid(),
					Name = name,
					Description = description,
					Owner = column.Owner,
					BoardId = column.BoardId,
					ColumnId = column.Id,
					Priority = column.Priority + 1,
					Style = "default",
					CheckListCount = 0,
					CommentCount = 0,
					RunningTimeLog = 0,
					CurrentEstimated = 0,
					AttachedCount = 0,
					Created = DateTime.Now,
					Updated = DateTime.Now,
					Status = Status.Enable
				};
				CardInfo cardInfo = (CardInfo)card;
				try
				{
					column.CardCount++;
					dbContext.Cards.Add(card);
					dbContext.SaveChanges();

					return cardInfo;
				}
				catch (DbEntityValidationException) { }
			}		

			return null;
		}


		public override CardInfo UpdateCard(Guid cardId, string name, string desciption)
		{
			Card card = dbContext.Cards.Where(c => c.Id.Equals(cardId) && c.Status == Status.Enable)
				.FirstOrDefault();

			if (card != null)
			{
				card.Name = name;
				card.Description = desciption;
				card.Updated = DateTime.UtcNow;

				CardInfo cardInfo = (CardInfo)card;

				try
				{
					dbContext.SaveChanges();
					return cardInfo;
				}
				catch (DbEntityValidationException) { }
			}

			return null;
		}

		public override bool DeleteCard(Guid cardId)
		{
			Card card = dbContext.Cards.Where(c => c.Id.Equals(cardId) && c.Status == Status.Enable).FirstOrDefault();
			Column column = dbContext.Columns.Where(c => c.Id.Equals(card.ColumnId) && c.Status == Status.Enable).FirstOrDefault();

			if (card != null)
			{
				card.Status = Status.Deleted;
				card.Updated = DateTime.UtcNow;

				try
				{
					column.CardCount--;
					dbContext.SaveChanges();
					return true;
				}
				catch (DbEntityValidationException) { }
			}

			return false;
		}

		
		public override CardInfo SyncCard(CardInfo cardInfo, int owner, bool isAdd)
		{
			if(isAdd)
			{
				Card card = new Card()
				{
					Id = cardInfo.Id,
					Name = cardInfo.Name,
					Description = cardInfo.Description,
					Owner = owner,
					BoardId = cardInfo.BoardId,
					ColumnId = cardInfo.ColumnId,
					Priority = cardInfo.Priority,
					Style = cardInfo.Style,
					CheckListCount = cardInfo.CheckListCount,
					CommentCount = cardInfo.CommentCount,
					RunningTimeLog = cardInfo.RunningTimeLog,
					CurrentEstimated =cardInfo.CurrentEstimated,
					AttachedCount = cardInfo.AttachedCount,
					Created = cardInfo.Created,
					Updated = cardInfo.Updated,
					Status = Status.Enable
				};
				
				try
				{
					dbContext.Cards.Add(card);
					dbContext.SaveChanges();

					return (CardInfo)card;
				}
				catch (Exception) { return null; }
			}
			else
			{
				Card card = dbContext.Cards.FirstOrDefault(c => c.Id.Equals(cardInfo.Id) && c.Status == Status.Enable);
				card.Name = cardInfo.Name;
				card.Description = cardInfo.Description;
				card.Owner = cardInfo.Owner;
				card.BoardId = cardInfo.BoardId;
				card.ColumnId = cardInfo.ColumnId;
				card.Priority = cardInfo.Priority;
				card.Style = cardInfo.Style;
				card.CheckListCount = cardInfo.CheckListCount;
				card.CommentCount = cardInfo.CommentCount;
				card.RunningTimeLog = cardInfo.RunningTimeLog;
				card.CurrentEstimated =cardInfo.CurrentEstimated;
				card.AttachedCount = cardInfo.AttachedCount;
				card.Created = cardInfo.Created;
				card.Updated = cardInfo.Updated;
				card.Status = cardInfo.Status;

				try
				{
					dbContext.SaveChanges();
					return (CardInfo) card;
				}
				catch (Exception) {
					return null;
				}
			}
		}

		public override bool CheckCardInData(Guid cardId)
		{
			Card card = dbContext.Cards.FirstOrDefault(c => c.Id.Equals(cardId) && c.Status != Status.Deleted);

			return card != null;
		}
	}
}
