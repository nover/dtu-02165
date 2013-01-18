using Bowling.Rest.Service.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
	public class ReservationPossibleResponse
	{
		public bool IsPossible { get; set; }

		public IList<ReservationType> Suggestions { get; set; }

		public ReservationPossibleResponse()
		{
			this.Suggestions = new List<ReservationType>();
		}
	}
}
