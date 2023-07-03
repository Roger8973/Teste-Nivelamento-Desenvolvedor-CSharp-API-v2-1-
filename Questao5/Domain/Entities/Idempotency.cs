namespace Questao5.Domain.Entities
{
    public class Idempotency
    {
        public string IdempotencyId { get; set; }
        public string Request { get; set; }
        public string Result { get; set; }
    }
}
