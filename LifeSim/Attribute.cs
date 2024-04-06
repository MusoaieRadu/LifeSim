using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LifeSim
{
    public enum ATTRIBUTE
    {
        STRENGTH,
        DEXTERITY,
        INTELLIGENCE,
        CONSTITUTION,
        ENDURANCE,
        CHARISMA
    }
    [System.Serializable]
    public class Attribute
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public Attribute(string name) {
            Name = name;
            Value = 1;
        }
        public Attribute() { }
    }
}
