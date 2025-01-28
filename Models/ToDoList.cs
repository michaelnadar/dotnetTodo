using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{

    public enum Priority
{
    Low,
    Medium,
    High
}

public enum Category
{
    Work,
    Personal
}

    public class ToDoList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        public int UserID { get; set; }

        
        [ForeignKey("UserID")]
        public User User { get; set; }

        // Foreign Key to Priorities
        [Required]
        public Priority Priority { get; set; }
        [Required]
        public Category Category { get; set; }
    }
}
