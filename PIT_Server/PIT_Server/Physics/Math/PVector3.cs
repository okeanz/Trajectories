using System;
using System.Text;
public struct PVector3
{
    public double X, Y, Z;
    public string Tag;
    public static PVector3 forward = new PVector3(0, 0, 1);
    public static PVector3 up = new PVector3(0, 1, 0);
    public static PVector3 right = new PVector3(1, 0, 0);
    public static PVector3 zero = new PVector3(0, 0, 0);

    public PVector3 normalized => this / Length();

    public double magnitude => Length();

    public double sqrMagnitude => Math.Pow(Length(),2);

    public PVector3(double X, double Y, double Z)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
        Tag = string.Empty;
    }
    public void Normalize()
    {
        double length = Length();
        X /= length;
        Y /= length;
        Z /= length;
    }
    public double Length()
    {
        return Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public PVector3 RotateAround(PVector3 axis, double angle) //RADIANS
    {
        return rot(this, PQuaternion.CreateFromAxisAngle(axis, angle));
    }

    PVector3 rot(PVector3 v, PQuaternion q)
    {
        PVector3 t = 2 * PVector3.Cross(q.XYZ, v);
        return v + q.W * t + PVector3.Cross(q.XYZ, t);
    }
    public static PVector3 operator +(PVector3 v1, PVector3 v2)
    {
        return new PVector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
    }
    public static PVector3 operator -(PVector3 v1, PVector3 v2)
    {
        return new PVector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
    }
    public static PVector3 operator *(PVector3 v1, double t)
    {
        return new PVector3(v1.X * t, v1.Y * t, v1.Z * t);
    }
    public static PVector3 operator *(double t,PVector3 v1)
    {
        return new PVector3(v1.X * t, v1.Y * t, v1.Z * t);
    }
    //public static PVector3 operator *(PVector3 v, PQuaternion q)
    //{
    //    PVector3 t = 2 * PVector3.Cross(q.XYZ, v);
    //    return v + q.W * t + PVector3.Cross(q.XYZ, t);
    //}

    public static PVector3 operator *(PQuaternion q, PVector3 v)
    {
        PVector3 res;
        var t = PQuaternion.QMulV(q, v);
        t = t*PQuaternion.Inverse(q);
        res = t.XYZ;
        return res;
    }
    public static PVector3 operator /(PVector3 v1, double t)
    {
        return new PVector3(v1.X / t, v1.Y / t, v1.Z / t);
    }
    public static double Angle(PVector3 v1, PVector3 v2)//RADIANS !!!!
    {
        return Math.Acos(Dot(v1, v2) / (v1.Length() * v2.Length()));
    }
    public static double Dot(PVector3 v1,PVector3 v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
    }
    public static PVector3 Cross(PVector3 v1,PVector3 v2)
    {
        return -new PVector3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
    }
    public static bool operator ==(PVector3 v1, PVector3 v2)
    {
        if (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z)
            return true;
        else
            return false;
    }
    public static bool operator !=(PVector3 v1, PVector3 v2)
    {
        return !(v1 == v2);
    }

    public static PVector3 operator -(PVector3 v)
    {
        return new PVector3(-v.X,-v.Y,-v.Z);
    }
    public override bool Equals(object obj)
    {
        return (this == (PVector3)obj);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder(32);
        sb.Append("{X:");
        sb.Append(this.X);
        sb.Append(" Y:");
        sb.Append(this.Y);
        sb.Append(" Z:");
        sb.Append(this.Z);
        sb.Append("}");
        return sb.ToString();
    }
}