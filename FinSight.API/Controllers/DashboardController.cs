using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    [HttpGet]
    public IActionResult GetStats()
    {
        return Ok(new
        {
            documents = 24,
            queries = 132,
            insights = 58,
            dataSources = 6
        });
    }
}