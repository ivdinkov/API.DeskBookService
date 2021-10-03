namespace API.DeskBookService.Core.Domain
{
    public class DeskBookingResult : DeskBookingBase
    {
        public DeskBookingResultCode Code { get; set; }
        public string DeskBookingId { get; set; }
    }
}
