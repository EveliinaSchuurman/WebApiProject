using System.ComponentModel.DataAnnotations;
using System;
using FluentValidation;
  public enum SwordTypes 
        {
            regular,
            Poison
            
        }
        
namespace WebApiProject{

    public class Sword : Item
    {
        [Required]
        public int OwnerLevel{get; set;}

        [EnumDataType(typeof(SwordTypes))]
        public SwordTypes SwordType { get; set; }

        public int Damage{get; set;}
      
    }
/* 
    public class Allowed : ValidationAttribute
    {

        public string GetErrorMessage() => $"Noob";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var today = DateTime.UtcNow;
            public int HighEnough{get; set;}

            public Allowed(int highEnough){
                HighEnough = highEnough;
            }
            

            if (Allowed <= )
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }*/

    //if the players level is too low, no poisontype
    public class LevelValidator: AbstractValidator<Sword>{
        public LevelValidator(){
            RuleFor(x=>x.OwnerLevel).GreaterThan(15).When(x=>x.SwordType == SwordTypes.Poison);
        }
    }
} 