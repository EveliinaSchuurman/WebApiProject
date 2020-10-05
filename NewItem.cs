using System.Linq;
using System;
using System.Threading.Tasks;

namespace GameWebApi{
    public class NewItem
    {
      
        public string Name { get; set; }
        public int Level { get; set; }
        public ItemType Type { get; set; }
    }
}