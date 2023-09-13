
/* <summary>
 * 
 * This ADT represents a task collection. The collection is in the form of a Directed Acyclic Graph (DAG)
 * for the purposes of topological sorting.
 * 
 * Invariants: Count >= 0 and no duplicates in the job collection.
 * 
 * </summary>
 */

public interface ITaskCollection
{
    /// <summary>
    /// Keeps count of tasks in the collection.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Add vertices (tasks) to the DAG.
    /// </summary>
    public void addVertices(ITask task);

    /// <summary>
    /// Add edges (tasks and their dependencies) to the DAG.
    /// </summary>
    public void addEdges(ITask u, ITask v);

    /// <summary>
    /// Return reference of a task in the collection based on its ID.
    /// </summary>
    public ITask? findTask(string taskId);

    /// <summary>
    /// Returns array of the collection.
    /// </summary>
    public ITask[] ToArray();

    /// <summary>
    /// Returns a vertex in the DAG.
    /// </summary>
    public HashSet<Vertex> V { get; }

    /// <summary>
    /// Returns an edge in the DAG.
    /// </summary>
    public HashSet<Tuple<Vertex, Vertex>> E { get; }

    /// <summary>
    /// Return the string ID of a task with no dependencies.
    /// </summary>
    public string findIndependentTask(ITask[] tasks);

    /// <summary>
    /// Remove a task from the DAG along with its dependencies. Also removes the task from other tasks' dependencies.
    /// </summary>
    public bool removeTask(string taskId);

    /// <summary>
    /// Create the DAG from the collection.
    /// </summary>
    public void createGraph(ITask[] tasks);

    /// <summary>
    /// Change execution time of a task.
    /// </summary>
    public void changeTime(string id, int newTime);

}