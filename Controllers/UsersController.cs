using BackendTestTask.Models.Test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppContext = BackendTestTask.Models.Test.AppContext;

namespace BackendTestTask.Controllers;

[ApiController]
[Route("report/[controller]/users_info")]
public class UsersController : ControllerBase
{
    private readonly AppContext _context;

    public UsersController(AppContext context) => _context = context;
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsers() => await _context.Users.ToListAsync();
    
    [HttpGet("/user/{id:long}")]
    public async Task<ActionResult<UserInfo>> GetUser(long id)
    {
        var user = await _context.Users.FindAsync(id);
        return user is null ? NotFound() : user;
    }
    
    [HttpPost]
    public async Task<ActionResult<UserInfo>> PostUser(UserInfo user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }
    
    
    [HttpPost("post_user_sign_in")]
    public async Task<ActionResult<DateTime>> PostUserSignIn(long id)
    {
        var item = new SignInLog { UserId = id, SignInDateTime = DateTime.Now };
        _context.SignInLogs.Add(item);
        await _context.SaveChangesAsync();
        return item.SignInDateTime;
    }
}