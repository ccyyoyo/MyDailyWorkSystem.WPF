namespace MyDailyWorkSystem.Domain.Models
{
    public enum TaskState
    {
        Unknown = 0,
        Todo = 1,
        InProgress = 2,
        Done = 3,
        Blocked = 4,
        Paused = 5
    }
}
