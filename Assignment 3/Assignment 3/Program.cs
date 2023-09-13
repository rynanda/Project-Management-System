
class Program
{
    public static void Main()
    {
        /// Ask user for the exact filepath of tasks text file, then saves the tasks to a list.
        Console.WriteLine("Enter the exact filepath of the text file that includes the tasks, including the text file name. " +
            "\nExample: C:\\Users\\ryani\\Documents\\jobs.txt:\n ");
        string filename = Console.ReadLine();
        Console.WriteLine();

        /// Ensures the filepath entered is valid.
        try
        {
            var testFile = File.ReadAllLines(filename);
        }
        catch
        {
            Console.WriteLine("Not a valid filepath. Please ensure the filepath is entered correctly, including the file name.\n");
            Main();
        }

        var lines = File.ReadAllLines(filename);
        var tasks = new List<string>(lines);
        tasks.RemoveAll(string.IsNullOrWhiteSpace);

        Console.WriteLine("Here are the tasks in the provided text file, including their ID, execution time, and dependencies: ");
        for(int i = 0; i < tasks.Count; i++)
        {
            Console.WriteLine(tasks[i]);
        }
        Console.WriteLine();

        List<ITask> taskList = createTaskList(tasks);

        Console.WriteLine();
        ITask[] Tasks = taskList.ToArray();

        /// Initialize task collection = Directed Acyclic Graph (DAG).
        TaskCollection aCollection = new TaskCollection(Tasks.Length);
        TopologicalSort topologicalSort = new(aCollection);
        EarliestTimes earliestTimes = new(aCollection);

        Menu.MainMenu(filename, taskList, Tasks, aCollection, topologicalSort, earliestTimes);

        Console.ReadLine();
    }

    class Menu
    {
        public static void MainMenu(string filename, List<ITask> taskList, ITask[] Tasks, 
            ITaskCollection aCollection, ITopologicalSort topologicalSort, IEarliestTimes earliestTimes)
        {
            /// Ask user what they would like to do, then do the option.
            Console.WriteLine("What would you like to do?" +
                "\n 1. Add a new task." +
                "\n 2. Remove a task." +
                "\n 3. Change execution time of a task." +
                "\n 4. Save the task list to the provided .txt file." +
                "\n 5. Find a sequence of tasks that do not violate any task dependancy." +
                "\n 6. Find the earliest possible commencement time of each task." +
                "\n 7. Exit. ");
            Console.WriteLine();

            var userChoice = Console.ReadLine();
            switch (userChoice)
            {
                /// Add new task.
                case "1":
                    try
                    {
                        /// Ensures task collection DAG is up to date with previous actions.
                        Tasks = taskList.ToArray();
                        aCollection = new TaskCollection(Tasks.Length);
                        aCollection.createGraph(Tasks);
                        Tasks = aCollection.ToArray();

                        /// Ask user for task information to add to the collection.
                        Console.WriteLine("\nEnter a task ID. Valid IDs begin with T followed by a number, e.g. T1, T5, T62, and do not yet exist in the collection: ");
                        string id = Console.ReadLine();
                        Console.WriteLine("Enter an execution time of the task: ");
                        int time = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Enter valid task IDs separated by commas only. E.g. T1,T2,T9: ");
                        List<string> dependenciesArray = Console.ReadLine().Split(',').Select(i => (i)).ToList();

                        /// Add the new task to the task list, which is then added to the task collection DAG.
                        if (string.IsNullOrEmpty(dependenciesArray[0]) == false)
                        {
                            taskList.Add(new Task(id, time, dependenciesArray));
                        }
                        else
                        {
                            taskList.Add(new Task(id, time, null));
                        }
                        Tasks = taskList.ToArray();
                        aCollection = new TaskCollection(Tasks.Length);
                        aCollection.createGraph(Tasks);
                        Tasks = aCollection.ToArray();

                        Console.WriteLine("\nNew task successfully added! Here is the current task collection: ");
                        printArray(Tasks);
                        Console.WriteLine();
                        Menu.MainMenu(filename, taskList, Tasks, aCollection, topologicalSort, earliestTimes); /// Redo the switch case using a Menu class.
                    }
                    catch
                    {
                        Console.WriteLine("\nSomething went wrong! Please ensure IDs, execution times, and/or dependencies are entered correctly.");
                        Console.WriteLine();
                        Menu.MainMenu(filename, taskList, Tasks, aCollection, topologicalSort, earliestTimes);
                    }
                    break;

                /// Remove task.
                case "2":
                    try
                    {
                        Console.WriteLine("\nHere are the tasks currently in the collection: ");
                        printArray(Tasks);

                        /// Ensures task collection DAG is up to date with previous actions.
                        Tasks = taskList.ToArray();
                        aCollection = new TaskCollection(Tasks.Length);
                        aCollection.createGraph(Tasks);
                        Tasks = aCollection.ToArray();

                        Console.WriteLine("\nEnter the task ID (e.g. T1, T2, T15) you would like to delete: ");
                        string delete = Console.ReadLine();

                        /// Finds the task to delete based on the ID given by user, then deletes it from the task collection DAG.
                        taskList.Remove(aCollection.findTask(delete));
                        aCollection.removeTask(delete);
                        Tasks = aCollection.ToArray();
                        aCollection = new TaskCollection(Tasks.Length);
                        aCollection.createGraph(Tasks);
                        Tasks = aCollection.ToArray();

                        Console.WriteLine("\nTask " + delete + " successfully deleted! Here is the current task collection: ");
                        printArray(Tasks);
                        Console.WriteLine();
                        Menu.MainMenu(filename, taskList, Tasks, aCollection, topologicalSort, earliestTimes);
                    }
                    catch
                    {
                        Console.WriteLine("\nSomething went wrong! Please try again and ensure the task ID is entered correctly.");
                        Console.WriteLine();
                        Menu.MainMenu(filename, taskList, Tasks, aCollection, topologicalSort, earliestTimes);
                    }
                    break;
                /// Change execution time of task.
                case "3":
                    try
                    {
                        Console.WriteLine("\nHere are the tasks currently in the collection: ");
                        printArray(Tasks);

                        Tasks = taskList.ToArray();
                        aCollection = new TaskCollection(Tasks.Length);
                        aCollection.createGraph(Tasks);
                        Tasks = aCollection.ToArray();

                        Console.WriteLine("\nEnter the task ID of which execution time you would like to change: ");
                        string change = Console.ReadLine();
                        Console.WriteLine("\nWhat execution time (in minutes) would you like to set it to?");
                        int newTime = Int32.Parse(Console.ReadLine());

                        /// Find task based on the ID given by the user, then replace the task in the DAG with a new task with a changed execution time.
                        var oldTask = aCollection.findTask(change);
                        Task newTask = new Task(oldTask.Id, newTime, oldTask.Dependencies);
                        taskList.Remove(oldTask);
                        taskList.Add(newTask);
                        aCollection.changeTime(change, newTime);
                        Tasks = aCollection.ToArray();
                        aCollection = new TaskCollection(Tasks.Length);
                        aCollection.createGraph(Tasks);
                        Tasks = aCollection.ToArray();

                        Console.WriteLine("\nExecution time of task " + change + " successfully changed to " + newTime + "! Here is the current task collection: ");
                        printArray(Tasks);
                        Console.WriteLine();
                        Menu.MainMenu(filename, taskList, Tasks, aCollection, topologicalSort, earliestTimes);
                    }
                    catch
                    {
                        Console.WriteLine("\nSomething went wrong! Please ensure the task ID and/or execution time is entered correctly.");
                        Console.WriteLine();
                        Menu.MainMenu(filename, taskList, Tasks, aCollection, topologicalSort, earliestTimes);
                    }
                    break;
                /// Save to file.
                case "4":
                    using (StreamWriter writer = new StreamWriter(filename))
                    {
                        foreach (Task task in Tasks)
                        {
                            if (task.Dependencies == null)
                                writer.WriteLine(task.Id + ", " + task.ExecutionTime);
                            else
                                writer.WriteLine(task.Id + ", " + task.ExecutionTime + ", " + string.Join(", ", task.Dependencies));
                        }
                    }
                    Console.WriteLine("\nSuccessfully saved to " + filename + ".");
                    Console.WriteLine();
                    Menu.MainMenu(filename, taskList, Tasks, aCollection, topologicalSort, earliestTimes);
                    break;
                /// Find sequence of tasks that do not violate any task dependency then save it to a file called Sequence.txt
                /// at a path the user specifies.
                case "5":
                    try
                    {
                        Console.WriteLine();

                        Tasks = taskList.ToArray();
                        aCollection = new TaskCollection(Tasks.Length);
                        aCollection.createGraph(Tasks);
                        topologicalSort = new TopologicalSort(aCollection);

                        /// Use Topological Sort algorithm to find the sequence.
                        ITask[] topSort = topologicalSort.TopSort();
                        Console.WriteLine("Topological Sorted Tasks:");
                        printTopSort(topSort);

                        /// Save sequence to Sequence.txt
                        Console.WriteLine("\nEnter the exact filepath where you would like to save Sequence.txt: ");
                        string saveSequence = Console.ReadLine() + "\\Sequence.txt";
                        using (StreamWriter writer = new StreamWriter(saveSequence))
                        {
                            List<string> sequenced = new List<string>();
                            foreach (Task task in topSort)
                            {
                                sequenced.Add(task.Id);
                            }

                            writer.WriteLine(String.Join(", ", sequenced));
                            Console.WriteLine("\nSuccessfully saved to " + saveSequence);
                        }

                        /// Exits gracefully after finding sequence.
                        Console.WriteLine();
                        Console.WriteLine("Thank you for using this program! Have a good day!");
                        Environment.Exit(0);
                    }

                    /// Goes back to main menu in the case that no sequence of tasks could be found.
                    catch
                    {
                        Console.WriteLine("\nCould not find a sequence of tasks that do not violate any task dependencies " +
                            "or filepath not valid!");
                        Console.WriteLine();
                        Menu.MainMenu(filename, taskList, Tasks, aCollection, topologicalSort, earliestTimes);
                    }
                    
                    break;
                
                /// Find sequence that do not violate any task dependencies, then calculate commencement times of each task
                /// based on this sequence.
                case "6":
                    try
                    {
                        Tasks = taskList.ToArray();
                        ITask[] copyTasks = Tasks;
                        aCollection = new TaskCollection(Tasks.Length);
                        aCollection.createGraph(Tasks);
                        topologicalSort = new TopologicalSort(aCollection);

                        ITask[] topSortEarliest = topologicalSort.TopSort();
                        Console.WriteLine("\nSequence of tasks that do not violate task dependencies: ");
                        printTopSort(topSortEarliest);

                        /// Saves the sorted numerical task IDs to a list.
                        List<int> taskNumbersSorted = new List<int>(topSortEarliest.Length);
                        int count = 0;
                        List<string> sorted = new List<string>();
                        foreach (Task task in topSortEarliest)
                        {
                            sorted.Add(task.Id);
                        }

                        foreach (string task in sorted)
                        {
                            int taskNumber = Int32.Parse(task.Substring(1));
                            taskNumbersSorted.Add(taskNumber);
                            count++;
                        }

                        ITask[] sortedTasks = topSortEarliest.ToArray();
                        ITaskCollection earliestCollection = new TaskCollection(sortedTasks.Length);
                        earliestCollection.createGraph(sortedTasks);
                        earliestTimes = new EarliestTimes(earliestCollection);

                        /// Uses depth first search on the sequence to calculate commencement times of each task.
                        earliestTimes.DFS(taskNumbersSorted[0], taskNumbersSorted);

                        /// Exits after calculating commencement times.
                        Console.WriteLine("\nThank you for using this program! Have a good day!");
                        Environment.Exit(0);
                    }
                    catch
                    {
                        Console.WriteLine("\nCould not find commencement times because " +
                            "no sequence of tasks that do not violate any task dependencies were found!");
                        Console.WriteLine();
                        Menu.MainMenu(filename, taskList, Tasks, aCollection, topologicalSort, earliestTimes);
                    }
                    break;

                /// Exit gracefully.
                case "7":
                    Environment.Exit(0);
                    break;
            }
        }
    }

    static List<ITask> createTaskList(List<string> tasks)
    {
        List<ITask> taskList = new List<ITask>();
        for (int i = 0; i < tasks.Count; i++)
        {
            string task = tasks[i];
            string stringId = task.Split(',')[0];
            int executionTime = Int32.Parse(task.Split(',')[1]);
            string stringDependencies = string.Join(',', task.Split(',').Skip(2)).Trim();
            List<string> initDependenciesList = stringDependencies.Split("\n").ToList();

            foreach (string dependency in initDependenciesList)
            {
                List<string> dependencyList = dependency.Split(", ").ToList();
                if (dependency.Contains(", "))
                {
                    foreach (string dep in dependencyList)
                    {
                        Console.WriteLine(stringId + " is dependant on " + dep);
                    }
                    taskList.Add(new Task(stringId, executionTime, dependencyList));
                }
                else if (dependency.Contains("T"))
                {
                    Console.WriteLine(stringId + " is dependant on " + dependency);
                    taskList.Add(new Task(stringId, executionTime, dependencyList));
                }
                else
                {
                    Console.WriteLine(stringId + " has no dependencies. ");
                    taskList.Add(new Task(stringId, executionTime, null));
                }
            }
        }
        return taskList;
    }

    static void printArray(ITask[] A)
    {
        for (int i = 0; i < A.Length; i++)
            Console.Write(A[i].ToString());
    }

    static void printTopSort(ITask[] TS)
    {
        List<string> sorted = new List<string>();
        foreach (Task task in TS)
        {
            sorted.Add(task.Id);
        }

        Console.WriteLine(String.Join(", ", sorted));
    }
}