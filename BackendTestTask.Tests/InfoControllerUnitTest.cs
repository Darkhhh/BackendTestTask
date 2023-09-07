using BackendTestTask.Controllers;

namespace BackendTestTask.Tests;

[TestClass]
public class InfoControllerUnitTest
{
    [TestMethod]
    public async Task CorrectInputAndOutput()
    {
        ApplicationConfiguration.RequestProcessingTime = 500;
        var infoController = new InfoController(_context);
        var userStatsController = new UserStatisticsController(_context);

        var t = await userStatsController.PostReportRequest(new ReportRequest
        {
            UserId = 1, 
            PeriodFrom = new DateTime(2023, 9, 7, 14, 26, 0),
            PeriodTo = new DateTime(2023, 9, 7, 16, 26, 0)
        });
        var guid = t.Value;
        
        Thread.Sleep(600);

        var result = await infoController.GetReportRequest(guid);
        if (result.Value is null) throw new NullReferenceException();
        Assert.IsNotNull(result.Value.Result);
        Assert.AreEqual(3, result.Value.Result.CountSignIn);
    }

    [TestMethod]
    public async Task IncorrectInputGuid()
    {
        ApplicationConfiguration.RequestProcessingTime = 15000;
        var controller = new InfoController(_context);

        var result = await controller.GetReportRequest(Guid.NewGuid());
        
        Assert.IsTrue(result.Result is NotFoundResult);
    }

    [TestMethod]
    public async Task RequestWasTooEarly()
    {
        ApplicationConfiguration.RequestProcessingTime = 2000;
        var infoController = new InfoController(_context);
        var userStatsController = new UserStatisticsController(_context);

        var t = await userStatsController.PostReportRequest(new ReportRequest
        {
            UserId = 1, 
            PeriodFrom = new DateTime(2023, 9, 7, 14, 26, 0),
            PeriodTo = new DateTime(2023, 9, 7, 16, 26, 0)
        });
        var guid = t.Value;
        
        Thread.Sleep(1000);

        var result = await infoController.GetReportRequest(guid);
        if (result.Value is null) throw new NullReferenceException();
        Assert.IsTrue(result.Value.Percent < 100);
        Assert.IsNull(result.Value.Result);
    }
    
    private static AppContext _context = null!;
    [ClassInitialize]
    public static void GetContext(TestContext testContext)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppContext>().UseInMemoryDatabase("DefaultDatabase");
        var context = new AppContext(optionsBuilder.Options);
        context.Users.AddRange(
            new UserInfo { Name = "Ivan", Surname = "Petrov" },
            new UserInfo { Name = "Mil", Surname = "Minski" }, 
            new UserInfo { Name = "Bar", Surname = "Barinski" }
        );
        context.SaveChanges();
        context.SignInLogs.AddRange(
            new SignInLog{ UserId = 1, SignInDateTime = new DateTime(2023, 9, 7, 15, 15, 0)},
            new SignInLog{ UserId = 2, SignInDateTime = new DateTime(2023, 9, 7, 15, 26, 0)},
            new SignInLog{ UserId = 1, SignInDateTime = new DateTime(2023, 9, 7, 15, 35, 0)},
            new SignInLog{ UserId = 3, SignInDateTime = new DateTime(2023, 9, 7, 15, 36, 0)},
            new SignInLog{ UserId = 3, SignInDateTime = new DateTime(2023, 9, 7, 15, 42, 0)},
            new SignInLog{ UserId = 1, SignInDateTime = new DateTime(2023, 9, 7, 15, 36, 0)}
            );
        context.SaveChanges();
        
        _context = context;
    }
}