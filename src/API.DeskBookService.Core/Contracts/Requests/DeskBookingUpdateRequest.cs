using System.ComponentModel.DataAnnotations;

namespace API.DeskBookService.Core.Contracts.Requests
{
    /// <summary>
    /// Desk update request
    /// </summary>
    public class DeskBookingUpdateRequest
    {
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

    }
}
