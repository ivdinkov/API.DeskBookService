namespace API.DeskBookService.Core.Conracts.v1.Responses
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
        NoDeskAvailable
    }
}
