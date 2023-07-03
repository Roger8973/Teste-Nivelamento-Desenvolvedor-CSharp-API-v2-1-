namespace Questao1
{
    public class CommandResult
    {
        public string Mensagem { get; private set; }

        public CommandResult(string mensagem)
        {
            Mensagem = mensagem;
        }
    }
}
