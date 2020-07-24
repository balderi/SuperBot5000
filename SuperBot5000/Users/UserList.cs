﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using Discord.Commands;

namespace SuperBot5000.Users
{
    public class UserList
    {
        public List<User> Users { get; set; }

        private UserList()
        {
            Users = new List<User>();
        }

        private static UserList _userList = null;

        public static UserList GetUserList()
        {
            if(_userList == null)
            {
                _userList = LoadList();
            }
            return _userList;
        }

        public User GetUser(SocketCommandContext context)
        {
            try
            {
                return Users.First(x => x.Name == context.User.Mention);
            }
            catch { }

            User user = new User(context);
            Users.Add(user);
            SaveList();
            Console.WriteLine($"Created user {context.User.Mention}.");
            return user;
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
            XmlSerializer serializer = new XmlSerializer(typeof(UserList));
            TextWriter writer = new StreamWriter("users.xml");
            serializer.Serialize(writer, this);
            writer.Close();
        }

        private static UserList LoadList()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserList));
            if(!File.Exists("users.xml"))
            {
                File.Create("users.xml");
                return new UserList();
            }
            FileStream fs = new FileStream("users.xml", FileMode.Open);
            return (UserList)serializer.Deserialize(fs);
        }
    }
}