using Bowling.Rest.Service.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Model.Operations
{
	public class ReservationsResponse
    {
        #region GET collection
        /// <summary>
        /// The list of operations associated with the parameters from the GET on the collection
        /// </summary>
        public IList<ReservationType> ReservationList { get; set; }
        #endregion
        #region POST/GET member operation
        /// <summary>
        /// The hydrated DB member resulting from either a POST or a GET for a sigle element
        /// </summary>
        public ReservationType Reservation { get; set; }
        #endregion

		public ReservationsResponse()
		{
			this.Reservation = new ReservationType();
            this.ReservationList = new List<ReservationType>();

		}
	}
}
