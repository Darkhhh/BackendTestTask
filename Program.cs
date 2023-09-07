using BackendTestTask;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AppContext = BackendTestTask.Models.Test.AppContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();


builder.Services.AddDbContext<AppContext>(
    options => options.UseInMemoryDatabase("DefaultDatabase"));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var r = new StreamReader("config.json"))
{
    var temp = JsonConvert.DeserializeObject(r.ReadToEnd()) ?? throw new NullReferenceException();
    var data = (JObject)temp;
    if (data is null) throw new Exception("Can not read JSON file");
    ApplicationConfiguration.RequestProcessingTime = (data.SelectToken("ApplicationSettings.RequestProcessingTime") 
                                           ?? throw new NullReferenceException()
                                           ).Value<int>();
}

app.Run();