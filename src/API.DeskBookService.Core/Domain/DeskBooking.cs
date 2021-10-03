namespace API.DeskBookService.Core.Domain
{
    public class DeskBooking : DeskBookingBase
    {
        public string Id { get; set; }
        public string DeskId { get; set; }
    }
}