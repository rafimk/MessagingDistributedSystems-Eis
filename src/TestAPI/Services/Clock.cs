using EIS.Shared.Time;

namespace TestAPI.Services;

public class Clock : IClock
{
    public DateTime Current() => DateTime.UtcNow;
}