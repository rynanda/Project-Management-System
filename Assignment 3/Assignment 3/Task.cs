using System;
using System.Collections.Generic;

public class Task : ITask
{
    public Task(string taskId, int executionTime, List<string>? dependencies = null)
    {
        Id = taskId;
        ExecutionTime = executionTime;
        Dependencies = dependencies;
    }

    private string taskId;
    private int executionTime;
    private List<string> dependencies;

    public string Id
    {
        get
        {
            return taskId;
        }
        private set
        {
            taskId = value;
        }
    }

    public int ExecutionTime
    { 
        get 
        { 
            return executionTime; 
        }
        set 
        {
            executionTime = value;
        }
    }

    public List<string>? Dependencies
    {
        get
        {
            return dependencies;
        }
        set
        {
            dependencies = value;
        }
    }

    public override string ToString()
    {
        if (dependencies != null)
            return $"Task(taskId: {Id}, executionTime: {executionTime}, dependencies: " + String.Join(", ", dependencies) + ")\n";
        else
            return $"Task(taskId: {Id}, executionTime: {executionTime}, dependencies: " + "no dependencies." + ")\n";
    }
}