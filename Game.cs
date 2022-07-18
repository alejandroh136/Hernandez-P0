using System;
namespace DungeonCrawler
{
    public class Game{
        OverworldPlayerLocation? playerloc;
        CharacterEntity? Player;
        List<CharacterEntity>? Monsters;
        List<string> BattleLog;
        Random random;

        DungeonWorld? world;
        private bool GameRunning = true;
        public bool NoDeath{get;set;}
        //stall things
        string[] dialog = {" points behind you", " looks at your feet", " laughs at you", " sneezes", " looks at their phone", " takes a nap", " yawns", " prompts you to take a free shot"};
        public Game(){
            random = new Random();
            Player = new CharacterEntity("Warrior",50,11,12,5,5,8);
            Monsters = CreateMonsters();
            playerloc = new OverworldPlayerLocation(19,14);//have to remember x represents row and 14 is column
            world = new DungeonWorld();
            BattleLog = new List<string>();
        }
        public void Main(){
            PrintStartGame();
            if(world != null && playerloc != null){
                world.SetPlayerLocation(playerloc);
            }
            else{
                return;
            }
            
            while(GameRunning || NoDeath){
                Run();
                int randomly = random.Next(100);
                if(randomly < 10 || randomly > 90 ){
                    GameRunning = Battle();
                }
            }
            if(Player.hp < 0){
                System.Console.WriteLine("your hp is " + Player.hp);
                System.Console.WriteLine("you died :(");
            }
            System.Console.WriteLine("Thank you for playing\nGood Bye :)");
            return;
        }
        private bool Run(){
            world!.PrintMap();
            //System.Console.WriteLine("Your HP is " + Player.hp);
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
            char checkTile = world!.GetTileAt(x,y);
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
                if(option == '2' && inputkey.Key == ConsoleKey.Enter)
                    PrintPlayerStats();
                if(option == '3' && inputkey.Key == ConsoleKey.Enter){
                    here = false;
                    GameRunning = false;
                    //I need to also change this
                    NoDeath = false;
                    System.Console.Clear();

                }
            }while(here);
        }
        private List<CharacterEntity> CreateMonsters(){
            //Random random = new Random();
            string[] names = {"Warcat","Ghostly","Knocker","Harpy","Spider","Spiked F","Gremlin","Siren","Tiger","Gargoyle","Lemon","Orange","Poisoned G","Boar","Pirate"};
            Monsters = new List<CharacterEntity>();
            for(int i = 0; i< 20; i++){
                Monsters.Add(new CharacterEntity(names[random.Next(15)],10+random.Next(4),3+random.Next(3),2+random.Next(3),4+random.Next(3),1+random.Next(3),6+random.Next(3)));
            }
            return Monsters;
        }
        private bool Battle(){//prepare for battle
            //get few monsters
            int howmany = 2;
            //Random rand = new Random();
            howmany = howmany + (random.Next(100) % 2);

            CharacterEntity[] enemies;
            if(howmany == 3){
                enemies = new CharacterEntity[3];
                enemies[0] = Monsters![random.Next(Monsters.Count)];
                enemies[1] = Monsters![random.Next(Monsters.Count)];
                enemies[2] = Monsters![random.Next(Monsters.Count)];
            }
            else{
                enemies = new CharacterEntity[2];
                enemies[0] = Monsters![random.Next(Monsters.Count)];
                enemies[1] = Monsters![random.Next(Monsters.Count)];
                
            }
            //my log for checking if it was created properly
            //System.Console.WriteLine("quick log\nplayer is");
            //System.Console.WriteLine(Player!.name);
            //for(int i = 0; i < enemies.Count(); i++){
            //    System.Console.WriteLine(enemies[i].name);
            //}
            //bool result = BattleScene(enemies);
            //return false;
            return BattleScene(enemies);
        }
        //private bool BattleLogic(CharacterEntity[,] enemies){//i wanted to battle front line vs back line but decided against that
        private bool BattleScene(CharacterEntity[] enemies){
            List<CharacterEntity> all = new List<CharacterEntity>();
            all.Add(Player!);
            for(int i = 0; i< enemies.Count(); i++){
                if(enemies[i].hp > 0){
                    all.Add(enemies[i]);
                }
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
                            HaveEnemy = true;
                            int randomly = random.Next(100);
                            if(randomly < 25 || randomly > 75 ){
                                //System.Console.WriteLine("quick log\nplayer is");
                                //System.Console.WriteLine(Player!.name);
                                //System.Console.WriteLine(attacker.name);
                                double damage = Math.Floor(attacker.BattleOther(Player!, CharacterEntity.AttackType.Physical));
                                bool stillAlive = Player!.TakesDamage(damage);
                                BattleLog.Add("You take battle damage of " + damage.ToString());
                                if(stillAlive == false){
                                    return stillAlive;
                                }
                            }
                            else{
                                randomly = random.Next(dialog.Count());
                                BattleLog.Add(attacker.name + dialog[randomly]);
                            }
                            
                        }
                    }
                    foreach(var entry in BattleLog){
                        System.Console.WriteLine(entry);
                    }
                    PrintBattleScene(enemies);
                }
            }

            return true;
        }
        private void PrintBattleScene(CharacterEntity[] enemies){
            //for(int i = 0 i < System.Console.WindowWidth; i++)
            System.Console.WriteLine("****************************************");
            System.Console.WriteLine("\nenemy list:");
            for(int i = 0; i< enemies.Count(); i++){
                System.Console.WriteLine(enemies[i].name);
            }
            System.Console.WriteLine("\nVs\n");
            System.Console.WriteLine("You a " + Player!.name);
            System.Console.WriteLine("\n****************************************");
        }
        private void PrintPlayerStats(){
            System.Console.Clear();
            System.Console.WriteLine("Player stats");
            System.Console.WriteLine("Name: " + Player!.name);
            System.Console.WriteLine("HP: " + Player.hp);
            System.Console.WriteLine("Attack: " + Player.strength);
            System.Console.WriteLine("Defense: " + Player.defense);
            System.Console.WriteLine("Intelligence: " + Player.intelligence);
            System.Console.WriteLine("Resistance: " + Player.resistance);
            System.Console.WriteLine("Resistance: " + Player.speed);
            System.Console.WriteLine("Press enter to continue");
            System.Console.ReadLine();
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
                if(!madechoice){
                    System.Console.Clear();
                    foreach(var entry in BattleLog){
                            System.Console.WriteLine(entry);
                    }
                    System.Console.WriteLine("Select enemy to attack");
                    for(int i = 0; i < listofenemies.Count; i++){
                        System.Console.WriteLine(listofenemies[i]);
                    }
                }
                if(!madechoice){inputkey = Console.ReadKey();}
                switch(inputkey.Key){
                    case ConsoleKey.DownArrow:
                        if(option < listofenemies.Count-1){
                            listofenemies[option] = listofenemies[option].Substring(3);
                            option++;
                            listofenemies[option] = "*  " + listofenemies[option];
                            for(int i = 0; i < listofenemies.Count; i++){
                                System.Console.WriteLine(listofenemies[i]);
                            }
                        }
                    break;
                    case ConsoleKey.UpArrow:
                        if(option > 0){
                            listofenemies[option] = listofenemies[option].Substring(3);
                            option--;
                            listofenemies[option] = "*  " + listofenemies[option];
                            for(int i = 0; i < listofenemies.Count; i++){
                                System.Console.WriteLine(listofenemies[i]);
                            }
                        }
                    break;
                    case ConsoleKey.Enter:
                        madechoice = true;
                        //for(int i = 0; i< enemies.Count(); i++){
                        foreach(CharacterEntity enemy in enemies){
                            //CharacterEntity enemy = enemies[i];
                            if(enemy.name == listofenemies[option].Substring(3) && enemy.hp > 0){
                                double damage = Math.Floor(Player!.BattleOther(enemy, CharacterEntity.AttackType.Physical));
                                enemy.TakesDamage(damage);
                                BattleLog!.Add(enemy.name + " takes battle damage of " + damage);
                            }
                        }
                    break;
                }
                //if(!madechoice){inputkey = Console.ReadKey();}

            }
            return;

        }
        private void PrintStartGame(){
            System.Console.Clear();
            System.Console.SetCursorPosition(System.Console.CursorLeft,(System.Console.WindowHeight / 4));
            System.Console.WriteLine("Welcome to Dungeon Doom");
            System.Console.WriteLine("Press enter to being ...\n\n");
            System.Console.SetCursorPosition(System.Console.BufferWidth-10,(System.Console.CursorTop));
            System.Console.WriteLine("your doom");
            System.Console.ReadLine();
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