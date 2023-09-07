namespace BackendTestTask.Tests;

[TestClass]
public class CreatingDatabaseUnitTest
{
    private static AppContext _context = null!;
    
    [TestMethod]
    public void CorrectEntitiesCountAfterInitialization()
    {
        Assert.AreEqual(3, _context.Users.Count());
    }
    
    [TestMethod]
    public void CorrectUserAppend()
    {
        _context.Users.Add(new UserInfo
        {
            Name = "Phil", Surname = "Voronin"
        });
        _context.SaveChanges();
        var user = _context.Users.Find(4L);
        Assert.IsNotNull(user);
    }
    
    [TestMethod]
    public void CorrectChangeableDatabaseConditionThroughTests()
    {
        var user = _context.Users.Find(4L);
        Assert.AreEqual(user!.Name, "Phil");
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