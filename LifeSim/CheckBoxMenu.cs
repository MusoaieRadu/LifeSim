using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSim
{
    public class CheckBoxMenu
    {
        public string[] List { get; set; }
        private int index = 0;
        private bool[] isChecked;
        public bool[] Checkbox { get { return isChecked; } }
        public CheckBoxMenu(string[] List)
        {
            this.List = List;
            isChecked = new bool[List.Length];
            for (int i = 0; i < List.Length; i++)
                isChecked[i] = false;
            List[0] += "<-";
        }
        private void Display()
        {
            foreach (var item in List)
            {
                Console.WriteLine(item);
            }
        }
        private void Up()
        {
            if (index <= 0) return;
            List[index] = List[index].Substring(0, List[index].Length - 2);
            index--;
            List[index] += "<-";
        }
        private void Down()
        {
            if (index >= List.Length - 1) return;
            List[index] = List[index].Substring(0, List[index].Length - 2);
            index++;
            List[index] += "<-";
        }
        private void UpdateList()
        {
            isChecked[index] = !isChecked[index];
            if (isChecked[index])
            {
                List[index] = List[index].Substring(0, List[index].Length - 2);
                List[index] += " X <-";
            }
            else
            {
                List[index] = List[index].Substring(0, List[index].Length - 5);
                List[index] += "<-";
            }
        }        
        public void Update()
        {
            Display();
            while (true)
            {
                char input = Console.ReadKey().KeyChar;
                if(input == (char)ConsoleKey.Enter) { UpdateList(); }
                if (input == (char)ConsoleKey.Spacebar) return;
                if (input == (char)ConsoleKey.Backspace) { index = -1; return; }
                Console.Clear();
                if (char.ToLower(input) == 'w') Up();
                else if (char.ToLower(input) == 's') Down();
                Display();
            }
        }
    }
}
