
/* <summary>
 * 
 * This ADT uses Depth First Search algorithm on a sequence of tasks that do not violate any task dependencies,
 * then calculates the commencement times of each task based on this sequence.
 * 
 * Invariants: The task collection exists and has a sequence of tasks that do not violate any task dependencies.
 * 
 * </summary> 
 */

public interface IEarliestTimes
{
    /// <summary>
    /// The task collection.
    /// </summary>
    public ITaskCollection Tasks { get; set; }

    /// <summary>
    /// Depth First Search algorithm.
    /// </summary>
    public void DFS(int v, List<int> sorted);

    /// <summary>
    /// Helper function for the Depth First Search algorithm.
    /// </summary>
    public void DFSHelper(int v, bool[] visited, List<int> sorted);

}