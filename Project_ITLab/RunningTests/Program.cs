using Project_ITLab.Models.Domain;
using System;
using System.Collections;

namespace RunningTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //DomainController dc = new DomainController();
            //dc.fillSessionService();
            
            //Console.WriteLine("Hello World!");
            ////ICollection<Session> sessions = dc.ListSessions();
            //foreach(Session session in dc.GetSessions())
            //{
            //    Console.WriteLine($"Session name: {session.Name} happening on {session.StartTime.Hour}:{session.StartTime.Minute}");
            //}

            //Console.WriteLine(Environment.NewLine + "By Time:");
            //string toPrint;

            //foreach (Session session in dc.GetSessionsByTime(new DateTime(2020, 2, 1, 0, 0, 0), new DateTime(2020, 3, 1, 0, 0, 0)))
            //{
            //    toPrint = $"Session name: {session.Name} happening on {session.StartTime.Hour}:{session.StartTime.Minute} and led by: ";
            //    foreach(User leader in session.Leaders)
            //    {
            //        toPrint += $"{leader.FirstName} {leader.LastName},";
            //    }
            //    Console.WriteLine(toPrint);
            //}

            //new DateTime(2020, 2, 1, 0, 0, 0)
            //to
            //new DateTime(2020, 3, 1, 0, 0, 0)
        }
    }
}
