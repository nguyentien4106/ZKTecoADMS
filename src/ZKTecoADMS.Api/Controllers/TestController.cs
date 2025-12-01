namespace ZKTecoADMS.Api.Controllers;

using System.Text;
using Microsoft.AspNetCore.Mvc;
using ZKTecoADMS.API.Controllers;
using MediatR;

[ApiController]
[Route("api/[controller]")]
public class TestController(
    ILogger<ClockController> logger,
    IMediator bus
) : ControllerBase
{
    [HttpPost("fake-attendance")]
    public async Task<IActionResult> FakeAttendances()
    {
        var bodyContent = $"1\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\t1\t1\t0";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(bodyContent));
        
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Body = stream;
        httpContext.Request.ContentLength = stream.Length;

        var controller = new ClockController(logger, bus)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        return await controller.PostCData(
            "1313240800160",
            "ATTLOG",
            DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
        );
    }
}