using System;
using System.Collections;
using System.Text.Json;

namespace LifeSim
{
    [System.Serializable]
    public class Player : Character
    {
        public int Experience { get; set; }
        public int Target { get; set; }

        public List<DateQuest> DateQuests { get; set; }
        public List<HabitQuest> HabitQuests { get; set; }
        private int[] multiplier = { 1, 2, 4, 8 };
        private int[] MAIN_multiplier = { 4, 8, 12, 18 };
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
            DateQuests = new List<DateQuest>();
            HabitQuests = new List<HabitQuest>();
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
                Thread.Sleep(1000);
                Console.Clear();
                string[] options =
                {
                    "Easy",
                    "Normal",
                    "Hard",
                    "Insane"
                };
                Menu difMenu = new Menu(options);
                difMenu.Update();
                int res = difMenu.getResults();
                options = new string[]
                {
                    "Today's quest",
                    "Due by quest",
                    "Habit quest"
                };
                Console.Clear();
                Console.WriteLine("Type of quest?");
                Thread.Sleep(1000);
                Console.Clear();
                difMenu = new Menu(options);
                difMenu.Update();
                int c = difMenu.getResults();
                Quest q;
                if (c == 0)
                {
                    exp = Level * multiplier[res] * 20;
                    q = new Quest(name, dict, exp);
                    DateTime now = DateTime.Now;
                    DateQuest d = new DateQuest(q, now, now);
                    DateQuests.Add(d);
                }
                else if(c == 1)
                {
                    int day, month, year;
                    Console.Clear();
                    Console.Write("Start day : ");
                    day = Int32.Parse(Console.ReadLine());
                    Console.Write("Start month : ");
                    month = Int32.Parse(Console.ReadLine());
                    Console.Write("Start year : ");
                    year = Int32.Parse(Console.ReadLine());
                    DateTime start = new DateTime(year, month, day);
                    Console.WriteLine("---------------");
                    Console.Write("Due day : ");
                    day = Int32.Parse(Console.ReadLine());
                    Console.Write("Due month : ");
                    month = Int32.Parse(Console.ReadLine());
                    Console.Write("Due year : ");
                    year = Int32.Parse(Console.ReadLine());
                    DateTime end = new DateTime(year, month, day);
                    exp = 20 * Level * MAIN_multiplier[res];
                    q = new Quest(name, dict, exp);
                    DateQuest d = new DateQuest(q, start, end);
                    DateQuests.Add(d);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Check the days!");
                    Thread.Sleep(1500);
                    Console.Clear();
                    options = new string[]{
                        "Sunday",
                        "Monday",
                        "Tuesday",
                        "Wednesday",
                        "Thursday",
                        "Friday",
                        "Saturday"
                     };
                    CheckBoxMenu check = new CheckBoxMenu(options);
                    List<DayOfWeek> days = new List<DayOfWeek>();
                    check.Update();
                    int l = check.Checkbox.Length;
                    for(int i = 0; i < l; i++)
                    {
                        if (check.Checkbox[i])
                            days.Add((DayOfWeek)i);
                    }
                    exp = 20 * Level * multiplier[res];
                    q = new Quest(name, dict, exp);
                    HabitQuest h = new HabitQuest(q, days);
                    HabitQuests.Add(h);
                }
            }
        }
        public void FinishQuest(int index)
        {
            int l = Attributes.Count;
            if (index < DateQuests.Count)
            {
                if (DateTime.Now.Date >= DateQuests[index].StartDate.Date)
                {
                    for (int i = 0; i < l; i++)
                    {
                        Attributes[i].Value += Attributes[i].Value * (int)((float)(DateQuests[index].Attributes[Attributes[i].Name]) / 100f);
                        Attributes[i].Value += Level;
                    }
                    Experience += DateQuests[index].Experience;
                    while (Experience >= Target)
                    {
                        Level++;
                        Experience -= Target;
                        Target += 20 * Level;
                    }
                    DateQuests.RemoveAt(index);
                }
            }
            else
            {
                int off = DateQuests.Count;
                bool ok = false;
                foreach (DayOfWeek d in HabitQuests[index - off].Days)
                    if (d == DateTime.Now.DayOfWeek) ok = true;
                if (!HabitQuests[index - off].Completed && ok)
                {
                    for (int i = 0; i < l; i++)
                    {
                        Attributes[i].Value += Attributes[i].Value * (int)((float)(HabitQuests[index - off].Attributes[Attributes[i].Name]) / 100f);
                        Attributes[i].Value += Level;
                    }
                    Experience += HabitQuests[index - off].Experience;
                    while (Experience >= Target)
                    {
                        Level++;
                        Experience -= Target;
                        Target += 20 * Level;
                    }
                    HabitQuests[index - off].Completed = true;
                    HabitQuests[index - off].LoggedDay = DateTime.Now.DayOfWeek;
                    int val = HabitQuests[index - off].Experience;
                    HabitQuests[index - off].Experience += (int)(val * 0.05);
                }
            }
            foreach(DateQuest q in DateQuests)
            {
                q.Experience += (int)(q.Experience * 0.05);
            }
        }
        public void DeleteQuest(int index)
        {
            if (index < 0 || index > HabitQuests.Count + DateQuests.Count) return;
            if (index < DateQuests.Count) { DateQuests.RemoveAt(index); return; }
            HabitQuests.RemoveAt(index - DateQuests.Count);
        }
        public string[] GetAttributes()
        {
            int l = Attributes.Count;
            string[] res = new string[l];
            for(int i = 0; i < l; i++)
                res[i] = Attributes[i].Name;
            return res;
        }
        public string[] GetHabits()
        {
            int l = HabitQuests.Count;
            string[] res = new string[l];
            for(int i = 0; i < l; i++)
                res[i] = HabitQuests[i].Name;
            return res;
        }
        public string[] GetDueQuests()
        {
            int l = DateQuests.Count;
            string[] res = new string[l];
            for (int i = 0; i < l; i++)
                res[i] = DateQuests[i].Name;
            return res;
        }
        public void DisplayHabits()
        {
            string[] options = new string[]{
                        "Sunday",
                        "Monday",
                        "Tuesday",
                        "Wednesday",
                        "Thursday",
                        "Friday",
                        "Saturday"
                     };
            List<string>[] schedule = new List<string>[7];
            for (int i = 0; i < 7; i++)
                schedule[i] = new List<string>();
            foreach(HabitQuest h in HabitQuests)
            {
                foreach (DayOfWeek day in h.Days)
                    schedule[(int)day].Add(h.Name);
            }
            int l = 20, l1, l2;
            for(int i = 0; i < 7; i++)
            {
                l1 = options[i].Length;
                l2 = ((l - l1 + 2) / 2) - 1;
                for (int k = 0; k < l; k++)
                    Console.Write('-');
                Console.WriteLine();
                Console.Write('|');
                for (int k = 0; k < l2-1; k++)
                    Console.Write(' ');
                Console.Write(options[i]);
                for (int k = 0; k < l2; k++)
                    Console.Write(' ');
                Console.WriteLine('|');
                for (int k = 0; k < l; k++)
                    Console.Write('-');
                Console.WriteLine();
                foreach (string s in schedule[i])
                {
                    Console.WriteLine(s);
                }
            }
        }
    }
}
