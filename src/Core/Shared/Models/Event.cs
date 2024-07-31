namespace DotnetEventSourcing.src.Core.Shared.Models;

public abstract class Event
{
    public abstract string StreamId { get; }
    public DateTime CreatedAt { get; init; }
}