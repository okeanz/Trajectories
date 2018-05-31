using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace PIT_Server
{
    public static class Database
    {
        static Dictionary<string, Character> _chars;
        public static void Init()
        {
            _chars = new Dictionary<string, Character>();
            Console.WriteLine(Lobby.SpacePorts.Count);
            Lobby.SpacePorts.TryAdd(new Spaceport(0, new Rocket(0)), null);
            Lobby.SpacePorts.TryAdd(new Spaceport(1, new Rocket(2)), null);
            Lobby.SpacePorts.TryAdd(new Spaceport(2, new Rocket(1)), null);

            for (int i = 0; i < 1000; i++)
            {
                _chars[i.ToString()] = new Character("user" + i + "" + i, i.ToString(), i.ToString());
            }
        }
        public static bool Authenticate(string login, string pass)
        {
            Character c;
            try
            {
                _chars.TryGetValue(login, out c);
            }
            catch
            {
                return false;
            }
            if (c != null)
                if (c.Password == pass)
                    return true;
            return false;
        }
        public static Character GetChar(string login)
        {
            return _chars[login];
        }
        public static Character GetCharbyName(string name)
        {
            return _chars.Values.ToList<Character>().Find(new Predicate<Character>(x => x.Name == name));
        }
    }
    public class Character
    {
        public string Name;
        public string Login;
        public string Password;
        public Character(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = password;
        }
        public int[] GenerateLevels()
        {
            int[] lvls = new int[6];
            return lvls;
        }
    }
}
