using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.DeskBookService.Core.Domain
{
    /// <summary>
    /// Desk with Id, Name and Description
    /// </summary>
    public class Desk : DeskBase
    {
        /// <summary>
        /// The Id of the Desk
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
