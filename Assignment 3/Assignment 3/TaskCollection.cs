
public class Vertex
{
    private ITask task;
    public Vertex(ITask task)
    {
        this.task = task;
    }
}

public class TaskCollection : ITaskCollection
{
    private Vertex? vertex;
    private Vertex? prerequisite;
    private HashSet<Vertex> v = new HashSet<Vertex>();
    private HashSet<Tuple<Vertex, Vertex>> e = new HashSet<Tuple<Vertex, Vertex>>();
    private int count;
    private ITask[] tasks;

    public TaskCollection(int capacity)
    {
        if (!(capacity >= 1)) throw new ArgumentException();
        tasks = new ITask[capacity];
        count = 0;
    }

    public int Count
    {
        get { return count; }
        set { count = value; }
    }

    public HashSet<Vertex> V { get { return v; } }
    public HashSet<Tuple<Vertex, Vertex>> E { get { return e; } }
    public void addVertices(ITask task)
    {
        vertex = new Vertex(task);
        V.Add(vertex);
        tasks[count] = task;
        count++;
    }

    public void addEdges(ITask u, ITask v)
    {
        prerequisite = new Vertex(u);
        vertex = new Vertex(v);

        var edge = Tuple.Create(prerequisite, vertex);
        E.Add(edge);
    }

    public void createGraph(ITask[] tasks)
    {
        foreach (Task task in tasks)
        {
            addVertices(task);

            if (task.Dependencies != null)
            {
                foreach (string dep in task.Dependencies)
                {
                    ITask prereq = findTask(dep);
                    addEdges(prereq, task);
                }
            }
        }
    }

    public ITask? findTask(string taskId)
    {
        for (int i = 0; i < Count; i++)
            if (tasks[i].Id == taskId)
            {
                return (tasks[i]);
            }
        /// Console.WriteLine("Task " + taskId + " does not exist.");
        return null;
    }

    public string findIndependentTask(ITask[] tasks)
    {
        foreach (Task t in tasks)
        {
            if (t.Dependencies == null)
            {
                return t.Id;
            }
        }
        return "not found";
    }

    public bool removeTask (string taskId)
    {
        for (int i = 0; i < count; i++)
            if (tasks[i].Id == taskId)
            {
                for (int j = i + 1; j < count; j++)
                    tasks[j - 1] = tasks[j];
                count--;
                removeDependencies(taskId);
                return true;
            }
        return false;
    }

    public void removeDependencies(string taskId)
    {
        for (int i = 0; i < count; i++)
        {
            if (tasks[i].Dependencies != null)
            {
                if ((tasks[i].Dependencies.Contains(taskId)) == true)
                { 
                    tasks[i].Dependencies.Remove(taskId);
                    if ((tasks[i].Dependencies.Contains("T") == false) && (tasks[i].Dependencies.Count == 0))
                    {
                        tasks[i].Dependencies = null;
                    }
                }
            }
        }
    }

    public void changeTime(string id, int newTime)
    {
        ITask task = findTask(id);
        if (task != null)
        {
            task.ExecutionTime = newTime;
        }
        else
            Console.WriteLine("A task with this ID was not found.");
    }
    public ITask[] ToArray()
    {
        ITask[] taskArray = new ITask[count];
        for (int i = 0; i < count; i++)
            taskArray[i] = tasks[i];
        return taskArray;
    }
}