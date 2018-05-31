using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PITPhysics
{
    public class PTrajectory
    {
        public PCurve Curve;

        public PRigidbody Body;

        public PWorld World;

        public PTrajectory(PRigidbody body, PWorld world)
        {
            Body = body;
            World = world;
        }
        public void UpdateCurve(PVector3 velocity, int cElementCount = PSettings.CurveElementCount)
        {
            var ElInfo = GetEllipseInfo(Body.LocalPosition, velocity, Body.Mass, Body.GravityBody.Mass);
            Curve = MakeCurve(ElInfo.a, ElInfo.b, Body.LocalPosition, velocity, Body.GravityBody.Position, Body.GravityBody.Mass, cElementCount);
            //Debug.Log("e: "+ Curve.e + " p: "+Curve.p + " iA: "+Curve.PositionAngle);

        }

        public PVector3 GetVelocityMagnitude()
        {
            return GetVelocityMagnitude(Body.LocalPosition, Curve.PositionAngle);
        }
        public PVector3 GetVelocityMagnitude(PVector3 Position, double angle)
        {
            double gm = PWorld.G * Body.GravityBody.Mass;
            double k = Math.Sqrt(gm / Curve.p);
            PVector3 Vn = PVector3.Cross(Curve.Lv, Position).normalized * (k * (1 + Curve.e * Math.Cos(angle)));
            PVector3 Vr = Position.normalized * (k * Curve.e * Math.Sin(angle));
            return Vn + Vr;
        }

        public PCurve MakeCurve(double e, double p, PVector3 pos, PVector3 velocity, PVector3 GBodyPosition, double GBodyMass, int StepCount, bool TransitionCurve = false)
        {
            #region Начальный угол, построение кривой, апоцентр\перицентр

            var crv = new PCurve
            {
                e = e,
                p = p,
                Curve = new List<Pair<PVector3, double>>(),
                Lv = PVector3.Cross(pos, velocity)
            };



            var IAngleCosValue = (p - pos.magnitude) / (e * pos.magnitude);
            double InitAngle = Math.Acos(IAngleCosValue);
            if (double.IsNaN(InitAngle))
                InitAngle = IAngleCosValue < 0 ? Math.PI : 0;
            if (PVector3.Dot(pos, velocity) < 0) // проверяем направление движения (см. радиальную компоненту скорости)
                InitAngle = 2 * Math.PI - InitAngle;

            var Vz = pos.normalized.RotateAround(crv.Lv, -InitAngle); // выравниваем на перицентр
            crv.Periapsis = new Pair<PVector3, double>(Vz * crv.RFuncAlpha(0), 0);
            crv.Apoapsis = new Pair<PVector3, double>(crv.RFuncAlphaV3(Math.PI), Math.PI);





            for (double x = -Math.PI; x <= Math.PI; x += (2 * Math.PI) / StepCount)
            {
                var alpha = e < 1 ? crv.AlphaFromEAnomaly(x) : x;
                //var alpha = crv.AlphaFromEAnomaly(x);

                if (e < 1)
                    crv.Curve.Add(new Pair<PVector3, double>(crv.RFuncAlphaV3(alpha) + GBodyPosition, alpha));
                else
                    if (Math.Abs(x) < Math.Acos(-1 / crv.e))
                    crv.Curve.Add(new Pair<PVector3, double>(crv.RFuncAlphaV3(alpha) + GBodyPosition, alpha));
            }


            crv.PositionAngle = InitAngle;
            #endregion
            #region Начальное время
            double t;
            if (crv.e < 1)
            {
                double E = 2 * Math.Atan(Math.Sqrt((1 - e) / (1 + e)) * Math.Tan(InitAngle / 2));
                // эксцентрическая аномалия (идем по обратному пути к уравнению кеплера)
                double a = crv.a; // большая полуось
                t = Math.Sqrt(a * a * a / (PWorld.G * GBodyMass)) * (E - e * Math.Sin(E));
            }
            else
            {
                double H = 2 * HyperbolicFuncs.Atanh(Math.Sqrt((e - 1) / (e + 1)) * Math.Tan(InitAngle / 2));
                double k = Math.Sqrt(Math.Pow(crv.a, 3) / (PWorld.G * GBodyMass)); // 1/n
                t = k * (e * Math.Sinh(H) - H);
            }


            crv.PositionTime = t; // для уравнения кеплера начальное время - до перицентра

            #endregion
            #region Наклонение орбиты, AN\DN

            double inclination = PVector3.Angle(crv.Lv, PVector3.up) * 180.0 / Math.PI;
            crv.inclination = inclination <= 90 ? inclination : 180 - inclination;
            if (Math.Abs(crv.inclination) > 0)
            {
                var DescendingNodeV = PVector3.Cross(crv.Lv, PVector3.up).normalized;
                double DNAngle = PVector3.Angle(DescendingNodeV, crv.Periapsis.a);
                double DNLength = crv.RFuncAlpha(DNAngle);
                double ANAngle = DNAngle - Math.PI;
                double ANLength = crv.RFuncAlpha(ANAngle);


                crv.DN = new Pair<PVector3, double>(DescendingNodeV * DNLength, DNAngle);
                crv.AN = new Pair<PVector3, double>(-DescendingNodeV * ANLength, -DNAngle);
            }


            #endregion
            #region Точка перехода
            var TPoint = new Pair<PVector3, double>();
            var isTPoint = false;


            foreach (var gb in World.Gravitybodies)
            {
                foreach (var point in crv.Curve)
                {
                    double distSqr1 = (point.a - GBodyPosition).magnitude;
                    double distSqr2 = (point.a - gb.Position).magnitude;
                    if (!(distSqr1 * distSqr1 * gb.Mass - distSqr2 * distSqr2 * GBodyMass > 0))
                        // грубый метод поиска точки перехода
                        continue;
                    isTPoint = true;
                    crv.PHGBody = gb;

                    var C = GBodyPosition - gb.Position;
                    Func<double, double> Falpha =
                        alpha =>
                            Math.Pow(crv.RFuncAlpha(alpha), 2) * crv.PHGBody.Mass -
                            (C + crv.RFuncAlphaV3(alpha)).sqrMagnitude * GBodyMass;

                    int i = crv.Curve.IndexOf(point);
                    int it;
                    double a = /*crv.Curve[i+1 > crv.Curve.Count ? 1 : i+1].b*/Math.PI;
                    double b = /*crv.Curve[i - 3 < 0 ? crv.Curve.Count - 3 : i-3].b*/0;
                    double angle = NumericalMethods.Dichotomy(Falpha, a, b, out it);
                    // уточнение координат точки
                    TPoint = new Pair<PVector3, double>(crv.RFuncAlphaV3(angle) + GBodyPosition, angle);
                    if (it > 100)
                        Debug.Write("angle: " + angle * 180 / Math.PI + " iterations:" + it + " a:" +
                                            a * 180 / Math.PI + " b:" + b * 180 / Math.PI);
                    break;
                }
                if (isTPoint)
                    break;
            }



            if (isTPoint)
            {
                crv.isTPoint = true;
                crv.TransitionPoint = TPoint;
            }
            else
                crv.isTPoint = false;
            #endregion
            #region Фантомная траектория перехода

            if (Curve == null) return crv;

            if (!isTPoint || TransitionCurve) return crv;


            PCurve PHCurve;
            PVector3 PHVelocity = GetVelocityMagnitude(TPoint.a, TPoint.b);
            PVector3 PHPosition = TPoint.a - crv.PHGBody.Position;
            var PHCInfo = GetEllipseInfo(PHPosition, PHVelocity, Body.Mass, crv.PHGBody.Mass);
            PHCurve = MakeCurve(PHCInfo.a, PHCInfo.b, PHPosition, PHVelocity, crv.PHGBody.Position, crv.PHGBody.Mass, PSettings.CurveElementCount, true);
            crv.TransitionCurve = PHCurve;

            #endregion


            return crv;
        }




        public static Pair<double, double> GetEllipseInfo(PVector3 position, PVector3 speed, double mass, double Mass)
        {
            double E = mass * speed.magnitude * speed.magnitude / 2 - PWorld.G * Mass * mass / position.magnitude;

            double L = PVector3.Cross(position, (mass * speed)).magnitude;

            double e = Math.Sqrt(1 + 2 * E * L * L / (PWorld.G * PWorld.G * Mass * Mass * mass * mass * mass));

            double p = L * L / (PWorld.G * Mass * mass * mass);

            //double mu = PWorld.G*Mass;

            //double p = PVector3.Cross(position, speed).sqrMagnitude/mu;

            //double h = speed.sqrMagnitude - (2*mu)/position.magnitude;

            //double e = Math.Sqrt(1 + h*p/mu);




            return new Pair<double, double>(e, p);
        }


    }

    public struct Pair<A, B>
    {
        public A a;
        public B b;
        public Pair(A a, B b)
        {
            this.a = a;
            this.b = b;
        }
    }
}
