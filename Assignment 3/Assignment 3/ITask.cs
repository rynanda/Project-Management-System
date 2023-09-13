
/* <summary>
 * 
 * This ADT represents a task.
 *
 * Invariants: ExecutionTime > 0.
 *
 * </summary>
 */

public interface ITask
{
    /// <summary>
    /// Get the ID of the task.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Get the execution time of this task.
    /// </summary>
    public int ExecutionTime { get; set; }

    /// <summary>
    /// Get the list of dependencies of this task.
    /// </summary>
    public List<string>? Dependencies { get; set; }

    public string ToString();
}