
using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class CurrentAccount
    {
        public int AccountNumber { get; set; }
        public string Name { get; set; }
        public AccountStatus Status { get; set; }
        public decimal Amount { get; set; }
    }
}
