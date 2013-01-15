using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bowling.Web.CustomerSite.Models
{
	public class MemberInputModel
	{
        public MemberInputModel()
        {
            this.ReceieveNewsletter = true;
            this.DialCode = "+45";
            this.DefaultNumberOfPlayers = 1;
        }

		public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
        public string Title { get; set; }

        public string DialCode { get; set; }

        [DataType(DataType.PhoneNumber)]
        public int CellPhone { get; set; }

        [Range(1, 120, ErrorMessage = "Please enter a number between 1 and 120")]
        public int DefaultNumberOfPlayers { get; set; }

        public bool ReceieveNewsletter { get; set; }
	}
}