using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord.WebSocket;
using System.Text.Json;

namespace SuperBot5000.Users
{
    public class UserList
    {
        public List<User> Users { get; set; } = new();

        private UserList()
        {
            Users = LoadList();
        }

        private static UserList _userList = null;

        public static UserList GetUserList()
        {
            if(_userList == null)
            {
                _userList = new UserList();
            }
            return _userList;
        }

        public User GetUser(SocketUser user)
        {
            try
            {
                return Users.First(x => x.Name == user.Mention);
            }
            catch
            {
                User u = new(user);
                Users.Add(u);
                SaveList();
                Console.WriteLine($"Created user {user.Username}.");
                return u;
            }
        }

        public bool TryGetUserByName(string name, out User user)
        {
            try
            {
                user = Users.First(x => x.Name == name);
                return true;
            }
            catch
            {
                user = null;
                return false;
            }
        }

        public int GetNumberOfUsers()
        {
            return Users.Count;
        }

        public void SaveList()
        {
            var jsonString = JsonSerializer.Serialize(Users);
            TextWriter writer = new StreamWriter("users.json");
            writer.Write(jsonString);
            writer.Close();
        }

        private static List<User> LoadList()
        {
            if(!File.Exists("users.json"))
            {
                return new List<User>();
            }

            StreamReader reader = new("users.json");

            var jsonString = reader.ReadToEnd();
            reader.Close();

            var userList = JsonSerializer.Deserialize<List<User>>(jsonString);

            return userList;
        }
    }
}
