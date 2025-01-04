namespace CollegeApp_2.Mylogging
{
    public class LogToFile : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("LogToFile");

            // write your own logic to save the logs to file
        }
    }
}
