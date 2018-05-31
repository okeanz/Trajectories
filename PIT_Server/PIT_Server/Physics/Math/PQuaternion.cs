using System;
using System.Text;
public struct PQuaternion
{
    public double X;
    public double Y;
    public double Z;
    public double W;
    static PQuaternion _identity = new PQuaternion(0, 0, 0, 1);
    public PVector3 XYZ
    {
        get
        {
            return new PVector3(X, Y, Z);
        }
    }

    public PQuaternion(double x, double y, double z, double w)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.W = w;
    }


    public PQuaternion(PVector3 vectorPart, double scalarPart)
    {
        this.X = vectorPart.X;
        this.Y = vectorPart.Y;
        this.Z = vectorPart.Z;
        this.W = scalarPart;
    }

    public static PQuaternion identity
    {
        get { return _identity; }
    }


    public static PQuaternion Add(PQuaternion PQuaternion1, PQuaternion PQuaternion2)
    {
        PQuaternion1.X += PQuaternion2.X;
        PQuaternion1.Y += PQuaternion2.Y;
        PQuaternion1.Z += PQuaternion2.Z;
        PQuaternion1.W += PQuaternion2.W;
        return PQuaternion1;
    }
    public static PQuaternion Inverse(PQuaternion PQuaternion)
    {
        PQuaternion result;
        double m1 = 1.0F / ((PQuaternion.X * PQuaternion.X) + (PQuaternion.Y * PQuaternion.Y) + (PQuaternion.Z * PQuaternion.Z) + (PQuaternion.W * PQuaternion.W));
        result.X = -PQuaternion.X * m1;
        result.Y = -PQuaternion.Y * m1;
        result.Z = -PQuaternion.Z * m1;
        result.W = PQuaternion.W * m1;
        return result;
    }
    public void Conjugate()
    {
        this.X = -this.X;
        this.Y = -this.Y;
        this.Z = -this.Z;
    }

    public static PQuaternion Conjugate(PQuaternion value)
    {
        PQuaternion PQuaternion;
        PQuaternion.X = -value.X;
        PQuaternion.Y = -value.Y;
        PQuaternion.Z = -value.Z;
        PQuaternion.W = value.W;
        return PQuaternion;
    }

    public static void Conjugate(ref PQuaternion value, out PQuaternion result)
    {
        result.X = -value.X;
        result.Y = -value.Y;
        result.Z = -value.Z;
        result.W = value.W;
    }

    public static PQuaternion CreateFromYawPitchRoll(double yaw, double pitch, double roll)
    {
        PQuaternion PQuaternion;
        PQuaternion.X = (((double)Math.Cos((double)(yaw * 0.5f)) * (double)Math.Sin((double)(pitch * 0.5f))) * (double)Math.Cos((double)(roll * 0.5f))) + (((double)Math.Sin((double)(yaw * 0.5f)) * (double)Math.Cos((double)(pitch * 0.5f))) * (double)Math.Sin((double)(roll * 0.5f)));
        PQuaternion.Y = (((double)Math.Sin((double)(yaw * 0.5f)) * (double)Math.Cos((double)(pitch * 0.5f))) * (double)Math.Cos((double)(roll * 0.5f))) - (((double)Math.Cos((double)(yaw * 0.5f)) * (double)Math.Sin((double)(pitch * 0.5f))) * (double)Math.Sin((double)(roll * 0.5f)));
        PQuaternion.Z = (((double)Math.Cos((double)(yaw * 0.5f)) * (double)Math.Cos((double)(pitch * 0.5f))) * (double)Math.Sin((double)(roll * 0.5f))) - (((double)Math.Sin((double)(yaw * 0.5f)) * (double)Math.Sin((double)(pitch * 0.5f))) * (double)Math.Cos((double)(roll * 0.5f)));
        PQuaternion.W = (((double)Math.Cos((double)(yaw * 0.5f)) * (double)Math.Cos((double)(pitch * 0.5f))) * (double)Math.Cos((double)(roll * 0.5f))) + (((double)Math.Sin((double)(yaw * 0.5f)) * (double)Math.Sin((double)(pitch * 0.5f))) * (double)Math.Sin((double)(roll * 0.5f)));
        return PQuaternion;
    }

    public static void CreateFromYawPitchRoll(double yaw, double pitch, double roll, out PQuaternion result)
    {
        result.X = (((double)Math.Cos((double)(yaw * 0.5f)) * (double)Math.Sin((double)(pitch * 0.5f))) * (double)Math.Cos((double)(roll * 0.5f))) + (((double)Math.Sin((double)(yaw * 0.5f)) * (double)Math.Cos((double)(pitch * 0.5f))) * (double)Math.Sin((double)(roll * 0.5f)));
        result.Y = (((double)Math.Sin((double)(yaw * 0.5f)) * (double)Math.Cos((double)(pitch * 0.5f))) * (double)Math.Cos((double)(roll * 0.5f))) - (((double)Math.Cos((double)(yaw * 0.5f)) * (double)Math.Sin((double)(pitch * 0.5f))) * (double)Math.Sin((double)(roll * 0.5f)));
        result.Z = (((double)Math.Cos((double)(yaw * 0.5f)) * (double)Math.Cos((double)(pitch * 0.5f))) * (double)Math.Sin((double)(roll * 0.5f))) - (((double)Math.Sin((double)(yaw * 0.5f)) * (double)Math.Sin((double)(pitch * 0.5f))) * (double)Math.Cos((double)(roll * 0.5f)));
        result.W = (((double)Math.Cos((double)(yaw * 0.5f)) * (double)Math.Cos((double)(pitch * 0.5f))) * (double)Math.Cos((double)(roll * 0.5f))) + (((double)Math.Sin((double)(yaw * 0.5f)) * (double)Math.Sin((double)(pitch * 0.5f))) * (double)Math.Sin((double)(roll * 0.5f)));
    }

    public static PQuaternion CreateFromAxisAngle(PVector3 axis, double angle)
    {
        axis.Normalize();
        double sin_a = Math.Sin(angle / 2.0);
        return new PQuaternion(axis.X * sin_a, axis.Y * sin_a, axis.Z * sin_a, Math.Cos(angle / 2.0));
    }


    public static void CreateFromAxisAngle(ref PVector3 axis, double angle, out PQuaternion result)
    {
        double sin_a = (double)Math.Sin(angle / 2.0f);
        result.X = axis.X * sin_a;
        result.Y = axis.Y * sin_a;
        result.Z = axis.Z * sin_a;
        result.W = (double)Math.Cos(angle / 2.0f);
    }


    public static double Dot(PQuaternion PQuaternion1, PQuaternion PQuaternion2)
    {
        return (PQuaternion1.X * PQuaternion2.X) + (PQuaternion1.Y * PQuaternion2.Y) + (PQuaternion1.Z * PQuaternion2.Z) + (PQuaternion1.W * PQuaternion2.W);
    }


    public static void Dot(ref PQuaternion PQuaternion1, ref PQuaternion PQuaternion2, out double result)
    {
        result = (PQuaternion1.X * PQuaternion2.X) + (PQuaternion1.Y * PQuaternion2.Y) + (PQuaternion1.Z * PQuaternion2.Z) + (PQuaternion1.W * PQuaternion2.W);
    }

    public static void Slerp(ref PQuaternion PQuaternion1, ref PQuaternion PQuaternion2, double amount, out PQuaternion result)
    {
        double q2, q3;

        double q4 = (PQuaternion1.X * PQuaternion2.X) + (PQuaternion1.Y * PQuaternion2.Y) + (PQuaternion1.Z * PQuaternion2.Z) + (PQuaternion1.W * PQuaternion2.W);
        bool flag = false;
        if (q4 < 0.0F)
        {
            flag = true;
            q4 = -q4;
        }
        if (q4 > 0.999999F)
        {
            q3 = 1.0F - amount;
            q2 = flag ? -amount : amount;
        }
        else
        {
            double q5 = (double)System.Math.Acos((double)q4);
            double q6 = (double)(1.0 / System.Math.Sin((double)q5));
            q3 = (double)System.Math.Sin((double)((1.0F - amount) * q5)) * q6;
            q2 = flag ? (double)-System.Math.Sin((double)(amount * q5)) * q6 : (double)System.Math.Sin((double)(amount * q5)) * q6;
        }
        result.X = (q3 * PQuaternion1.X) + (q2 * PQuaternion2.X);
        result.Y = (q3 * PQuaternion1.Y) + (q2 * PQuaternion2.Y);
        result.Z = (q3 * PQuaternion1.Z) + (q2 * PQuaternion2.Z);
        result.W = (q3 * PQuaternion1.W) + (q2 * PQuaternion2.W);
    }
    public static PQuaternion Negate(PQuaternion PQuaternion)
    {
        PQuaternion result;
        result.X = -PQuaternion.X;
        result.Y = -PQuaternion.Y;
        result.Z = -PQuaternion.Z;
        result.W = -PQuaternion.W;
        return result;
    }


    public static void Negate(ref PQuaternion PQuaternion, out PQuaternion result)
    {
        result.X = -PQuaternion.X;
        result.Y = -PQuaternion.Y;
        result.Z = -PQuaternion.Z;
        result.W = -PQuaternion.W;
    }


    public void Normalize()
    {
        double f1 = 1.0F / (double)System.Math.Sqrt((double)((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W)));
        this.X *= f1;
        this.Y *= f1;
        this.Z *= f1;
        this.W *= f1;
    }


    public static PQuaternion Normalize(PQuaternion PQuaternion)
    {
        PQuaternion result;
        double f1 = 1.0F / (double)System.Math.Sqrt((double)((PQuaternion.X * PQuaternion.X) + (PQuaternion.Y * PQuaternion.Y) + (PQuaternion.Z * PQuaternion.Z) + (PQuaternion.W * PQuaternion.W)));
        result.X = PQuaternion.X * f1;
        result.Y = PQuaternion.Y * f1;
        result.Z = PQuaternion.Z * f1;
        result.W = PQuaternion.W * f1;
        return result;
    }


    public static void Normalize(ref PQuaternion PQuaternion, out PQuaternion result)
    {
        double f1 = 1.0F / (double)System.Math.Sqrt((double)((PQuaternion.X * PQuaternion.X) + (PQuaternion.Y * PQuaternion.Y) + (PQuaternion.Z * PQuaternion.Z) + (PQuaternion.W * PQuaternion.W)));
        result.X = PQuaternion.X * f1;
        result.Y = PQuaternion.Y * f1;
        result.Z = PQuaternion.Z * f1;
        result.W = PQuaternion.W * f1;
    }


    public static PQuaternion operator +(PQuaternion PQuaternion1, PQuaternion PQuaternion2)
    {
        PQuaternion1.X += PQuaternion2.X;
        PQuaternion1.Y += PQuaternion2.Y;
        PQuaternion1.Z += PQuaternion2.Z;
        PQuaternion1.W += PQuaternion2.W;
        return PQuaternion1;
    }


    public static PQuaternion operator /(PQuaternion PQuaternion1, PQuaternion PQuaternion2)
    {
        PQuaternion result;

        double w5 = 1.0F / ((PQuaternion2.X * PQuaternion2.X) + (PQuaternion2.Y * PQuaternion2.Y) + (PQuaternion2.Z * PQuaternion2.Z) + (PQuaternion2.W * PQuaternion2.W));
        double w4 = -PQuaternion2.X * w5;
        double w3 = -PQuaternion2.Y * w5;
        double w2 = -PQuaternion2.Z * w5;
        double w1 = PQuaternion2.W * w5;

        result.X = (PQuaternion1.X * w1) + (w4 * PQuaternion1.W) + ((PQuaternion1.Y * w2) - (PQuaternion1.Z * w3));
        result.Y = (PQuaternion1.Y * w1) + (w3 * PQuaternion1.W) + ((PQuaternion1.Z * w4) - (PQuaternion1.X * w2));
        result.Z = (PQuaternion1.Z * w1) + (w2 * PQuaternion1.W) + ((PQuaternion1.X * w3) - (PQuaternion1.Y * w4));
        result.W = (PQuaternion1.W * PQuaternion2.W * w5) - ((PQuaternion1.X * w4) + (PQuaternion1.Y * w3) + (PQuaternion1.Z * w2));
        return result;
    }


    public static bool operator ==(PQuaternion PQuaternion1, PQuaternion PQuaternion2)
    {
        return PQuaternion1.X == PQuaternion2.X
            && PQuaternion1.Y == PQuaternion2.Y
            && PQuaternion1.Z == PQuaternion2.Z
            && PQuaternion1.W == PQuaternion2.W;
    }


    public static bool operator !=(PQuaternion PQuaternion1, PQuaternion PQuaternion2)
    {
        return PQuaternion1.X != PQuaternion2.X
            || PQuaternion1.Y != PQuaternion2.Y
            || PQuaternion1.Z != PQuaternion2.Z
            || PQuaternion1.W != PQuaternion2.W;
    }


    public static PQuaternion operator *(PQuaternion PQuaternion1, PQuaternion PQuaternion2)
    {
        PQuaternion result;
        double f12 = (PQuaternion1.Y * PQuaternion2.Z) - (PQuaternion1.Z * PQuaternion2.Y);
        double f11 = (PQuaternion1.Z * PQuaternion2.X) - (PQuaternion1.X * PQuaternion2.Z);
        double f10 = (PQuaternion1.X * PQuaternion2.Y) - (PQuaternion1.Y * PQuaternion2.X);
        double f9 = (PQuaternion1.X * PQuaternion2.X) + (PQuaternion1.Y * PQuaternion2.Y) + (PQuaternion1.Z * PQuaternion2.Z);
        result.X = (PQuaternion1.X * PQuaternion2.W) + (PQuaternion2.X * PQuaternion1.W) + f12;
        result.Y = (PQuaternion1.Y * PQuaternion2.W) + (PQuaternion2.Y * PQuaternion1.W) + f11;
        result.Z = (PQuaternion1.Z * PQuaternion2.W) + (PQuaternion2.Z * PQuaternion1.W) + f10;
        result.W = (PQuaternion1.W * PQuaternion2.W) - f9;
        return result;
    }

    public static PQuaternion QMulV(PQuaternion a, PVector3 b)
    {
        PQuaternion res;
        res.W = -a.X * b.X - a.Y * b.Y - a.Z * b.Z;
        res.X = a.W * b.X + a.Y * b.Z - a.Z * b.Y;
        res.Y = a.W * b.Y - a.X * b.Z + a.Z * b.X;
        res.Z = a.W * b.Z + a.X * b.Y - a.Y * b.X;
        return res;
    }


    public static PQuaternion operator *(PQuaternion PQuaternion1, double scaleFactor)
    {
        PQuaternion1.X *= scaleFactor;
        PQuaternion1.Y *= scaleFactor;
        PQuaternion1.Z *= scaleFactor;
        PQuaternion1.W *= scaleFactor;
        return PQuaternion1;
    }


    public static PQuaternion operator -(PQuaternion PQuaternion1, PQuaternion PQuaternion2)
    {
        PQuaternion1.X -= PQuaternion2.X;
        PQuaternion1.Y -= PQuaternion2.Y;
        PQuaternion1.Z -= PQuaternion2.Z;
        PQuaternion1.W -= PQuaternion2.W;
        return PQuaternion1;
    }


    public static PQuaternion operator -(PQuaternion PQuaternion)
    {
        PQuaternion.X = -PQuaternion.X;
        PQuaternion.Y = -PQuaternion.Y;
        PQuaternion.Z = -PQuaternion.Z;
        PQuaternion.W = -PQuaternion.W;
        return PQuaternion;
    }

    public override bool Equals(object obj)
    {
        return (obj is PQuaternion) ? this == (PQuaternion)obj : false;
    }

    public bool Equals(PQuaternion other)
    {
        if ((X == other.X) && (Y == other.Y) && (Z == other.Z))
            return W == other.W;
        return false;
    }
    public override int GetHashCode()
    {
        return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
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
        sb.Append(" W:");
        sb.Append(this.W);
        sb.Append("}");
        return sb.ToString();
    }

}