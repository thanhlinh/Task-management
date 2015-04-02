using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sioux.TaskManagement.Controllers
{
	public class BoardsController : AppController
	{
		[HttpPost]
		[Authorize(Roles = Permission.Boards.Add)]
		public ActionResult Add()
		{
			string message;
			BussinessLogic.BlBoards blBoard = new BussinessLogic.BlBoards(this.DataStorage, this.AccountId);
			BoardInfo boardInfo = blBoard.Add(Request.Form, out message);

			return JsonFormat(boardInfo, message);
		}

		[HttpPost]
		[Authorize(Roles = Permission.Boards.Add)]
		public ActionResult SaveData()
		{
			string message;
			BoardInfo boardInfo = null;

			BussinessLogic.BlBoards blBoard = new BussinessLogic.BlBoards(this.DataStorage, this.AccountId);
			try
			{
				string json = Request.Form["data"];
				boardInfo = JsonConvert.DeserializeObject<BoardInfo>(json);
			}
			catch (Exception) { }
			boardInfo = blBoard.SaveData(boardInfo, out message);

			return JsonFormat(boardInfo, message);
		}

		[HttpGet]
		[Authorize(Roles = Permission.Boards.Get)]
		public ActionResult GetAll()
		{
			string message;

			BussinessLogic.BlBoards blBoard = new BussinessLogic.BlBoards(this.DataStorage, this.AccountId);
			List<BoardInfo> boardInfos = blBoard.GetAll(Request.QueryString, out message);

			return JsonFormat(boardInfos, message);
		}

		[HttpGet]
		[Authorize(Roles = Permission.Boards.Get)]
		public ActionResult GetOne()
		{
			string message;

			BussinessLogic.BlBoards blBoard = new BussinessLogic.BlBoards(this.DataStorage, this.AccountId);
			BoardInfo boardInfo = blBoard.GetOne(Request.QueryString, out message);

			return JsonFormat(boardInfo, message);
		}

		[HttpPost]
		[Authorize(Roles = Permission.Boards.Update)]
		public ActionResult Update()
		{
			string message;

			BussinessLogic.BlBoards blBoard = new BussinessLogic.BlBoards(this.DataStorage, this.AccountId);
			BoardInfo boardInfo = blBoard.Update(Request.Form, out message);

			return JsonFormat(boardInfo, message);
		}

		[HttpPost]
		[Authorize(Roles = Permission.Boards.Sync)]
		public ActionResult Sync()
		{
			List<BoardInfo> boards = new List<BoardInfo>();
			if (Request.Form["boards"] != null)
			{
				try
				{
					List<BoardInfo> inputs = JsonConvert.DeserializeObject<List<BoardInfo>>(Request.Form["boards"]);
					BussinessLogic.BlBoards blBoard = new BussinessLogic.BlBoards(this.DataStorage, this.AccountId);
					boards = blBoard.SyncToDataStorage(inputs);
				}
				catch (Exception) {
				}
			}
			return JsonFormat(boards);
		}

		[AllowAnonymous]
		public ActionResult HashCode()
		{
			BoardInfo boardInfo = null;
			if(Request["board"] != null)
			{
				try
				{
					BoardInfo board = JsonConvert.DeserializeObject<BoardInfo>(Request.Form["board"]);
					BussinessLogic.BlBoards blBoard = new BussinessLogic.BlBoards(this.DataStorage, this.AccountId);
					boardInfo = blBoard.HashCode(board);
				}
				catch (Exception) { }
			}

			return JsonFormat(boardInfo);
		}

		//[HttpGet]
		//[Authorize(Roles = Permission.Boards.Sync)]
		//public ActionResult Sync(string updated)
		//{
		//	// List<BoardInfo>
		//}
	}
}
