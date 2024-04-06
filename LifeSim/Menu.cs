namespace LifeSim
{
    public class Menu
    {
        public string[] List { get; set; }
        public int index = 0;
        public Menu(string[] list)
        {
            List = list;
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
        public void Update()
        {
            Display();
            while (true)
            {
                char input = Console.ReadKey().KeyChar;
                if (input == (char)ConsoleKey.Enter) return;
                if (input == (char)ConsoleKey.Backspace) { index = -1; return; }
                Console.Clear();
                if (char.ToLower(input) == 'w') Up();
                else if (char.ToLower(input) == 's') Down();
                Display();
            }
        }
        public int getResults() { return index; }
    }
}
