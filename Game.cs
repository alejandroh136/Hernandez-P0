using System;
namespace DungeonCrawler
{
    public class Game{
        OverworldPlayerLocation? playerloc;
        CharacterEntity? Player;
        List<CharacterEntity>? Monsters;
        List<string> BattleLog;

        DungeonWorld? world;
        private bool GameRunning = true;
        public Game(){
            Player = new CharacterEntity("Warrior",50,11,12,5,5,8);
            Monsters = CreateMonsters();
            playerloc = new OverworldPlayerLocation(19,14);//have to remember x represents row and 14 is column
            world = new DungeonWorld();
            BattleLog = new List<string>();
        }
        public void Main(){
            if(world != null && playerloc != null){
                world.SetPlayerLocation(playerloc);
            }
            else{
                return;
            }
            
            while(GameRunning){
                Run();
                Random rand = new Random();
                if(rand.Next(100) % 2 == 0 && GameRunning){
                    Battle();
                }
            }
            return;
        }
        private bool Run(){
            world.PrintMap();
            ConsoleKeyInfo inputkey = Console.ReadKey();
            if(inputkey.Key == ConsoleKey.P)
                PauseMenu();
            else{
                switch(inputkey.Key){
                    case ConsoleKey.LeftArrow:
                    CheckAndMove(playerloc!.mx, playerloc.my-1);
                    break;
                    case ConsoleKey.DownArrow:
                    CheckAndMove(playerloc!.mx+1, playerloc.my);
                    break;
                    case ConsoleKey.UpArrow:
                    CheckAndMove(playerloc!.mx-1, playerloc.my);
                    break;
                    case ConsoleKey.RightArrow:
                    CheckAndMove(playerloc!.mx, playerloc.my+1);
                    break;
                }
            }   
            return true;
        }
        private void CheckAndMove(int x, int y){
            char checkTile = world.GetTileAt(x,y);
            if(checkTile == 'O' && playerloc != null
                && playerloc.mx > 0 && playerloc.mx < world.GetMapHeight() 
                && playerloc.my > 0 && playerloc.my < world.GetMapWidth())
            {
                playerloc.mx = x;
                playerloc.my = y;
                world.UpdateMap();
            }
        }
        private void PauseMenu(){
            System.Console.WriteLine();//add a new line
            char option = '1';
            ConsoleKeyInfo inputkey;
            bool here = true;
            string option1 = "";
            string option2 = "";
            string option3 = "";
            do{
                option1 = "  unpause";
                option2 = "  view stats";
                option3 = "  quit game";
                if(option < '1')
                    option = '1';
                if(option > '3')
                    option = '3';
                switch(option){
                    case '1':
                    option1 = "*" + option1;
                    break;
                    case '2':
                    option2 = "*" + option2;
                    break;
                    case '3':
                    option3 = "*" + option3;
                    break;
                }
                System.Console.Clear();
                System.Console.SetCursorPosition(System.Console.CursorLeft,(System.Console.WindowHeight / 4));
                System.Console.WriteLine(option1);
                System.Console.WriteLine(option2);
                System.Console.WriteLine(option3);
                inputkey = Console.ReadKey();
                if(inputkey.Key == ConsoleKey.DownArrow)
                    option++;
                if(inputkey.Key ==ConsoleKey.UpArrow)
                    option--;
                if(option == '1' && inputkey.Key == ConsoleKey.Enter)
                    here = false;
                if(option == '3' && inputkey.Key == ConsoleKey.Enter){
                    here = false;
                    GameRunning = false;
                    System.Console.Clear();

                }
            }while(here);
        }
        private List<CharacterEntity> CreateMonsters(){
            Random rand = new Random();
            string[] names = {"Warcat","Ghostly","Knocker","Harpy","Spider","Spiked F","Gremlin","Siren","Tiger","Gargoyle","Lemon","Orange","Poisoned G","Boar","Pirate"};
            Monsters = new List<CharacterEntity>();
            for(int i = 0; i< 20; i++){
                Monsters.Add(new CharacterEntity(names[rand.Next(15)],10+rand.Next(4),3+rand.Next(3),2+rand.Next(3),4+rand.Next(3),1+rand.Next(3),6+rand.Next(3)));
            }
            return Monsters;
        }
        private bool Battle(){//prepare for battle
            //get few monsters
            int howmany = 2;
            Random rand = new Random();
            howmany = howmany + (rand.Next(100) % 2);
            /*
            CharacterEntity[,] enemies = new CharacterEntity[3,3];
            if(howmany == 3){
                enemies[0,0] = Monsters[rand.Next(Monsters.Count)];
                enemies[0,1] = Monsters[rand.Next(Monsters.Count)];
                enemies[1,1] = Monsters[rand.Next(Monsters.Count)];
            }
            else{
                enemies[0,1] = Monsters[rand.Next(Monsters.Count)];
                enemies[1,1] = Monsters[rand.Next(Monsters.Count)];
            }
            */
            CharacterEntity[] enemies;
            if(howmany == 3){
                enemies = new CharacterEntity[3];
                enemies[0] = Monsters[rand.Next(Monsters.Count)];
                enemies[1] = Monsters[rand.Next(Monsters.Count)];
                enemies[2] = Monsters[rand.Next(Monsters.Count)];
            }
            else{
                enemies = new CharacterEntity[2];
                enemies[0] = Monsters[rand.Next(Monsters.Count)];
                enemies[1] = Monsters[rand.Next(Monsters.Count)];
                
            }
            System.Console.WriteLine("quick log\nplayer is");
            System.Console.WriteLine(Player!.name);
            for(int i = 0; i < enemies.Count(); i++){
                System.Console.WriteLine(enemies[i].name);
            }
            //bool result = BattleScene(enemies);
            //return false;
            return BattleScene(enemies);
        }
        //private bool BattleLogic(CharacterEntity[,] enemies){//i wanted to battle front line vs back line but decided against that
        private bool BattleScene(CharacterEntity[] enemies){
            List<CharacterEntity> all = new List<CharacterEntity>();
            all.Add(Player!);
            for(int i = 0; i< enemies.Count(); i++){
                all.Add(enemies[i]);
            
            }
            PrintBattleScene(enemies);
            all.Sort();
            bool HaveEnemy = true;
            while(HaveEnemy){
                HaveEnemy = false;
                foreach(CharacterEntity attacker in all){
                    if(attacker.name == "Warrior"){
                        PlayerAttacks(enemies);
                    }
                    else{
                        if(attacker.hp > 0){
                            //System.Console.WriteLine("quick log\nplayer is");
                            System.Console.WriteLine(Player!.name);
                            System.Console.WriteLine(attacker.name);
                            //System.Console.WriteLine("Sdlkfjkl;asd kljf;jal;kdf");
                            HaveEnemy = true;
                            double damage = Math.Floor(attacker.BattleOther(Player, CharacterEntity.AttackType.Physical));
                            bool stillAlive = Player.TakesDamage(damage);
                            //System.Console.WriteLine("damage is " + damage);
                            //System.Console.WriteLine("did I make it here??????");
                            BattleLog.Add("You take battle damage of " + damage.ToString());
                            System.Console.WriteLine(BattleLog[0]);
                            //System.Console.WriteLine("did I make it here??????");
                            if(stillAlive == false){
                                return stillAlive;
                            }
                        }
                    }
                    foreach(var entry in BattleLog){
                        System.Console.WriteLine(entry);
                    }
                    PrintBattleScene(enemies);
                }
            }

            return false;
        }
        private void PrintBattleScene(CharacterEntity[] enemies){
            //for(int i = 0 i < System.Console.WindowWidth; i++)
            System.Console.WriteLine("****************************************");
            System.Console.WriteLine("\nenemy list:");
            for(int i = 0; i< enemies.Count(); i++){
                System.Console.WriteLine(enemies[i].name);
            }
            System.Console.WriteLine("\nVs\n");
            System.Console.WriteLine("You as" + Player!.name);
            System.Console.WriteLine("\n****************************************");
        }
        private void PlayerAttacks(CharacterEntity[] enemies){
            bool HaveEnemy = false;
            foreach(CharacterEntity other in enemies){
                if(other.hp > 0){
                    HaveEnemy = true;
                }
            }
            if(HaveEnemy == false){
                return;
            }
            ConsoleKeyInfo inputkey;
            List<string> listofenemies = new List<string>();
            for(int i = 0; i< enemies.Count(); i++){
                CharacterEntity enemy = enemies[i];
                if(enemy.hp > 0){
                    listofenemies.Add(enemy.name);
                }
            }
            listofenemies[0] = "*  " + listofenemies[0];
            int option = 0;
            System.Console.Clear();
            System.Console.WriteLine("Select enemy to attack");
            for(int i = 0; i < listofenemies.Count; i++){
                System.Console.WriteLine(listofenemies[i]);
            }
            inputkey = Console.ReadKey();
            bool madechoice = false;
            
            while(!madechoice){
                System.Console.Clear();
                System.Console.WriteLine("Select enemy to attack");
                for(int i = 0; i < listofenemies.Count; i++){
                    System.Console.WriteLine(listofenemies[i]);
                }
                if(inputkey.Key == ConsoleKey.Enter){
                    madechoice = true;
                    for(int i = 0; i< enemies.Count(); i++){
                        CharacterEntity enemy = enemies[i];
                        if(enemy.name == listofenemies[i].Substring(3)){
                            double damage = Math.Floor(Player!.BattleOther(enemy, CharacterEntity.AttackType.Physical));
                            BattleLog!.Add(enemy.name + " takes battle damage of " + damage);
                        }
                    }
                }
                if(!madechoice){inputkey = Console.ReadKey();}
                if(inputkey.Key == ConsoleKey.DownArrow){
                    if(option < listofenemies.Count-1){
                        listofenemies[option] = listofenemies[option].Substring(3);
                        option++;
                        listofenemies[option] = "*  " + listofenemies[option];
                        for(int i = 0; i < listofenemies.Count; i++){
                            System.Console.WriteLine(listofenemies[i]);
                        }
                    }
                }
                if(inputkey.Key ==ConsoleKey.UpArrow){
                    if(option > 0){
                        listofenemies[option] = listofenemies[option].Substring(3);
                        option--;
                        listofenemies[option] = "*  " + listofenemies[option];
                        for(int i = 0; i < listofenemies.Count; i++){
                            System.Console.WriteLine(listofenemies[i]);
                        }
                    }
                }
            }
            return;

        }

    }    
    public class OverworldPlayerLocation{
        public enum Move{
            Left, 
            Down,
            Up,
            Right
        }
        public int mx;
        public int my;
        public OverworldPlayerLocation(int x, int y){
            this.mx = x;
            this.my = y;
        }
    }
}