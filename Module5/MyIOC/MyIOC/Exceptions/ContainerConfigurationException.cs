using System;

namespace MyIOC.Exceptions
{
    public class ContainerConfigurationException : Exception
    {
        public ContainerConfigurationException() 
        {
            
        }

        public ContainerConfigurationException(string message)
            : base(message)
        {
            
        }

        public ContainerConfigurationException(string message, Exception innerException) 
            : base(message, innerException)
        {
            
        }

    }
}
