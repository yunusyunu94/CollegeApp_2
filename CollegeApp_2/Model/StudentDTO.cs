using System.ComponentModel.DataAnnotations;

namespace CollegeApp_2.Model
{
    public class StudentDTO
    {
        public int Id { get; set; }

        [Required]
        public string StudentName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Adres { get; set; }
    }
}
