using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
    public class AppWeightPair
    {
        public bool applicable;
        public double weight;

        public AppWeightPair(bool applicable, double weight)
        {
            this.applicable = applicable;
            this.weight = weight;
        }
    }
}
