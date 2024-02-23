using DungeonsOfDoom.Core;
using System;
using System.ComponentModel.Design;
using System.Text;
using System.Threading.Channels;

namespace DungeonsOfDoom
{
    class Program
    {
        Room[,] rooms;
        Player player;
        bool aktivera=false;
        int counter;
        
        
       

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Play();
        }

        public void Play()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            player = new Player();
            CreateRooms();

              
            do
            {
                Console.Clear();
                DisplayRooms();
                DisplayStats();
                if (AskForMovement())
                {                
                    ExploreRoom();                
                }

            } while (player.IsAlive);

            GameOver();
        }

        void CreateRooms()
        {
            rooms = new Room[20, 5];
            for (int y = 0; y < rooms.GetLength(1); y++)
            {
                for (int x = 0; x < rooms.GetLength(0); x++)
                {
                    rooms[x, y] = new Room();
                    int spawnChance = Random.Shared.Next(1, 100 + 1);
                    if (spawnChance < 5)
                        rooms[x, y].MonsterInRoom = new Skeleton();
                    else if (spawnChance < 10)
                        rooms[x, y].MonsterInRoom = new Oger();
                    else if (spawnChance < 20)
                        rooms[x, y].ItemInRoom = new ItemPotion();
                    else if (spawnChance < 30)
                        rooms[x, y].ItemInRoom = new ItemSword();
                }
            }
        }

        void DisplayRooms()
        {
            for (int y = 0; y < rooms.GetLength(1); y++)
            {
                for (int x = 0; x < rooms.GetLength(0); x++)
                {
                    Room room = rooms[x, y];
                    if (player.X == x && player.Y == y)
                    {

                        Console.Write(player.Health >= Player.MaxHealth / 2 ? "🙂" : "😲");
                    }
                    else if (room.MonsterInRoom is Skeleton )
                        Console.Write("🩻");
                    else if (room.MonsterInRoom is Oger)
                        Console.Write("🧌");

                    else if (room.ItemInRoom != null)
                        Console.Write("📦");
                    else
                        Console.Write("🔹");
                }
                Console.WriteLine();
            }
        }



        void DisplayStats()
        {
            Room playerPosition = rooms[player.X, player.Y];
            Console.WriteLine($"❤️{player.Health}/{Player.MaxHealth}");
            if (playerPosition.MonsterInRoom!=null)
            {
                 Console.WriteLine($"Monster💛: {playerPosition.MonsterInRoom.Health}");

            }
            Console.WriteLine(" ");
            
            Console.WriteLine($"Your current damage: {player.DefaultDamage}");
            player.PotionCount = 0;
            player.SwordCount = 0;
            
            for (int i = 0; i < player.SupplyPack.Count; i++)
            {
                if (player.SupplyPack[i] is ItemSword )
                {
                    player.SwordCount++;
                    
                }
                else if (player.SupplyPack[i] is ItemPotion )
                {
                    player.PotionCount++;
                   
                }
                
            }
            Console.WriteLine(" ");
            Console.WriteLine($"⚔️: {player.SwordCount} Press S To boost Damage for one move");
            
            Console.WriteLine($"🧪: {player.PotionCount}  Press H to heal 5 hp");

            Console.WriteLine(" ");

            Console.WriteLine("🧌 has 30 hp in life and takes 5 hp from you");
            Console.WriteLine("🩻 has 20 hp in life and take 10 hp from you");
        }
        bool AskForMovement()
        {
            int newX = player.X;
            int newY = player.Y;
            bool isValidKey = true;            
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (aktivera == true)
            {
                counter++;
                if (counter == 2) 
                {
                    player.DefaultDamage = 10;
                    counter = 0;
                    aktivera = false;
                }
            }
            switch (keyInfo.Key)
            {
                case ConsoleKey.RightArrow: newX++; break;
                case ConsoleKey.LeftArrow: newX--; break;
                case ConsoleKey.UpArrow: newY--; break;
                case ConsoleKey.DownArrow: newY++; break;
                case ConsoleKey.H: ConsumePotion(); break;
                case ConsoleKey.S: AttackBoost(); break;
                default: isValidKey = false; break;                                      
            }
            
          

            if (isValidKey &&
                newX >= 0 && newX < rooms.GetLength(0) &&
                newY >= 0 && newY < rooms.GetLength(1))
            {
                player.X = newX;
                player.Y = newY;

                return true;
            }
            else
            {
                return false;
            }         
        }

        private void AttackBoost()
        {
            for (int i = 0; i < player.SupplyPack.Count; i++)
            {
                if (player.SupplyPack[i] is ItemSword)
                {
                    player.SupplyPack.RemoveAt(i);
                    player.DefaultDamage = 20;
                    aktivera = true;


                    break;
                }
            }

        }

        private void ConsumePotion()
        {
            if (player.Health<30)
            {
                for (int i = 0; i < player.SupplyPack.Count; i++)
                {
                    if (player.SupplyPack[i] is ItemPotion)
                    {
                        player.SupplyPack.RemoveAt(i);
                        player.Health += 5;
                        break;
                    }
                }
            }
           
        }

        private void ExploreRoom()
        {
            Room playerPosition = rooms[player.X, player.Y];

            if (playerPosition.ItemInRoom != null)
            {
                player.SupplyPack.Add(playerPosition.ItemInRoom);
                playerPosition.ItemInRoom = null;
            }

            if (playerPosition.MonsterInRoom != null)
            {
                player.Attack(playerPosition.MonsterInRoom);
                if (playerPosition.MonsterInRoom.IsAlive)
                {
                    playerPosition.MonsterInRoom.Attack(player);
                }
                else
                {
                    playerPosition.MonsterInRoom = null;
                }
            }
      
        }

        void GameOver()
        {
            Console.Clear();
            Console.WriteLine("Game over...");
            Console.ReadKey();
            Play();
        }
    }
}
