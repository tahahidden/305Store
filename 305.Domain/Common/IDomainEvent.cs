namespace _305.Domain.Common;

/// <summary>
/// اینترفیس پایه برای رویدادهای دامنه
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// زمان وقوع رویداد
    /// </summary>
    DateTime OccurredOn { get; }
}
