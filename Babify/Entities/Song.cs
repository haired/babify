using MongoDB.Bson.Serialization.Attributes;

namespace Babify
{
    public class Song
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public int PublishingYear { get; set; }

        public string ArtistName { get; set; }
    }
}
