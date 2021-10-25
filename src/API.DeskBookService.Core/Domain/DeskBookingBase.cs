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
        [MinLength(24,ErrorMessage = "The field dskId must be a string with a minimum length of '24'.")]
        [MaxLength(24, ErrorMessage = "The field dskId must be a string with a MAXIMUM length of '24'.")]                
        public string DeskId { get; set; }

        /// <summary>
        /// The first name of the person making the booking
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the person making the booking
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string LastName { get; set; }

        /// <summary>
        /// The email of the person making the booking
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The message of the person making the booking
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(60)]
        public string Message { get; set; }

        /// <summary>
        /// The booking date of the booking
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

    }
}