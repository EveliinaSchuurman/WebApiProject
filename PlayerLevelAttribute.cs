using System.ComponentModel.DataAnnotations;

namespace GameWebApi{

    public class PlayerLevelAttribute : ValidationAttribute {

        public PlayerLevelAttribute(int level)
        {
            Level = level;
        }

        public int Level { get; }

        public string GetErrorMessage() =>
            $"Player must have lever higher than {Level} to use sword.";

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            Player playe = (Player)validationContext.ObjectInstance;

            if (playe.Level < Level)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}