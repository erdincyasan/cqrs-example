using EY.Cqrs.Common.Queries;

namespace EY.CqrsTestApi.Queries;

public sealed class TestResultQuery : IQuery<string>
{
    public byte SwitchByte { get; set; }
}

public sealed class TestResultQueryHandler : IQueryHandler<TestResultQuery, string>
{
    private readonly ILogger<TestResultQuery> _logger;

    public TestResultQueryHandler(ILogger<TestResultQuery> logger)
    {
        _logger = logger;
    }

    public async Task<string> HandleAsync(TestResultQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Test result is started!");
        await Task.Delay(2000);
        return query.SwitchByte switch
        {
            0 => "Hello",
            1 => "World",
            2 => "Merhaba",
            3 => "Dünya",
            4 => "Bugün",
            5 => "Nasılsın",
            6 => "Umarım",
            7 => "İyisindir",
            _ => "Aradığınız durum bulunamadı!"
        };
    }
}