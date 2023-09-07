using BackendTestTask.Models.Test;
using Microsoft.AspNetCore.Mvc;
using AppContext = BackendTestTask.Models.Test.AppContext;

namespace BackendTestTask.Controllers;

[ApiController]
[Route("report/[controller]")]
public class UserStatisticsController : ControllerBase
{
    private readonly AppContext _context;

    public UserStatisticsController(AppContext context) => _context = context;

    [HttpPost]
    public async Task<ActionResult<Guid>> PostReportRequest(ReportRequest request)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        if (user is null)
            return NotFound($"No such user with id={request.UserId}");
        if (request.PeriodFrom > request.PeriodTo) 
            throw new Exception("Datetime To earlier than datetime From");
        
        var requestGuid = Guid.NewGuid();
        var requestLog = new ReportRequestLog
        {
            PeriodFrom = request.PeriodFrom,
            PeriodTo = request.PeriodTo,
            RequestGuid = requestGuid,
            RequestTime = DateTime.Now,
            UserId = request.UserId
        };
        _context.ReportRequestLogs.Add(requestLog);
        await _context.SaveChangesAsync();

        return requestGuid;
    }
}