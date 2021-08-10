public static class PigeonString
{

    public static object Length(object[] args)
    {
        return ((string)args[0]).Length;
    }

    public static object Char(object[] args)
    {
        return ((string)args[0])[(int)args[1]].ToString();
    }

}
