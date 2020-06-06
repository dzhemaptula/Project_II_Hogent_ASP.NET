using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project_ITLab.Models.Domain;
using Project_ITLab.Models.Enums;

namespace Project_ITLab.Data {
    public class DataInit {
        private readonly Context context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DataInit(Context con, UserManager<IdentityUser> userMan, RoleManager<IdentityRole> roleMan) {
            context = con;
            userManager = userMan ?? throw new ArgumentException("Error user manager ItLabContext");
            roleManager = roleMan ?? throw new ArgumentException("Error role manager ItLabContext");
        }

        public async Task InitAsync() {
            context.Database.EnsureDeleted();
            if (context.Database.EnsureCreated()) {
                InitData();
                await InitializeRoles();
                await InitializeUsers();
            }
        }

        private async Task InitializeRoles() {
            string[] roles = { Role.HeadAdmin, Role.Admin, Role.Student };

            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

        }

        private async Task InitializeUsers() {
            const string password = "password";
            await CreateUser("nicklersberghe", password, Role.HeadAdmin);
            await CreateUser("dzhemaptula", password, Role.Admin);
            await CreateUser("tijlzwartjes", password, Role.Admin);
            await CreateUser("jannevschep", password, Role.Admin);
            await CreateUser("josephstalin", password, Role.Student);
            await CreateUser("johncena", password, Role.Student);
        }

        private async Task CreateUser(string username, string password, string role)
        {
            
            var user = new IdentityUser { UserName = username };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded) {
                var createdUser = await userManager.FindByNameAsync(user.UserName);
                await userManager.AddToRoleAsync(createdUser, role);

                //Add java users for admin and above roles, hashed passwords into MD5 so we can access them from db
                if (role != Role.Student)
                {
                    var admins = context.Admins;
                    using (MD5 md5Hash = MD5.Create())
                    {
                        string passwordHash = GetMd5Hash(md5Hash, password);
                        UserRole userRole;
                        if (role == Role.HeadAdmin)
                            userRole = UserRole.HeadAdmin;
                        else
                            userRole = UserRole.Admin;
                        //Console.WriteLine("The password hash is: " + passwordHash);
                        admins.Add(new AdminAuth(username, passwordHash, userRole));
                        context.SaveChanges();
                    }
                }
                
            }
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        private void InitData() {
            var Users = context.Users;
            var Sessions = context.Sessions;
            var Calendars = context.Calendars;

            User us1 = new User("Dzhem", "Aptula", "1122642588674", "dzhem.aptula@gmail.com", UserRole.HeadAdmin);
            User us2 = new User("Nick", "Lersberghe", "1122582892039", "nick.lersberghe@student.hogent.be", UserRole.HeadAdmin);
            Users.Add(us1);
            Users.Add(us2);
            Users.Add(new User("Janne", "Vschep", "1117253563394", "a@a.g", UserRole.Admin));
            Users.Add(new User("Tijl", "Zwartjes", "31254121234123" , "tijl.zwartjes4@gmail.com", UserRole.Admin));
            Users.Add(new User("John", "Cena", "31254121234123", "a1@a.g", UserRole.Student));
            Users.Add(new User("Billie", "Eilish", "31254121234123", "a2@a.g", UserRole.Student));
            Users.Add(new User("Joseph", "Stalin", "31254121234123", "a3@a.g", UserRole.Student));
            Users.Add(new User("Napoleon", "Bonaparte", "31254121234123", "a4@a.g", UserRole.Student));
            Users.Add(new User("Post", "Malone", "31254121234123", "a5@a.g", UserRole.Student));
            Users.Add(new User("Lil", "Pump", "31254121234123", "a6@a.g", UserRole.Student));

            context.SaveChanges();

            var dzhem = Users.Single(s => s.FirstName == "Dzhem");
            var nick = Users.Single(s => s.FirstName == "Nick");
            var janne = Users.Single(s => s.FirstName == "Janne");
            var tijl = Users.Single(s => s.FirstName == "Tijl");

            Session testSesh = new Session(tijl, DateTime.Now, DateTime.Now.AddMinutes(60), "Github", "C1024");

            //uncomment and debug the following 8 lines if you want to add feedbacks, async goes too fast and crashes.
            // testSesh.RegisterUser(us1);
            // testSesh.RegisterUser(us2);
            // testSesh.Open();
            // testSesh.Attend(us1);
            // testSesh.Attend(us2);
            // testSesh.Close();
            // testSesh.AddFeedback(us1, "This session rocked!");
            // testSesh.AddFeedback(us2, "wow...");

            Sessions.Add(new Session(dzhem, DateTime.Now, DateTime.Now.AddHours(1), "Cyber Security", "B2014"));
            Sessions.Add(new Session(nick, DateTime.Now.AddMinutes(30), DateTime.Now.AddMinutes(60), "Relaxing", "D3001"));
            Sessions.Add(testSesh);
            Sessions.Add(new Session(janne, new DateTime(2020, 2, 16, 12, 30, 0), new DateTime(2020, 2, 16, 13, 30, 0), "Learn to learn", "C2034"));
            Sessions.Add(new Session(dzhem, new DateTime(2020, 3, 5, 20, 0, 0), new DateTime(2020, 3, 5, 21, 0, 0), "Explore your inner peace", "B0010"));
            Sessions.Add(new Session(nick, new DateTime(2020, 3, 10, 20, 0, 0), new DateTime(2020, 3, 10, 21, 0, 0), "Anime", "D2014"));
            Sessions.Add(new Session(janne, new DateTime(2020, 3, 1, 15, 0, 0), new DateTime(2020, 3, 1, 18, 0, 0), "Astrology", "B1024"));
            Sessions.Add(new Session(tijl, new DateTime(2020, 1, 25, 20, 0, 0), new DateTime(2020, 1, 25, 21, 0, 0), "Science evening", "C3028"));
            Sessions.Add(new Session(dzhem, new DateTime(2020, 2, 5, 20, 0, 0), new DateTime(2020, 2, 5, 21, 0, 0), "Hacking", "B3004"));
            Sessions.Add(new Session(nick, new DateTime(2020, 2, 15, 20, 0, 0), new DateTime(2020, 2, 15, 21, 0, 0), "Filmography", "D0004"));

            

            DateTime date1 = new DateTime(2019, 8, 1);
            DateTime date2 = new DateTime(2020, 6, 30);
            DateTime date3 = new DateTime(2018, 8, 1);
            DateTime date4 = new DateTime(2029, 6, 30);


            Calendars.Add(new SessionCalendar(date1, date2));
            Calendars.Add(new SessionCalendar(date3, date4));

            context.SaveChanges();
        }
    }
}
