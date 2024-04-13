using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSim
{
    public abstract class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public List<Attribute> Attributes { get; set; }
    }
}
