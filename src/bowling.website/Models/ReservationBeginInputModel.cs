using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bowling.Web.CustomerSite.Models
{
    public class ReservationBeginInputModel
    {
        [Required]
        public int NumberOfPeople { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PlayAt { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartPlayingAt { get; set; }

        [Required]
        public TimeSpan StopPlayingAt { get; set; }
    }
}