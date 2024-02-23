namespace DungeonsOfDoom.Core
{
    class Monster : Characters
    {


        public Monster(string name, int health) : base(name, health)
        {
            //Name = name;
            //Health = health;
            SupplyPack = new List<Item>();
        }
    }
}
