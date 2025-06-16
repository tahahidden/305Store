using _305.Domain.Common;
using _305.Domain.Entity;

namespace _305.Domain.Events;

/// <summary>
/// رویداد ایجاد کاربر جدید
/// </summary>
public class UserCreatedDomainEvent(User user) : IDomainEvent
{
    public User User { get; } = user;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
