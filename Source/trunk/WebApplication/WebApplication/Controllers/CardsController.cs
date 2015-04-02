using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sioux.TaskManagement.Controllers
{
    public class CardsController : AppController
    {
		[HttpPost]
		public ActionResult SaveData()
		{
			string message;
			CardInfo cardInfo = null;
			try
			{
				string json = Request.Form["data"];
				cardInfo = JsonConvert.DeserializeObject<CardInfo>(json);
			}
			catch (Exception) { }
			
			BussinessLogic.BlCards blCard = new BussinessLogic.BlCards(this.DataStorage, this.AccountId);
			cardInfo = blCard.SaveData(cardInfo, out message);

			return JsonFormat(cardInfo, message);
		}


		[HttpGet]
		[Authorize(Roles = Permission.Cards.Get)]
		public ActionResult GetOne()
		{
			string message;
			BussinessLogic.BlCards blCard = new BussinessLogic.BlCards(this.DataStorage, this.AccountId);
			CardInfo cardInfo = blCard.GetOne(Request.QueryString, out message);

			return JsonFormat(cardInfo, message);
		}


		[HttpGet]
		[Authorize(Roles = Permission.Cards.Get)]
		public ActionResult GetAll()
		{
			string message;
			BussinessLogic.BlCards blCard = new BussinessLogic.BlCards(this.DataStorage, this.AccountId);
			List<CardInfo> cardInfos = blCard.GetAll(Request.QueryString, out message);

			return JsonFormat(cardInfos, message);
		}
    }
}
