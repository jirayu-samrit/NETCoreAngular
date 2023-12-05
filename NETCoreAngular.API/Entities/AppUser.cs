using System.ComponentModel.DataAnnotations;

namespace NETCoreAngular.API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
}
