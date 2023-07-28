using UnityEngine;


public static class QuaternionExt
{
    public static Quaternion GetNormalized(this Quaternion q)
    {
        float num = 1f / Mathf.Sqrt((float)((double)q.x * (double)q.x + (double)q.y * (double)q.y +
                                            (double)q.z * (double)q.z + (double)q.w * (double)q.w));
        return new Quaternion(q.x * num, q.y * num, q.z * num, q.w * num);
    }
}