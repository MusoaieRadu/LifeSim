using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LifeSim
{
    public static class DataHandler
    {
        public static void Save(Player p)
        {
            if (p == null) return;
            string data, path = p.Name + ".json";
            data = JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true }); ;
            File.WriteAllText(path, data);
        }
    }
}
