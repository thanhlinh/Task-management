using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.BLA
{
    abstract class CommonAccess
    {
        public List<Card> GetAllCardOfColumn(int ColumnId);
    }
}
