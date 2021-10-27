using API.DeskBookService.Core.Domain;

namespace API.DeskBookService.Core.Conracts.Responses
{
    /// <summary>
    /// DeskBookingResult with result Code and DeskBookingId
    /// </summary>
    public class DeskBookingResult : DeskBookingBase
    {
        /// <summary>
        /// Result code
        /// </summary>
        public DeskBookingResultCode Code { get; set; }
        
        /// <summary>
        /// DeskBookingId
        /// </summary>
        public string DeskBookingId { get; set; }
    }
}
