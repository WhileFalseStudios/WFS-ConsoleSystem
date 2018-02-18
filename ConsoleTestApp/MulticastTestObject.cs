using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhileFalseStudios.Console;

namespace ConsoleTestApp
{
    public class MulticastTestObject
    {
        int health;
        string name;
        bool dead;

        public MulticastTestObject(string n, int h)
        {
            name = n;
            health = h;
        }

        public void PrintHealth()
        {
            if (health > 0)
            {
                ConsoleSystem.Log(string.Format("{0}: my health is {1}!", name, health));
            }
            else
            {
                ConsoleSystem.Log(string.Format("{0}: rip, I'm dead!", name));
            }
        }

        public void Damage(int damage)
        {
            health -= damage;
            if (!dead && health <= 0)
            {
                ConsoleSystem.Log(string.Format("{0}: ahhhh!", name));
            }
        }

        public void Register()
        {
            var mch = ConsoleSystem.GetConsoleObject<ConsoleMulticastCommand>("dev_multicast_health");
            mch.Add(PrintHealth);

            var mcd = ConsoleSystem.GetConsoleObject<ConsoleMulticastCommand<int>>("dev_multicast_damage");
            mcd.Add(Damage);

            ConsoleSystem.Log("{0}: Why, hello there!", LogVerbosity.Normal, name);
        }

        public void Unregister()
        {
            var mch = ConsoleSystem.GetConsoleObject<ConsoleMulticastCommand>("dev_multicast_health");
            mch.Remove(PrintHealth);

            var mcd = ConsoleSystem.GetConsoleObject<ConsoleMulticastCommand<int>>("dev_multicast_damage");
            mcd.Remove(Damage);

            ConsoleSystem.Log("{0}: I'm floating off into the voooiid....!", LogVerbosity.Normal, name);
        }
    }
}
