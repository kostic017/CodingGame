using System.Collections.Generic;

public static class PigeonQueue
{

    private static readonly List<Queue<object>> queues = new List<Queue<object>>();

    public static object Create(object[] _)
    {
        queues.Add(new Queue<object>());
        return queues.Count - 1;
    }

    public static object Destroy(object[] args)
    {
        queues[(int)args[0]] = null;
        return null;
    }

    public static object Enqueue(object[] args)
    {
        Queue(args).Enqueue(args[1]);
        return null;
    }

    public static object Dequeue(object[] args)
    {
        return Queue(args).Dequeue();
    }

    public static object Empty(object[] args)
    {
        return Queue(args).Count == 0;
    }

    private static Queue<object> Queue(object[] args)
    {
        return queues[(int)args[0]];
    }

}
