using System;
using System.Collections;
using System.Text.Json;

namespace LifeSim
{
    [System.Serializable]
    public class Player
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Target { get; set; }

        public List<Attribute> Attributes { get; set; }
        public List<Quest> Quests { get; set; }
        public Player(string name) {
            Name = name;
            Level = 1;
            Experience = 0;
            Target = 100;
            Attributes = new List<Attribute>{
                new Attribute("Strength"),
                new Attribute("Dexterity"),
                new Attribute("Intelligence"),
                new Attribute("Constitution"),
                new Attribute("Endurance"),
                new Attribute("Charisma")
            };
            Quests = new List<Quest>();
        }
        public Player() { 
            
        }
        public static Player ReadPlayer(string username)
        {
            string path = username + ".json";
            if (File.Exists(path))
            {
                string data = File.ReadAllText(path);
                return JsonSerializer.Deserialize<Player>(data);
            }
            return null;
        }
        public void AddQuest()
        {
            string name;
            Console.Write("Quest name : ");
            name = Console.ReadLine();
            if (name != null)
            {
                Console.WriteLine("--Attributes--");
                int attrCount = Attributes.Count, exp;
                int[] value = new int[attrCount];
                for (int i = 0; i < attrCount; i++)
                {
                    Console.Write(Attributes[i].Name + "[%] : ");
                    value[i] = Int32.Parse(Console.ReadLine());
                    //Console.WriteLine();
                }
                Dictionary<string, int> dict = new Dictionary<string, int>();
                for (int i = 0; i < attrCount; i++)
                {
                    dict.Add(Attributes[i].Name, value[i]);
                }
                Console.Clear();
                Console.Write("Choose difficulty");
                Thread.Sleep(2000);
                Console.Clear();
                int[] multiplier = { 1, 2, 4, 8 };
                string[] options =
                {
                    "Easy",
                    "Normal",
                    "Hard",
                    "Insane"
                };
                Menu difMenu = new Menu(options);
                difMenu.Update();
                int res = multiplier[difMenu.getResults()];
                exp = Level*res*20;
                Quests.Add(new Quest(name, dict, exp));
            }
        }
        public void FinishQuest(int index)
        {
            if (index < 0 || index >= Quests.Count) return;
            if(Quests.Count == 0) return;
            int l = Attributes.Count;
            for (int i = 0; i < l; i++)
            {
                Attributes[i].Value += Attributes[i].Value*(int)((float)(Quests[index].Attributes[Attributes[i].Name]) / 100f);
                Attributes[i].Value += Level;
            }
            Experience += Quests[index].Experience;
            while(Experience >= Target)
            {
                Level++;
                Experience -= Target;
                Target += 20*Level;
            }
            Quests.RemoveAt(index);
        }
        public void DeleteQuest(int index)
        {
            Quests.RemoveAt(index);
        }
        public string[] GetAttributes()
        {
            int l = Attributes.Count;
            string[] res = new string[l];
            for(int i = 0; i < l; i++)
                res[i] = Attributes[i].Name;
            return res;
        }
        public string[] GetQuests()
        {
            int l = Quests.Count;
            string[] res = new string[l];
            for(int i = 0; i < l; i++)
                res[i] = Quests[i].Name;
            return res;
        }
    }
}
