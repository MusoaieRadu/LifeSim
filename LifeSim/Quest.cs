using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSim
{
    [System.Serializable]
    public class Quest
    {
        public string Name { get; set; }
        public int Experience { get; set; }
        public Dictionary<string, int> Attributes { get; set; }
        public Quest(string name, Dictionary<string, int> attrb,  int experience)
        {
            Name = name;
            Experience = experience;
            Attributes = attrb;
        }
        public Quest(Quest q)
        {
            Name = q.Name;
            Experience = q.Experience;
            Attributes = q.Attributes;
        }
        public Quest() {
        }
    }
    public class DateQuest : Quest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateQuest(Quest q, DateTime startDate, DateTime endDate) : base(q)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
        }
        public DateQuest() { }
        public DateQuest(DateQuest q)
        {
            StartDate = q.StartDate;
            EndDate = q.EndDate;
            this.Attributes = q.Attributes;
            this.Experience = q.Experience;
            this.Name = q.Name;
        }
    }
    public class HabitQuest : Quest
    {
        public DayOfWeek LoggedDay { get; set; }
        public bool Completed { get; set; }
        public List<DayOfWeek> Days { get; set; }
        public HabitQuest(Quest q , List<DayOfWeek> days) : base(q)
        {
            this.Days = days;
            Completed = false;
            LoggedDay = DateTime.Now.DayOfWeek;
        }
        public HabitQuest() { }
    }
}
