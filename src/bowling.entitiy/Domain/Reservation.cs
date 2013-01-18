﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Entity.Domain
{
    public class Reservation : SharpLite.Domain.Entity
    {

        public virtual DateTime PlayAt { get; set; }
        public virtual int NumberOfPlayers { get; set; }
        public virtual String Name { get; set; }
        public virtual String PhoneNumber { get; set; }
        public virtual Member Member { get; set; }
        public virtual ReservationStatus Status { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        private IList<Lane> lanes = new List<Lane>();

        public virtual IList<Lane> Lanes
        {
            get
            {
                return this.lanes;
            }
            protected set
            {
                this.lanes = value;
            }
        }

        public virtual void AddLane(Lane lane)
        {
            Lanes.Add(lane);
        }
    }
}
