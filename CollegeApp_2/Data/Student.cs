using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeApp_2.Data
{
    public class Student
    {
        //[Key] StudentConfigda tanimladik
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]  StudentConfigda tanimladik
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public DateTime DOB { get; set; }

        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; } // İliskiyi kurunca konfig sinifina git
    }
}
