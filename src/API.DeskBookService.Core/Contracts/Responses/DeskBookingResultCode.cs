namespace API.DeskBookService.Core.Conracts.Responses
{
    /// <summary>
    /// DeskBooking result codes
    /// </summary>
    public enum DeskBookingResultCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success,
        /// <summary>
        /// No desk available for this date
        /// </summary>
        NoDeskAvailable,
        /// <summary>
        /// Booking desk Id
        /// </summary>
        InvalidDeskId
    }
}
