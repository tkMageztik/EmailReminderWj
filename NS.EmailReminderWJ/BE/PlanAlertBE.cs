using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace EmailReminderWJ.BE
{
    public class PlanAlertBE
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        [BsonElement("Customer")]
        public string Customer { get; set; }

        [BsonElement("Concept")]
        public string Concept { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Charge")]
        public decimal Charge { get; set; }

        [BsonElement("DueDate")]
        public DateTime DueDate { get; set; }

        [BsonElement("AlertDate")]
        public DateTime AlertDate { get; set; }

        [BsonElement("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("Comment")]
        public string Comment { get; set; }

        [BsonElement("Enabled")]
        public bool Enabled { get; set; }

    }
}
