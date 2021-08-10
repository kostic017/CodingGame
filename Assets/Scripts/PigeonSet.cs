using System.Collections.Generic;

public static class PigeonSet
{

    private static readonly List<HashSet<object>> sets = new List<HashSet<object>>();

    public static object Create(object[] _)
    {
        sets.Add(new HashSet<object>());
        return sets.Count - 1;
    }

    public static object Destroy(object[] args)
    {
        sets[(int)args[0]] = null;
        return null;
    }

    public static object Add(object[] args)
    {
        Set(args).Add(args[1]);
        return null;
    }

    public static object Remove(object[] args)
    {
        return Set(args).Remove(args[1]);
    }

    public static object In(object[] args)
    {
        return Set(args).Contains(args[1]);
    }

    private static HashSet<object> Set(object[] args)
    {
        return sets[(int)args[0]];
    }

}
