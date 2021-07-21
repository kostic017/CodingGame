using System.Collections.Generic;

class PigeonMap
{

    private static readonly List<Dictionary<object, object>> maps = new List<Dictionary<object, object>>();

    public static object Create(object[] _)
    {
        maps.Add(new Dictionary<object, object>());
        return maps.Count - 1;
    }

    public static object Destroy(object[] args)
    {
        maps[(int)args[0]] = null;
        return null;
    }

    public static object Add(object[] args)
    {
        Map(args).Add(args[1], args[2]);
        return null;
    }

    public static object Get(object[] args)
    {
        return Map(args)[args[1]];
    }

    public static object Remove(object[] args)
    {
        return Map(args).Remove(args[1]);
    }

    public static object Empty(object[] args)
    {
        return Map(args).Count == 0;
    }

    private static Dictionary<object, object> Map(object[] args)
    {
        return maps[(int)args[0]];
    }

}
