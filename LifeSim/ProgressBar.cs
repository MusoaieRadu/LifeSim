using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSim
{
    public class ProgressBar
    {
        private int target, current;
        private int divisions, length;
        public ProgressBar(int target, int current, int divisions)
        {
            this.target = target;
            this.current = current;
            this.divisions = divisions;
        }
        public void Display()
        {
            //upper bar
            for(int i = 0; i < divisions+2; i++)
            {
                Console.Write('-');
            }
            //Content
            Console.Write("\n|");
            int offset = target / divisions, j = 0;
            for (; j*offset <= current; j++) ;
            j--;
            for (int i = 0; i < j; i++)
                Console.Write('#');
            for (int i = j; i < divisions; i++)
                Console.Write(' ');
            Console.Write("|\n");
            //down bar
            for (int i = 0; i < divisions + 2; i++)
                Console.Write('-');
        }
    }
}
