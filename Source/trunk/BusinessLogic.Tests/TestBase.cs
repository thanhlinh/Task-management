using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sioux.TaskManagement.BusinessLogic.Tests
{
    public class TestBase
    {
        [NUnit.Framework.SetUp]
        public virtual void SetUp()
        {
			DataStorage = this.DataStorage;
            Console.WriteLine("set up");
        }

        [NUnit.Framework.TearDown]
        public virtual void TearDown()
        {
            Console.WriteLine("tear down");
        }

        public DataStorage DataStorage { get; protected set; }
    }
}
