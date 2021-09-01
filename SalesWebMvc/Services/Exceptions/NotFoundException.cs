using System;


namespace SalesWebMvc.Services.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        //Criar Exception personalizada
        public NotFoundException(string message): base(message)
        {

        }
    }
}
