using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public enum Tag{
    ONLINE,
    OFFLINE
}
namespace WebApiProject
{
    public class Player{
         public Guid Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
        
        [EnumDataType(typeof(Tag))]
        public Tag Active { get; set; }

        public List<Item> itemList = new List<Item>();
        public DateTime CreationTime { get; set; }

        public int ItemScore{get; set;}

        public bool Nro1{get; set;}

    }
}