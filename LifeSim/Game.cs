using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LifeSim
{
    public delegate void DisplayDelegate();
    public delegate void QuestDelegate(int x);
    public class Game
    {
        Player player;
        ProgressBar playerBar;
        private int barDivision = 100;
        public Game() {
        }
        public void Start()
        {
            Console.Clear();
            //Login or new account
            int reg = Registration();
            while (reg == -1) { Console.Clear(); reg = Registration(); }
            if (reg == 0)
                CreateAccount();
            else Login();
        Console.Clear();
        //Now we are at a point where the user has entered
        //everything to proceed
        PlayerChoice();
    }
        private int Registration()
        {
            string[] s = new string[] {"Create new account",
                "Login"
            };
            Menu reg_menu = new Menu(s);
            reg_menu.Update();
            return reg_menu.getResults();
        }
        private void CreateAccount()
        {
            Console.Clear();
            string name, filepath;
            Console.Write("Name : ");
            name = Console.ReadLine();
            filepath = name + ".json";
            while(File.Exists(filepath))
            {
                Console.WriteLine("Account already exists!");
                Thread.Sleep(1000);
                Console.Clear();
                Console.Write("Name : ");
                if(Console.ReadKey().KeyChar == (char)ConsoleKey.Backspace) { Start(); return; }
                name = Console.ReadLine();
                filepath = name + ".json";
            }
            player = new Player(name);
            DataHandler.Save(player);
        }
        private void Login()
        {
            Console.Clear();
            Console.Write("Username : ");
            string name = Console.ReadLine();
            string filepath = name + ".json";
            while(!File.Exists(filepath))
            {
                Console.WriteLine("Username doesn't exist.");
                Thread.Sleep(1000);
                Console.Clear();
                Console.Write("Username : ");
                if (Console.ReadKey().KeyChar == (char)ConsoleKey.Backspace) { Start(); return; }
                name = Console.ReadLine();
                filepath = name + ".json";
            }
            player = Player.ReadPlayer(name);
        }
        private void PlayerData()
        {
            Console.WriteLine("Name : " + player.Name);
            Console.WriteLine("Level : " + player.Level);
            Console.WriteLine("---------------------------");
            for(int i = 0; i < player.Attributes.Count; i++)
            {
                Console.WriteLine(player.Attributes[i].Name + " : " + player.Attributes[i].Value);
            }
            Console.WriteLine("---------------------------");
            playerBar = new ProgressBar(player.Target, player.Experience, barDivision);
            Console.WriteLine("Experience : " + player.Experience + "/" + player.Target);
            playerBar.Display();
        }
        private void QuestData()
        {
            foreach(Quest q in player.Quests)
            {
                Console.WriteLine("Quest name : " + q.Name);
                Console.WriteLine("XP : " + q.Experience);
                Console.WriteLine("-----Rewards------");
                int max = 0;
                foreach(string attrb in q.Attributes.Keys)
                {
                    if (attrb.Length > max) max = attrb.Length;
                }
                foreach(string attrb in q.Attributes.Keys)
                {
                    if (q.Attributes[attrb] > 0)
                        Console.Write(attrb);
                    for (int i = attrb.Length; i < max; i++)
                        Console.Write(' ');
                    Console.WriteLine(" : +" + q.Attributes[attrb] + "%");
                }
                Console.WriteLine("---------------------");
            }
        }
        private void DisplayData(DisplayDelegate d)
        {
            char key = (char)0;
            while(key != (char)ConsoleKey.Backspace)
            {
                d();
                key = Console.ReadKey().KeyChar;
                Console.Clear();
            }
            //Go back
            PlayerChoice();
        }
        private void AddQuest()
        {
            player.AddQuest();
            Console.Clear();
            PlayerChoice();
        }
        private void HandleQuest(QuestDelegate q)
        {
            if (player.GetQuests().Length > 0)
            {
                Menu quests = new Menu(player.GetQuests());
                quests.Update();
                int r = quests.getResults();
                if (r == -1) { PlayerChoice(); return; }
                q(r);
                Console.Clear();
            }
            PlayerChoice();
        }
        private void Exit()
        {
            DataHandler.Save(player);
            System.Environment.Exit(0);
        }
        public void PlayerChoice() {
            string[] src = new string[]
            {
                "View character",
                "View quests",
                "Add quest",
                "Finish quest",
                "Quit quest",
                "Save and Exit"
            };
            Menu menu = new Menu(src);
            menu.Update();
            int r = menu.getResults();
            Console.Clear();
            switch (r)
            {
                case 0: DisplayData(PlayerData); break;
                case 1: DisplayData(QuestData); break;
                case 2: AddQuest(); break;
                case 3: HandleQuest(player.FinishQuest); break;
                case 4: HandleQuest(player.DeleteQuest); break;
                case 5: Exit(); break;
                case -1: Start(); break;
            }
        }
    }
}
