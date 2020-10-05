using System.ComponentModel.DataAnnotations;
using System;
  public enum ItemType 
        {
            POTION,
            SHIELD
        }
        
namespace WebApiProject{

    public class Item
    {

        public Guid Id { get; set; }
        public string Name{get; set;}

        [Range(1, 900, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Level { get; set; }

        [EnumDataType(typeof(ItemType))]
        public ItemType Type { get; set; }

        DateTime localDate = DateTime.UtcNow;
        [Present]
        public DateTime CreationTime { get; set; }
      
    }
    public class Present : ValidationAttribute
    {
        public DateTime CreationTime { get; }

        public string GetErrorMessage() => $"Date from the past";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var today = DateTime.UtcNow;
            var today = (DateTime)validationContext.ObjectInstance;

            if (today >= CreationTime)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}