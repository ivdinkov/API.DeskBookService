using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.DeskBookService.Core.Domain
{
    /// <summary>
    /// Desk with Id, Name and Description
    /// </summary>
    public class Desk
    {
        /// <summary>
        /// The Id of the Desk
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// The Name of the Desk
        /// </summary>
        [BsonElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The description of the Desk
        /// </summary>
        public string Description { get; set; }
    }
}
