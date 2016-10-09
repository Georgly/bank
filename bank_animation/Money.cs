using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_animation
{
    class Money
    {
        int _cost;
        int _count;

        public Money(int cost, int count)
        {
            Cost = cost;
            Count = count;
        }

        public int Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
    }
}
