using System.Collections.Generic;

public static class PigeonPair
{
    class Pair
    {
        internal object X { get; private set; }
        internal object Y { get; private set; }

        public Pair(object x, object y)
        {
            X = x;
            Y = y;
        }
    }

    private static readonly List<Pair> pairs = new List<Pair>();

    public static object Create(object[] args)
    {
        pairs.Add(new Pair(args[0], args[1]));
        return pairs.Count - 1;
    }

    public static object Destroy(object[] args)
    {
        pairs[(int)args[0]] = null;
        return null;
    }

    public static object GetX(object[] args)
    {
        return Get(args).X;
    }

    public static object GetY(object[] args)
    {
        return Get(args).Y;
    }

    private static Pair Get(object[] args)
    {
        return pairs[(int)args[0]];
    }

}
