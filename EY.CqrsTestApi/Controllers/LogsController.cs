using EY.Cqrs.Common;
using EY.CqrsTestApi.Commands;
using EY.CqrsTestApi.Queries;
using Microsoft.AspNetCore.Mvc;

namespace EY.CqrsTestApi.Controllers;

[Route("api/[controller]")]
public class LogsController : ControllerBase
{

    private Dispatcher _dispatcher => HttpContext.RequestServices.GetRequiredService<Dispatcher>();

    [HttpPost]
    public async Task PostLog(CancellationToken cancellationToken)
    {
        await _dispatcher.DispatchAsync(new LogCommand(), cancellationToken);
    }

    [HttpGet]
    public async Task<string> Get(TestResultQuery query, CancellationToken cancellationToken) => await _dispatcher.DispatchAsync(query, cancellationToken);
}