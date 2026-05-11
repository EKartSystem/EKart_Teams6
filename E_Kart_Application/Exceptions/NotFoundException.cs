namespace E_Kart_Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entity, int id)
        : base($"{entity} with ID {id} was not found.")
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }
}