using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsOfDoom.Core
{
    internal class Oger : Monster
    {
        public Oger() : base("Oger", 30)
        {

        }

        public override void Attack(Characters opponent)
        {
            opponent.Health -= 5;
        }
    }
}
