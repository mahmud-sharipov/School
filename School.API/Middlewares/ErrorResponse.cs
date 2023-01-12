namespace School.API.Middlewares;

internal class ErrorResponse
{
    public ErrorResponse()
    {
    }

    public string Error { get; set; }
    public int Status { get; set; }
    public string Title { get; set; }
}