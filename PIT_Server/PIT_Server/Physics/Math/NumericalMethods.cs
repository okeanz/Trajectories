using System;
using System.Collections;
using PITPhysics;

public static class NumericalMethods
{

    public static double NewtonForKepler(Func<double, double, double> Function, Func<double, double> DFunction, double x0, double M, out int iterations, double epsilon = PSettings.EpsilonNumMethods)
    {
        iterations = 0;
        double y;
        do //TODO: Добавить доп. расчет точки приближения более простым методом
        {
            iterations++;
            x0 = x0 - Function(x0, M) / DFunction(x0);
            y = Function(x0, M);
            if (iterations > PSettings.NumMethodsMaxIterations)
                break;
        }
        while (Math.Abs(y) >= epsilon);
        return x0;
    }

    public static double StaticPointKepler(double e, double M, out int iterations, double epsilon = PSettings.EpsilonNumMethods)
    {
        Func<double, double> f;
        Func<double, double> next;
        if (e < 1)
            f = E => E - e * Math.Sin(E) - M;
        else
            f =  H => e * Math.Sinh(H) - H - M;

        if (e < 1)
            next = En => e*Math.Sin(En) + M;
        else
            next = Hn => HyperbolicFuncs.Arsh((Hn + M)/e);


        iterations = 0;
        double x0 = M;
        double y;
        do
        {
            iterations++;
            x0 = next(x0);
            y = f(x0);
            if (iterations > PSettings.NumMethodsMaxIterations)
                break;
        }
        while (Math.Abs(y) >= epsilon);
        return x0;
    }
    public static double Newton(Func<double, double> Function, Func<double, double> DFunction, double x0, out int iterations, double epsilon = PSettings.EpsilonNumMethods)
    {
        iterations = 0;
        double y;
        do
        {
            iterations++;
            x0 = x0 - Function(x0) / DFunction(x0);
            y = Function(x0);
            if (iterations > PSettings.NumMethodsMaxIterations)
                break;
        }
        while (Math.Abs(y) >= epsilon);
        return x0;
    }

    public static double Dichotomy(Func<double, double> Function, double a, double b, out int iterations, double delta = PSettings.DeltaStep)
    {
        iterations = 0;
        double x = (a + b) / 2;
        double xpre = b;
        double x1, x2;
        do
        {
            iterations++;
            x1 = x - delta;
            x2 = x + delta;
            double fx1 = Function(x1);
            double fx2 = Function(x2);
            if (fx1 < 0 && fx2 < 0)
            {
                xpre = x;
                x = (a + x)/2;
                continue;
            }
            else if (fx1 > 0 && fx2 > 0)
            {
                var q = xpre;
                x = (xpre + x)/2;
                xpre = q;
                continue;
            }
            else if (fx1*fx2 < 0)
                return x1;
        } while (iterations <= PSettings.NumMethodsMaxIterations);



        return x1;
    }

}
