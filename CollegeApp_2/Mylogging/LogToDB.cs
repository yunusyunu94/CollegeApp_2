namespace CollegeApp_2.Mylogging
{
    public class LogToDb : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("LogToDB");
        
            // write your own logic to save the logs to Db

        }
    }
}
