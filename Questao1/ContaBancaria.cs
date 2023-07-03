namespace Questao1
{
    public class ContaBancaria : DadosContaBancaria
    {
        private const decimal TAXA_SAQUE = 3.50M;
        public CommandResult CommandResult { get; set;}

        public ContaBancaria(int numero, string titular, decimal depositoInicial)
        {
            Numero = numero;
            Titular = titular;
            Deposito(depositoInicial);
        }

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
        }

        public void Saque(decimal valorSaque)
        {
            Saldo -= valorSaque + TAXA_SAQUE;

            AtualizarMensagem();
        }

        public void Deposito(decimal valorDeposito)
        {
            Saldo += valorDeposito;

            AtualizarMensagem();
        }

        private void AtualizarMensagem()
        {
            CommandResult = new CommandResult($"Conta {Numero}, Titular: {Titular}, Saldo: ${Saldo:N2}");
        }
    }
}

