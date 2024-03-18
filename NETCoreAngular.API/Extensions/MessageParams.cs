using NETCoreAngular.API.Heplers;

namespace NETCoreAngular.API.Extensions;
public class MessageParams : PaginationParams
{
    public string Username { get; set; }
    public string Container { get; set; } = "Unread";
}