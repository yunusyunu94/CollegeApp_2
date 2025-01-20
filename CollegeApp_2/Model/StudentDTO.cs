using CollegeApp_2.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp_2.Model
{
    public class StudentDTO
    {
        //[ValidateNever]                 // Dogrulanmasini istemedigimiz alanlar icin kullanilir
        public int Id { get; set; }

        //[Required]
        public string StudentName { get; set; }

        //[EmailAddress]
        public string Email { get; set; }

        //[Range(10,20)]                  // 10 ile 20 arasinda olmalidir
        //public int Age { get; set; }

        //[DataCheck]
        //public DateTime AdmissionDate { get; set; } // ogrenci kabul tarihi
        //// StudentController de DateTime kontrolunu CreateStudent Actionda sagladik

        //[Required]
        public string Adres { get; set; }

        public DateTime DOB { get; set; }

        //public string Password { get; set; }

        //[Compare(nameof(Password))]           // sifre onaylama 
        //public string ConfirmPassword { get; set; }
    }
}
