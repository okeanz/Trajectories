
using System;

public static class HyperbolicFuncs
{
    public static double Atanh(double x)
    {
        if (Math.Abs(x) > 1)
            throw new ArgumentException("x must be less than 1");

        return 0.5 * Math.Log((1 + x) / (1 - x));
    }
    public static double Acosh(double x)
    {
        return Math.Log(x + Math.Sqrt(x*x-1));
    }

    public static double Arsh(double x)
    {
        return Math.Log(x + Math.Sqrt(x*x + 1));
    }

}
