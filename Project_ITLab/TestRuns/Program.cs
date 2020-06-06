using Project_ITLab.Data.ServiceInstances;
using Project_ITLab.Models.Domain;
using System;

namespace TestRuns
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            UserService userServ = new UserService();
            userServ.LogIn("dzhemaptula");
            Console.WriteLine(userServ.GetCurrentUser().FirstName);
        }
    }
}
