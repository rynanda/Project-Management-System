
public class TopologicalSort : ITopologicalSort
{
    public ITaskCollection Tasks { get; set; }
    
    public TopologicalSort(ITaskCollection tasks)
    {
        Tasks = tasks;
    }

    public ITask[] TopSort()
    {
        ITaskCollection copy = Tasks;
        ITask[] sorted = new ITask[Tasks.Count];
        int sortedCount = 0;

        while (copy.Count != 0)
        {
            ITask[] tasks = copy.ToArray();
            string independentId = copy.findIndependentTask(tasks);
            ITask? independentTask = copy.findTask(independentId);
            sorted[sortedCount] = independentTask;
            sortedCount++;
            copy.removeTask(independentId);
        }
        return sorted;
    }
}
