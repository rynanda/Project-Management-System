
public class EarliestTimes : IEarliestTimes
{
    public ITaskCollection Tasks { get; set;}

    public EarliestTimes(ITaskCollection tasks)
    {
        Tasks = tasks;
    }

    private int timeCount = 0;
    private string filepath;

    public void DFS(int v, List<int> sorted)
    {
        Console.WriteLine();
        Console.WriteLine("Enter the filepath where you would like to save the commencement times text file: ");
        filepath = Console.ReadLine() + "\\EarliestTimes.txt";
        File.WriteAllText(filepath, string.Empty);
        int V = sorted.Count + 1;
        bool[] visited = new bool[V];

        Console.WriteLine("\nBased on this sequence, here are the commencement times of the tasks: ");

        DFSHelper(v, visited, sorted);

        Console.WriteLine("\nSuccessfully saved to " + filepath);
    }

    public void DFSHelper(int v, bool[] visited, List<int> sorted)
    {
        visited[v] = true;
        Console.WriteLine("T" + v + ", " + timeCount);

        if (!File.Exists(filepath))
        {
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                writer.WriteLine("T" + v + ", " + timeCount);
            }
        }
        else
        {
            using (StreamWriter writeMore = File.AppendText(filepath))
            {
                writeMore.WriteLine("T" + v + ", " + timeCount);
            }
        }

        timeCount = timeCount + Tasks.findTask("T" + v).ExecutionTime;

        foreach (var n in sorted)
        {
            if (!visited[n])
                DFSHelper(n, visited, sorted);
        }
    }
}