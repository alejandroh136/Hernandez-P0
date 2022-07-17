using System;
namespace DungeonCrawler
{
    public class Program{
        static void Main(string[] argv){
            Game game = new Game();
            //Console.WriteLine(argv[0]);
            if(argv.Length > 0 && argv[0] == "zombie"){
                //Console.WriteLine("you became a zombie");
                game.NoDeath = true;
            }
            game.Main();
        }
    }    
    
}