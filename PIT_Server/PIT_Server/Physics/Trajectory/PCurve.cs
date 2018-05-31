using System;
using System.Collections;
using System.Collections.Generic;
using PITPhysics;

public class PCurve
{
    public List<Pair<PVector3, double>> Curve; // вектор + угол для эллипса
    public double e, p, inclination, PositionAngle, PositionTime;
    public bool isTPoint;
    public Pair<PVector3, double> AN, DN, TransitionPoint, Apoapsis, Periapsis;// вектор/угол
    public PVector3 Lv; // результат векторного произведения позиции на скорость(для определения направления движения)

    public PCurve TransitionCurve;
    public PGravityBody PHGBody;


    public double a
    {
        get
        {
            if (e < 1)
                return p / (1 - e * e);
            else
                return p / (e * e - 1);
        }
    } // большая полуось
    public Func<double, double> RFuncAlpha // уравнение r(alpha) радиус-вектора для эллипса и гиперболы
    {
        get
        {
            //if(e < 1)
            return angle => p / (1 + e * Math.Cos(angle));
            //else
            //    return angle => p / (1 - e * Math.Cos(angle));
        }
    }

    public Func<double, PVector3> RFuncAlphaV3
    {
        get
        {
            return angle => ((p / (1 + e * Math.Cos(angle))) * Periapsis.a.normalized).RotateAround(Lv, angle);
        }
    }
    public Func<double, double> AlphaFuncR // уравнение alpha(r) угла радиус-вектора для эллипса и гиперболы
    {
        get
        {
            return r => (p - r) / (e * r);
        }
    }


    public Func<double, double, double> KeplerFunc // Уравнение кеплера для эллипса и гиперболы
    {
        get
        {
            if (e < 1)
                return (E, M) => E - e * Math.Sin(E) - M;
            else
                return (H, M) => e * Math.Sinh(H) - H - M;
        }
    }

    public Func<double, double> KeplerDFunc // производная уравнения кеплера( для алгоритма ньютона)
    {
        get
        {
            if (e < 1)
                return E => 1 - e * Math.Cos(E);
            else
                return H => e * Math.Cosh(H) - 1;
        }
    }

    public Func<double, double> AlphaFromEAnomaly // связь угла и эксцентрической аномалии(и гиперболической э.а.)
    {
        get
        {
            if (e < 1)
                return E => 2 * Math.Atan(Math.Sqrt((1 + e) / (1 - e)) * Math.Tan(E / 2));
            else
                return H => 2 * Math.Atan(Math.Sqrt((1 + e) / (e - 1)) * Math.Tanh(H / 2));
        }
    }

}
