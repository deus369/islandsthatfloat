using UnityEngine;

//! Some helper functions to interpolate between Quaternions and angle functions.
public static class QuaternionHelpers
{
    public static Quaternion Multiply(Quaternion q2, float mult)
    {
        return new Quaternion(mult * q2.x, mult * q2.y, mult * q2.z, mult * q2.w);
    }
    public static Quaternion Add(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.x + q2.x, q1.y+ q2.y, q1.z + q2.z, q1.w + q2.w);
    }

    public static Quaternion slerp(Quaternion q1, Quaternion q2, float t)
    {
        float dt = Quaternion.Dot(q1, q2);
        if (dt < 0.0f)
        {
            dt = -dt;
            q2 = Multiply(q2, -1f); // new Quaternion(-q2.x, -q2.y, -q2.z, -q2.w); // -q2;
        }
        if (dt < 0.9995f)
        {
            float angle = Mathf.Acos(dt);
            float s = 1f/ Mathf.Sqrt(1.0f - dt * dt);    // 1.0f / sin(angle)
            float w1 = Mathf.Sin(angle * (1.0f - t)) * s;
            float w2 = Mathf.Sin(angle * t) * s;
            return Add(Multiply(q1, w1), Multiply(q2, w2)); // q1 * w1 + q2 * w2;
        }
        else
        {
            // if the angle is small, use linear interpolation
            return nlerp(q1, q2, t);
        }
    }

    public static Quaternion nlerp(Quaternion q1, Quaternion q2, float t)
    {
        float dt = Quaternion.Dot(q1, q2);
        if (dt < 0.0f)
        {
            q2 = Multiply(q2, -1f);
        }
        return Quaternion.Normalize(Quaternion.Lerp(q1, q2, t));
    }

    public static Quaternion slerpSafe(Quaternion q1, Quaternion q2, float t)
    {
        float dt = Quaternion.Dot(q1, q2);
        if (dt < 0.0f)
        {
            dt = -dt;
            q2 = Multiply(q2, -1f);
        }

        if (dt < 0.9995f)
        {
            float angle = Mathf.Acos(dt);
            float s = 1f / Mathf.Sqrt(1.0f - dt * dt);    // 1.0f / sin(angle)
            float w1 = Mathf.Sin(angle * (1.0f - t)) * s;
            float w2 = Mathf.Sin(angle * t) * s;
            // return Mathf.Quaternion(q1 * w1 + q2 * w2);
            return Add(Multiply(q1, w1), Multiply(q2, w2)); // q1 * w1 + q2 * w2;
        }
        else
        {
            // if the angle is small, use linear interpolation
            return nlerpSafe(q1, q2, t);
        }
    }

    public static Quaternion nlerpSafe(Quaternion q1, Quaternion q2, float t)
    {
        float dt = Quaternion.Dot(q1, q2);
        if (dt < 0.0f)
        {
            q2 = Multiply(q2, -1f);
        }
        return Quaternion.Normalize(Quaternion.Lerp(q1, q2, t));
    }
}