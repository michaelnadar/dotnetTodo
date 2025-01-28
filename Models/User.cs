using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // Navigation property for one-to-many relationship
        [JsonIgnore]
        public ICollection<ToDoList> ToDoLists { get; set; } = new List<ToDoList>();
       
    }
}
