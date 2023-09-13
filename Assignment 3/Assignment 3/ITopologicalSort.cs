
/* <summary>
 * 
 * This ADT applies topological sort to the task collection (Directed Acyclic Graph adjacency list)
 * to find a sequence of tasks that do not violate any task dependencies.
 * 
 * Invariants: The task collection exists and has a sequence of tasks that do not violate any task dependencies.
 * 
 * </summary> 
 */

public interface ITopologicalSort
{
    /// <summary>
    /// Returns the task collection.
    /// </summary>
    public ITaskCollection Tasks { get; }

    /// <summary>
    /// Returns a sequence of tasks that do not violate any task dependencies.
    /// </summary>
    public ITask[] TopSort();
}