using BackendTestTask.Controllers;

namespace BackendTestTask.Tests;

[TestClass]
public class ReportRequestUnitTests
{
    private static AppContext _context = null!;
    
    [TestMethod]
    public void ReportRequestCorrectInput()
    {
        var userStatsController = new UserStatisticsController(_context);
        var result = userStatsController.PostReportRequest(new ReportRequest
        {
            UserId = 2,
            PeriodFrom = new DateTime(2023, 9, 7, 14, 46, 0),
            PeriodTo = new DateTime(2023, 9, 7, 15, 46, 0)
        });
        Assert.AreEqual(typeof(ActionResult<Guid>), result.Result.GetType());
        
        Assert.AreEqual(1, _context.ReportRequestLogs.Count());
    }
    
    [TestMethod]
    public async Task ReportRequestIncorrectUserIdInput()
    {
        var userStatsController = new UserStatisticsController(_context);
        var result = await userStatsController.PostReportRequest(new ReportRequest
        {
            UserId = 5,
            PeriodFrom = new DateTime(2023, 9, 7, 14, 46, 0),
            PeriodTo = new DateTime(2023, 9, 7, 15, 46, 0)
        });
        Assert.IsTrue(result.Result is NotFoundObjectResult);
    }
    
    [TestMethod]
    public async Task ReportRequestIncorrectDataInput()
    {
        var userStatsController = new UserStatisticsController(_context);
        await Assert.ThrowsExceptionAsync<Exception>(() => userStatsController.PostReportRequest(new ReportRequest
        {
            UserId = 2,
            PeriodFrom = new DateTime(2023, 9, 7, 14, 46, 0),
            PeriodTo = new DateTime(2023, 9, 7, 13, 46, 0)
        }));
    }

    [ClassInitialize]
    public static void GetContext(TestContext testContext)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppContext>().UseInMemoryDatabase("DefaultDatabase");
        var context = new AppContext(optionsBuilder.Options);
        context.Users.AddRange(new UserInfo { Name = "Ivan", Surname = "Petrov" },
            new UserInfo { Name = "Mil", Surname = "Minski" }, 
            new UserInfo { Name = "Bar", Surname = "Barinski" }
            );
        context.SaveChanges();

        _context = context;
    }
}