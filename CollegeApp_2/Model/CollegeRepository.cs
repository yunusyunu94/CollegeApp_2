namespace CollegeApp_2.Model
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>(){
                new Student
                {
                    Id = 1,
                    StudentName = "Student 1",
                    Email ="Studentemail1@hotmail.com",
                    Adres = "Hyd, INDIA"

                },
                new Student
                {
                    Id = 2,
                    StudentName = "Student 2",
                    Email = "Studentemail1@hotmai2.com",
                    Adres = "Banglore, INDIA"

                }
         };
    }
}
