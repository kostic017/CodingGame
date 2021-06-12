using System.Collections.Generic;

public static class PigeonList
{

    private static readonly List<List<object>> lists = new List<List<object>>();

    public static object Create(object[] _)
    {
        lists.Add(new List<object>());
        return lists.Count - 1;
    }

    public static object Destroy(object[] args)
    {
        lists[(int)args[0]] = null;
        return null;
    }

    public static object Add(object[] args)
    {
        List(args).Add(args[1]);
        return null;
    }

    public static object Remove(object[] args)
    {
        List(args).RemoveAt((int)args[1]);
        return null;
    }

    public static object Get(object[] args)
    {
        return List(args)[(int)args[1]];
    }

    public static object Size(object[] args)
    {
        return List(args).Count;
    }

    private static List<object> List(object[] args)
    {
        return lists[(int)args[0]];
    }

}
