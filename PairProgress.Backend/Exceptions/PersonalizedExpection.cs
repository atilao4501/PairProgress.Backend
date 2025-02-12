public class PersonalizedException : Exception
{
    public PersonalizedException(string message) 
        : base(message) { }

    public PersonalizedException(string message, Exception inner) 
        : base(message, inner) { }
}