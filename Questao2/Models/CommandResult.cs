using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2.Models
{
    public class CommandResult
    {
        public bool Success { get; }
        public string ErrorMessage { get; }

        public CommandResult(bool success, string errorMessage = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
    }
}
