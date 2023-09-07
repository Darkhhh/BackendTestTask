using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTestTask.Models.Test;

public class SignInLog
{
    public long Id { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long UserId { get; set; }
    
    public DateTime SignInDateTime { get; set; }
}