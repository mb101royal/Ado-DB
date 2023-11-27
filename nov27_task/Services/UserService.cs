using nov27_task.Helpers;
using nov27_task.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace nov27_task.Services
{
    internal class UserService
    {
        public int Register(User data)
        {
            Console.Write("Enter a password: ");
            string password = Console.ReadLine();
            string query = $"INSERT INTO Blogs VALUES (N'{data.Name}', N'{data.Surname}', N'{HashPassword.HashingPass(password, out byte[] salt)}')";
            return SqlHelper.Exec(query);
        }
        public void Login(string username, string password)
        {

        }

        /*ICollection<User> GetAllUsers() {  }*/

    }
}
