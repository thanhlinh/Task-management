using Sioux.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement
{
	public abstract class DataStorage
	{
		//------------------------- account -----------------------------//
		public abstract AccountInfo GetAccountWithId(int accountId);

		public abstract AccountInfo GetAccountWithEmail(string email);

		public abstract AccountInfo GetAccountWithUserId(int userId);

		public abstract List<AccountInfo> GetAllAccountInfo();

		public abstract AccountInfo AddAccount(string email, string firstName, string lastName, string password, int userId, string avatar, bool isExternal, string role, Status status);

		public abstract AccountInfo UpdateAccountUserId(int acocuntId, int userId);

		public abstract AccountInfo UpdateAccountName(int accountId, string firstName, string lastName);

		public abstract AccountInfo UpdateAccountPassword(int accountId, string password);

		public abstract AccountInfo UpdateAccountIsExternal(int accountId, bool isExternal);

		public abstract AccountInfo UpdateAccountAvatar(int accountId, string avatar);

		public abstract AccountInfo UpdateAccountStatus(int accountId, Status status);

		public abstract AccountInfo Upgrade(int accountId);


		//------------------------ board ------------------------------//
		public abstract BoardInfo SyncBoard(BoardInfo board, int owner, bool isAdd);

		public abstract BoardInfo GetBoardWithId(Guid boardId);

		public abstract List<BoardInfo> GetAllBoardOfAccount(int owner);

		public abstract BoardInfo UpdateBoard(Guid boardId, string name);

		public abstract BoardInfo AddBoard(string name, int owner);

		public abstract bool DeleteBoard(Guid boardId);

		public abstract bool CheckBoardInData(Guid boardId);

		public abstract BoardInfo CalHashCode(BoardInfo boardInfo);

		//------------------------ card -----------------------------//

		public abstract CardInfo SyncCard(CardInfo cardInfo, int owner, bool isAdd);

		public abstract CardInfo GetCardInfo(Guid cardId);

		public abstract List<CardInfo> GetAllCardOfColumn(Guid columnId);

		public abstract CardInfo AddCard(Guid columnId, string name, string description);

		public abstract CardInfo UpdateCard(Guid cardId, string name, string desciption);

		public abstract bool DeleteCard(Guid cardId);

		public abstract bool CheckCardInData(Guid cardId);


		//-------------------------- column ---------------------------//

		public abstract ColumnInfo SyncColumn(ColumnInfo column, int owner, bool isAdd);

		public abstract ColumnInfo GetColumnInfo(Guid columnId);

		public abstract List<ColumnInfo> GetAllColumnOfBoard(Guid boardId);

		public abstract ColumnInfo AddColumn(string name, int ownerId, Guid boardId);

		public abstract ColumnInfo UpdateColumn(Guid columnId, string name);

		public abstract bool DeleteColumn(Guid columnId);

		public abstract bool CheckColumnInData(Guid columnId);


		//-------------------------- columnsetting ----------------------//

		public abstract ColumnSettingInfo SyncColumnSetting(ColumnSettingInfo settingInfo, bool isAdd);

		public abstract List<ColumnSettingInfo> GetAllSettingOfBoard(Guid boardId);

		public abstract ColumnSettingInfo AddSetting(Guid columnId, Guid next, Guid boardId);

		public abstract bool DeleteSetting(Guid columnId, Guid next);

		public abstract bool CheckSettingInData(Guid columnId, Guid next);
	}
}
