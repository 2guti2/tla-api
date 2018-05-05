namespace TeLoArreglo.Application.Dtos.Error
{
    public class ErrorDto
    {
        public string Message { get; set; }

        public ErrorDto(string message)
        {
            Message = message;
        }
    }
}
