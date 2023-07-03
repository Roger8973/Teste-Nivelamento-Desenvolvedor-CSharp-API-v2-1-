using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string ToShortString(this AccountMovement type)
        {
            return type switch
            {
                AccountMovement.Credit => "C",
                AccountMovement.Debit => "D",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }

        public static AccountStatus ForEnum(this int value)
        {
            return value switch
            {  
                0 => AccountStatus.Inactive,
                1 => AccountStatus.Active,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
