namespace Questao5.Domain.Extensions
{
    public static class MonetaryValueExtensions
    {
        public static decimal RoundDecimal(decimal amount, int precision)
            => Math.Round(amount, precision, MidpointRounding.AwayFromZero);
    }
}
