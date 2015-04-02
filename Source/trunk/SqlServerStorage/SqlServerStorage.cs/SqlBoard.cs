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
		public override BoardInfo GetBoardWithId(Guid boardId)
		{
			Board board = dbContext.Boards.Where(b => b.Id.Equals(boardId) && b.Status == Status.Enable).FirstOrDefault();
			if(board != null)
			{
				return (BoardInfo)board;
			}

			return null;
		}

		public override List<BoardInfo> GetAllBoardOfAccount(int owner)
		{
			List<Board> boards = dbContext.Boards.Where(b => b.Owner == owner && b.Status == Status.Enable).ToList();
			List<BoardInfo> boardInfos = new List<BoardInfo>();
			foreach(Board board in boards)
			{
				boardInfos.Add((BoardInfo)board);
			}
			return boardInfos;
		}

		public override BoardInfo AddBoard(string name, int owner)
		{
			Board board = new Board() {
 				Id = Guid.NewGuid(),
				Name = name,
				Owner = owner,
				Priority = 0,
				ColumnCount = 0,
				Created = DateTime.UtcNow,
				Updated = DateTime.UtcNow,
				Status = Status.Enable
			};

			BoardInfo boardInfo = (BoardInfo)board;

			try
			{
				dbContext.Boards.Add(board);
				dbContext.SaveChanges();
				return boardInfo;
			}
			catch (Exception) { }

			return null;
		}

		//update board
		public override BoardInfo UpdateBoard(Guid boardId, string name)
		{
			Board board = dbContext.Boards.Where(b => b.Id.Equals(boardId)).FirstOrDefault();
			board.Name = name;
			board.Updated = DateTime.Now;

			BoardInfo boardInfo = (BoardInfo)board;


			try
			{
				dbContext.Boards.Add(board);
				dbContext.SaveChanges();
				return boardInfo;
			}
			catch (Exception) { }


			return null;
		}


		//delete board in database
		public override bool DeleteBoard(Guid boardId)
		{
			try
			{
				Board board = dbContext.Boards.Where(b => b.Id.Equals(boardId)).FirstOrDefault();

				if (board != null)
				{
					board.Status = Status.Deleted;
					board.Updated = DateTime.Now;

					List<Column> columns = dbContext.Columns.Where(c => c.BoardId.Equals(boardId) && c.Status == Status.Enable).ToList();
					List<Card> cards = dbContext.Cards.Where(c => c.BoardId.Equals(boardId) && c.Status == Status.Enable).ToList();

					foreach (Column column in columns)
					{
						column.Status = Status.Deleted;
						column.Updated = DateTime.Now;
					}

					foreach (Card card in cards)
					{
						card.Status = Status.Deleted;
						card.Updated = DateTime.Now;
					}
				}
				dbContext.SaveChanges();
				return true;
			}
			catch (Exception)
			{
			}

			return false;
		}

		public override BoardInfo SyncBoard(BoardInfo boardInfo, int owner, bool isAdd)
		{
			if (isAdd)
			{
				// add
				Board board = new Board()
				{
					Id = boardInfo.Id,
					Name = boardInfo.Name,
					Owner = owner,
					Priority = boardInfo.Priority,
					ColumnCount = boardInfo.ColumnCount,
					Created = boardInfo.Created,
					Updated = boardInfo.Updated,
					Status = Status.Enable
				};

				board.HashCode = board.CalcHashcode();

				boardInfo = (BoardInfo)board;

				try{
					dbContext.Boards.Add(board);
					dbContext.SaveChanges();
					return boardInfo;
				}catch(DbEntityValidationException)
				{
					return null;
				}
			}
			else
			{
				// update
				Board board = dbContext.Boards.Where(b => b.Id.Equals(boardInfo.Id)).FirstOrDefault();
				if(board != null)
				{
					board.Name = boardInfo.Name;
					board.Owner = boardInfo.Owner;
					board.Priority = boardInfo.Priority;
					board.ColumnCount = boardInfo.ColumnCount;
					board.Created = boardInfo.Created;
					board.Updated = boardInfo.Updated;
					board.Status = board.Status;
					board.HashCode = board.CalcHashcode();
					try
					{
						dbContext.SaveChanges();
						return (BoardInfo)board;
					}
					catch (DbEntityValidationException)
					{
						return null;
					}
				}
				return null;
			}
		}


		public override BoardInfo CalHashCode(BoardInfo boardInfo)
		{
			Board board = new Board() {
				Id = boardInfo.Id,
				Name = boardInfo.Name,
				Owner = boardInfo.Owner,
				Priority = boardInfo.Priority,
				ColumnCount = boardInfo.ColumnCount,
				Created = boardInfo.Created,
				Updated = boardInfo.Updated,
				Status = Status.Enable
			};

			board.HashCode = board.CalcHashcode();

			boardInfo = (BoardInfo)board;

			return boardInfo;
		}

		public override bool CheckBoardInData(Guid boardId)
		{
			Board board = dbContext.Boards.FirstOrDefault(b => b.Id.Equals(boardId) && b.Status == Status.Enable);
			return board != null;
		}
	}
}
