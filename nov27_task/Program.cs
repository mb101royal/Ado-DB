using nov27_task.Helpers;
using nov27_task.Models;
using nov27_task.Services;
using System.Text;

namespace nov27_task
{
    internal class Program
    {
        static void Main(string[] args)
        {
			UserService userService = new UserService();

            userService.Run();

        }
    }
}