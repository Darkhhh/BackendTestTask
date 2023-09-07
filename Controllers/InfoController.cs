using BackendTestTask.Models.Test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppContext = BackendTestTask.Models.Test.AppContext;

namespace BackendTestTask.Controllers;

[ApiController]
[Route("report/[controller]")]
public class InfoController : ControllerBase
{
    private readonly AppContext _context;

    public InfoController(AppContext context)
    {
        _context = context;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReportAnswer>> GetReportRequest(Guid id)
    {
        var info = await _context.ReportRequestLogs.FirstOrDefaultAsync(log => log.RequestGuid == id);
        if (info is null) return NotFound();

        var progress = GetProgress(info.RequestTime);

        var answer = new ReportAnswer
        {
            Query = id,
            Percent = progress,
            Result = progress == 100 ? GetAnswer(info) : null
        };

        return answer;
    }

    private static int GetProgress(DateTime requestTime)
    {
        var processingTime = ApplicationConfiguration.RequestProcessingTime;
        var delta = DateTime.Now - requestTime;
        var progress = delta.TotalMilliseconds / processingTime;
        if (progress >= 1) return 100;

        return (int) (progress * 100);
    }

    private UserStatisticsAnswer GetAnswer(ReportRequestLog log)
    {
        var count = _context.SignInLogs.Count(signInLog => log.UserId == signInLog.UserId &&
            signInLog.SignInDateTime > log.PeriodFrom && signInLog.SignInDateTime < log.PeriodTo);
        
        return new UserStatisticsAnswer
        {
            CountSignIn = count,
            UserId = log.UserId
        };
    }
}