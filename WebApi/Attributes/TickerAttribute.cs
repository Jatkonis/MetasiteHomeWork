using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TradingPlaces.WebApi.Attributes
{
    public class TickerAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string ticker = value as string;

            if (ticker == null)
            {
                return false;
            } 
            
            if (ticker.Any(char.IsLower))
            {
                return false;
            }
            if (ticker.Length < 3 || ticker.Length > 5)
            {
                return false;
            }
            if (!ticker.Any(char.IsLetter))
            {
                return false;
            }

            return true;
        }
    }
}
