using System.Net;

namespace RoboticArmSim.Models;

public class ApiResponse<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public List<string> ErrorMessages { get; set; } = new();
    public T? Data { get; set; }
}