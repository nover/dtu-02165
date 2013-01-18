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
        [Range(1, 24, ErrorMessage="Please enter a number between 1 and 24. If you are more persons, please contact sales for a package deal")]
        public int NumberOfPeople { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PlayAt { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartPlayingAt { get; set; }

        [Required]
        [Range(1, 9, ErrorMessage = "Please enter a number between 1 and 9")]
        public int HowManyHours { get; set; }
    }
}