using Project_ITLab.Models.Domain;
using Project_ITLab.Models.Exceptions;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Project_ITLab_Test {
    public class UserTest {
        private User user = new User();

        [Theory(DisplayName = "Test Username invalid")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        //length username = [4;50] 
        [InlineData("aaa")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        [InlineData("1aaaaa")]
        [InlineData("@aaaa")]
        [InlineData("&aaaa")]
        [InlineData("#aaaa")]
        [InlineData("aa@aa")]
        //only allowed symbols [a-zA-Z-_.]
        //Starts with a letter
        public void TestUsernameInvalidValuesThrowsException(string name) {
            Setup();
            Assert.Throws<ArgumentException>(() => user.Username = name);
        }



        [Theory(DisplayName = "Test Username Valid")]
        [InlineData("aaaa")]
        [InlineData("aaaaaaaaaaaa")]
        [InlineData("aaaaaaaaaaaaaaaaa")]
        [InlineData("nick.lersberghe")]
        [InlineData("nick-lersberghe")]
        [InlineData("nick_lersberghe")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]//50 chars
        public void TestUsernameValidValues(string name) {
            Setup();
            try {
                user.Username = name;
                Assert.Equal(name, user.Username);
            } catch {
                Assert.True(false);
            }
        }


        //[Fact(DisplayName = "Test First or last name doesn't contain any symbols or numbers")]
        //public void TestFirstnamedLastnameNoSymbols() {

        //}

        private void Setup() {
            user = new User();
        }

    }
}