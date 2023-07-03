using Questao5.Domain.Enumerators;
using Questao5.Domain.Extensions;

namespace Questao5.Application.Dtos
{
    public class CurrentAccountDto
    {
        public int AccountNumber { get; set; }
        public string Name { get; set; }
        public string DateHour { get; set; }
        private decimal _amount { get; set; }
        public decimal Amount
        {
            get => _amount;
            set
            {
                if (value == 0)
                {
                    return;
                }
                else
                {
                    _amount = MonetaryValueExtensions.RoundDecimal(value, 2);
                }
            }
        }
    }
}
