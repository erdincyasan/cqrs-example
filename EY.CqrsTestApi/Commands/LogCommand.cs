namespace EY.CqrsTestApi.Commands;

using System.Threading;
using System.Threading.Tasks;
using EY.Cqrs.Common.Commands;
public sealed class LogCommand : ICommand
{

}

public sealed class LogCommandHandler : ICommandHandler<LogCommand>
{

    private readonly ILogger<LogCommandHandler> _logger;

    public LogCommandHandler(ILogger<LogCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(LogCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Logging command is works!");
    }

}