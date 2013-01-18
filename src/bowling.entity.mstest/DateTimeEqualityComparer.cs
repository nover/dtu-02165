using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.entity.mstest
{
    class DateTimeEqualityComparer : IEqualityComparer
    {
        private TimeSpan maxDifference;

        public DateTimeEqualityComparer()
            : this(TimeSpan.FromSeconds(1))
        {
        }

        public DateTimeEqualityComparer(TimeSpan maxDifference)
        {
            this.maxDifference = maxDifference;
        }

        public bool Equals(object x, object y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            else if (x is DateTime && y is DateTime)
            {
                var dt1 = (DateTime)x;
                var dt2 = (DateTime)y;
                var duration = (dt1 - dt2).Duration();
                return duration < maxDifference;
            }
            return x.Equals(y);
        }

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
