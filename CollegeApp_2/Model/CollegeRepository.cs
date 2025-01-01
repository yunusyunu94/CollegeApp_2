namespace CollegeApp_2.Model
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>(){
                new Student
                {
                    Id = 1,
                    StudentName = "Yunus",
                    Email ="yunus@hotmail.com",
                    Adres = "Hyd, INDIA",
                    Age = 11,
                    //AdmissionDate = DateTime.Now,
                    Password =  "f",
                    ConfirmPassword = "f"
                    
                },
                new Student
                {
                    Id = 2,
                    StudentName = "Anil",
                    Email = "Anil@hotmail.com",
                    Adres = "Banglore, INDIA",
                    Age = 11,
                    //AdmissionDate = DateTime.Now,
                    Password =  "f",
                    ConfirmPassword = "f"

                }
         };
    }
}
