using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace UserDemoApp.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("firstName")]
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name must be between {2} and {1} characters.", MinimumLength = 1)]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name must be between {2} and {1} characters.", MinimumLength = 1)]
        public string LastName { get; set; }

        [BsonElement("contact")]
        public string Contact { get; set; } = string.Empty;
        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("email")]
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [BsonElement("consentGiven")]
        public bool ConsentGiven { get; set; } = true;
        [BsonElement("consentDate")]
        public DateTime ConsentDate { get; set; } = DateTime.UtcNow;

        public User()
        {
            // Generate a new ObjectId for the Id property
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}
