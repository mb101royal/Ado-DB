using nov27_task.Helpers;
using nov27_task.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace nov27_task.Services
{
	public class UserService
	{
		private readonly BlogService blogService = new BlogService();
		private User loggedInUser;

		public void Run()
		{
			string option;
			do
			{
				Console.WriteLine("1. Register");
				Console.WriteLine("2. Login");
				Console.Write("Select an option: ");
				option = Console.ReadLine();

				switch (option)
				{
					case "1":
						Register();
						break;
					case "2":
						Login();
						break;
					case "0":
                        Console.WriteLine("Quiting..");
                        break;
					default:
						Console.WriteLine("\nInvalid option.");
						break;
				}
			} while (option!="0");
			
		}

		public void Register()
		{
			User newUser = new User();

			Console.Write("Enter Name: ");
			newUser.Name = Console.ReadLine();

			Console.Write("Enter Surname: ");
			newUser.Surname = Console.ReadLine();

			Console.Write("Enter Username: ");
			newUser.Username = Console.ReadLine();

			Console.Write("Enter Password: ");
			string password = Console.ReadLine();

			byte[] salt = GenerateSalt();

			newUser.Password = HashPassword.HashingPass(password, salt);
			
			newUser.Salt = salt;

			int result = RegisterUser(newUser);

			if (result > 0)
			{
				Console.WriteLine("User registered successfully.");
			}
			else
			{
				Console.WriteLine("Failed to register user.");
			}
		}

		private static int RegisterUser(User user)
		{
			string query = $"INSERT INTO Users (Name, Surname, Username, Password) " +
						   $"VALUES (N'{user.Name}', N'{user.Surname}', N'{user.Username}', " +
						   $"N'{user.Password}')";

			return SqlHelper.Exec(query);
		}

		public void Login()
		{
			Console.Write("Enter Username: ");
			string username = Console.ReadLine();

			Console.Write("Enter Password: ");
			string password = Console.ReadLine();

			User user = GetUserByUsername(username);

			if (user != null && CheckPassword(user, password))
			{
				loggedInUser = user;
				Console.WriteLine("You're logged in.");

				BlogMenu();
			}
			else
			{
				Console.WriteLine("Failed to log in.");
			}
		}

		private static bool CheckPassword(User user, string inputPassword)
		{
			byte[] storedSalt = user.Password.Take(32).ToArray();
			byte[] storedHash = user.Password.Skip(32).ToArray();

			byte[] hashedInput = HashPassword.HashingPass(inputPassword, storedSalt);

			return hashedInput.SequenceEqual(storedHash);
		}

		private void BlogMenu()
		{
			while (true)
			{
				Console.WriteLine("\nBlog Menu:");
				Console.WriteLine("1. View All Blogs");
				Console.WriteLine("2. View Blog by ID");
				Console.WriteLine("3. Create Blog");
				Console.WriteLine("4. Logout");

				Console.Write("Select an option: ");
				string option = Console.ReadLine();

				switch (option)
				{
					case "1":
						GetAllBlogs();
						break;
					case "2":
						GetBlogById();
						break;
					case "3":
						CreateBlog();
						break;
					case "4":
						loggedInUser = null;
						Console.WriteLine("Logged out.");
						return;
					default:
						Console.WriteLine("Invalid option.");
						break;
				}
			}
		}

		public void GetAllBlogs()
		{
			ICollection<Blog> blogs = blogService.GetAll();

			foreach (var blog in blogs)
			{
				Console.WriteLine($"Blog Id: {blog.Id}, Title: {blog.Title}, Description: {blog.Description}, UserId: {blog.UserId}");
			}
		}
		
		public void GetBlogById()
		{
			Console.Write("Enter Blog Id: ");
			if (int.TryParse(Console.ReadLine(), out int blogId))
			{
				Blog blog = blogService.GetById(blogId);

				if (blog != null)
				{
					Console.WriteLine($"Blog Id: {blog.Id}, Title: {blog.Title}, Description: {blog.Description}, UserId: {blog.UserId}");
				}
				else
				{
					Console.WriteLine("Blog not found.");
				}
			}
			else
			{
				Console.WriteLine("Invalid input for Blog Id.");
			}
		}

		public void CreateBlog()
		{
			if (loggedInUser == null)
			{
				Console.WriteLine("You must be logged in to create a blog.");
				return;
			}

			Blog newBlog = new Blog();

			Console.Write("Enter Blog Title: ");
			newBlog.Title = Console.ReadLine();

			Console.Write("Enter Blog Description: ");
			newBlog.Description = Console.ReadLine();

			newBlog.UserId = loggedInUser.Id;

			int result = blogService.Create(newBlog);

			if (result > 0)
			{
				Console.WriteLine("Blog created successfully.");
			}
			else
			{
				Console.WriteLine("Failed to create blog.");
			}
		}

		private static byte[] GenerateSalt()
		{
			byte[] salt = new byte[32];
			using (var rng = new RNGCryptoServiceProvider())
			{
				rng.GetBytes(salt);
			}
			return salt;
		}

		private static User GetUserByUsername(string username)
		{
			DataTable dt = SqlHelper.GetDatas($"SELECT * FROM Users WHERE Username = N'{username}'");

			if (dt.Rows.Count > 0)
			{
				DataRow row = dt.Rows[0];
				return new User
				{
					Id = (int)row["Id"],
					Name = (string)row["Name"],
					Surname = (string)row["Surname"],
					Username = (string)row["Username"],
					Password = (byte[])row["Password"]
				};
			}

			return null;
		}
	}
}