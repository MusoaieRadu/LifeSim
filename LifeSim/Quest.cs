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
        public Quest() {
        }
    }
}
