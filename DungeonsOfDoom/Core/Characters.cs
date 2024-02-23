using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsOfDoom.Core
{
    internal class Characters
    {
        public List<Item> SupplyPack { get; set; }
        public int Health { get; set; }
        public bool IsAlive { get { return Health > 0; } }
        public string Name { get; set; }

        public int DefaultDamage { get; set; }

        public Characters(string name, int health)
        {
            Name = name;
            Health = health;
            DefaultDamage = 10;
        }

        public virtual void Attack(Characters opponent)
        {
            opponent.Health -= DefaultDamage; 
              
        }


    }
}
