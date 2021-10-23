using System;

namespace API.DeskBookService.Core.Domain
{
    /// <summary>
    /// DeskBookingBase
    /// </summary>
    public class DeskBookingBase
    {
        /// <summary>
        /// The first name of the person making the booking
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the person making the booking
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email of the person making the booking
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The message of the person making the booking
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The booking date of the booking
        /// </summary>
        public DateTime Date { get; set; }

    }
}