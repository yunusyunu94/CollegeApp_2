namespace CollegeApp_2.Mylogging
{
    public class LogToServerMemory : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("LogToServerManager");

            // write your own logic to save the logs to Server memory

        }
    
       
    }
}
