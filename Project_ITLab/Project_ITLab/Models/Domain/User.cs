using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project_ITLab.Models.Enums;

namespace Project_ITLab.Models.Domain {
    public class User {
        public int UserId { get; set; }
        public UserStatus Status { get; set; }
        public UserRole UserRole { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardNumber { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Email { get; set; }
        private string _username;


        public string Username {
            get => _username;
            set {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Gebruikersnaam mag niet leeg zijn.");
                value.Trim();
                value.ToLower();
                Regex re = new Regex(@"^[a-z]{1}[a-z._-]{3,49}", RegexOptions.IgnoreCase);
                if (value.Length < 4 || value.Length > 50)
                    throw new ArgumentException("Gebruikersnaam moet tussen 4 en 50 karakters lang zijn.");
                if (!re.IsMatch(value))
                    throw new ArgumentException("Username must start with a letter, can only contains '.', '_' or '-' as special characters and has to be between 4 and 50 (inclusive) long.");

                //#FIX DIE SHIT, ge kun niemeer inloggen
                //#FIX in comments gezet, laten testen met betere regels

                _username = value;
            }
        }

        //public string Description { get; set; }
        //public DateTime BirthDate { get; set; }
        //public string Class { get; set; }
        public int TimesAbsent { get; private set; }

       // public ICollection<Session> Sessions { get; set; }//Leave null, needed to establish many to many relations

        public User(string fName, string lName) : this(fName, lName, "", "", UserRole.Student){
            
        }
        public User(string fName, string lName, string cNumber, string email, UserRole role)
        {
            this.CardNumber = cNumber;
            this.Email = email;
            this.Status = UserStatus.Active;
            this.FirstName = fName;
            this.LastName = lName;
            this.Username = (fName + lName).ToLower();
            this.UserRole = role;
        }

        public User() {
        }

        //more to come
        public bool MakeActive() {
            Status = UserStatus.Active;
            TimesAbsent = 0;
            return true;
        }

        public void IncrementTimesAbsent() {
            TimesAbsent++;
            if (TimesAbsent >= 3)
                Status = UserStatus.Blocked;
        }

        public override bool Equals(object? obj) {
            if (obj == null)
                return false;
            if (!(obj is User))
                return false;
            var user = (User)obj;

            if ((user.Username == null && Username != null) || (user.Username != null && Username == null))
                return false;

            if (user.Username == null && Username == null)//Only used in Test cases where test users are made without a username
                return true;

            return Username.ToLower().Equals(user.Username.ToLower());

        }


        public override int GetHashCode() {
            return (_username != null ? _username.GetHashCode() : 0);
        }

        public String GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }

    //Blocked is unattended to 3 sessions
    //Inactive is 3 failed logins
    public enum UserStatus {
        Active, // = 0
        Inactive, // = 1
        Blocked // = 2
    }

}
