namespace BankApp.Implementations
{
    public class UserNotFoundException: Exception
    {
        public UserNotFoundException(string message) : base(message)
        {

        }
    }
}
