using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterMe.TestConsole
{
    class Program
    {
        class User
        {
            public string Username { get; set; }
            public int Age { get; set; }
        }

        private static List<User> users; 

        static void Main(string[] args)
        {
            users = new List<User>();

            for (int i = 0; i < 100; i++)
            {
                users.Add(new User()
                {
                    Age = i,
                    Username = "TestUser" + i
                });
            }

            FilterContext.MapType<User>(cfg =>
            {
                cfg.FilterForProperty(e => e.Username).StringContains();
                cfg.FilterForProperty(e => e.Age).IntLessThanOrEqual();
            });

            var filterItems = new Dictionary<string, object>()
            {
                {"Username", "Test" },
                {"Age", 60 }
            };

            var filter = FilterContext.GetFilter<User>(filterItems);

            var filteredUsers = users.Where(filter.Compile()).ToList();

            Console.ReadKey(true);
        }
    }
}
