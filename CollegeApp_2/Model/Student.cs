using System.ComponentModel.DataAnnotations;

namespace CollegeApp_2.Model
{
    public class Student
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public int Age { get; set; }

        public DateTime AdmissionDate { get; set; } // ogrenci kabul tarihi

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
