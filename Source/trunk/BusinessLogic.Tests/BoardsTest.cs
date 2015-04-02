using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.BusinessLogic.Tests
{
    [TestFixture]
    public class BoardsTest : TestBase
    {
        
        [TestCase]
        public void Add() {
            BussinessLogic.BlBoards boardsLogic = new BussinessLogic.BlBoards(this.DataStorage, 1);

            string message;

        /*    NameValueCollection case1 = new NameValueCollection();
            BoardInfo board = boardsLogic.Add(case1, out message);
            NUnit.Framework.Assert.AreEqual(1, board);

            NameValueCollection case2 = new NameValueCollection();
            case2["name"] = "";
            board = boardsLogic.Add(case2, out message);
            NUnit.Framework.Assert.AreEqual(1, board);

            NameValueCollection case3 = new NameValueCollection();
            case3["name"] = "nhat";
            board = boardsLogic.Add(case3, out message);
            NUnit.Framework.Assert.AreEqual("nhat", board.Name); */

			NameValueCollection case1 = new NameValueCollection();
			case1["name"] = "Huy";
			BoardInfo board = boardsLogic.Add(case1, out message);
			Assert.NotNull(board);
        }
    }
}
