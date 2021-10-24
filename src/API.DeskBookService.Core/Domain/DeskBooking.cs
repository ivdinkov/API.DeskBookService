using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.DeskBookService.Core.Domain
{
    /// <summary>
    /// DeskBooking with Id and Desk Id
    /// </summary>
    public class DeskBooking : DeskBookingBase
    {
        /// <summary>
        /// The Id of the DeskBooking
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

    }
}