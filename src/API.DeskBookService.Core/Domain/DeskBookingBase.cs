using System;
using System.ComponentModel.DataAnnotations;

namespace API.DeskBookService.Core.Domain
{
    /// <summary>
    /// DeskBookingBase
    /// </summary>
    public class DeskBookingBase
    {
        /// <summary>
        /// The DeskId of the DeskBooking
        /// </summary>
        [Required]
        public string DeskId { get; set; }

        /// <summary>
        /// The first name of the person making the booking
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the person making the booking
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// The email of the person making the booking
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The message of the person making the booking
        /// </summary>
        [Required]
        public string Message { get; set; }

        /// <summary>
        /// The booking date of the booking
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

    }
}