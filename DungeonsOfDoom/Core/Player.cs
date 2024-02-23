using System.Numerics;

namespace DungeonsOfDoom.Core
{
    class Player:Characters
    {
        public const int MaxHealth = 30;
        public int X { get; set; }
        public int Y { get; set; }

        public int PotionCount { get; set; }
        public int SwordCount { get; set; }

        public Player():base("Aladdin",MaxHealth)
        {
            
            SupplyPack = new List<Item>();
        }

       
           
        
        
            
        
    }
}
