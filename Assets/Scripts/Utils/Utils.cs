using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SorcererRush;
using UnityEngine;
using CompressionLevel = System.IO.Compression.CompressionLevel;

public static class Utils
{
    private static string m_saveDataOverride = (string)null;
    public static string persistantDataPath = Application.persistentDataPath;
    private static Plane[] mainPlanes;
    private static int lastPlaneFrame = -1;
    private static int lastFrameCheck = 0;
    private static Camera lastMainCamera = (Camera)null;


    public static void SetSaveDataPath(string path) => Utils.m_saveDataOverride = path;

    public static void ResetSaveDataPath() => Utils.m_saveDataOverride = (string)null;

    public static string GetPrefabName(GameObject gameObject)
    {
        string name = gameObject.name;
        char[] anyOf = new char[2] { '(', ' ' };
        int startIndex = name.IndexOfAny(anyOf);
        return startIndex != -1 ? name.Remove(startIndex) : name;
    }

    public static bool InsideMainCamera(Bounds bounds)
    {
        Plane[] cameraFrustumPlanes = Utils.GetMainCameraFrustumPlanes();
        return cameraFrustumPlanes != null && GeometryUtility.TestPlanesAABB(cameraFrustumPlanes, bounds);
    }

    public static bool InsideMainCamera(BoundingSphere bounds)
    {
        Plane[] cameraFrustumPlanes = Utils.GetMainCameraFrustumPlanes();
        if (cameraFrustumPlanes == null)
            return false;
        for (int index = 0; index < cameraFrustumPlanes.Length; ++index)
        {
            if ((double)cameraFrustumPlanes[index].GetDistanceToPoint(bounds.position) < -(double)bounds.radius)
                return false;
        }

        return true;
    }

    public static Plane[] GetMainCameraFrustumPlanes()
    {
        Camera mainCamera = Utils.GetMainCamera();
        if (!(bool)(UnityEngine.Object)mainCamera)
            return (Plane[])null;
        if (Time.frameCount != Utils.lastPlaneFrame)
        {
            Utils.mainPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            Utils.lastPlaneFrame = Time.frameCount;
        }

        return Utils.mainPlanes;
    }

    public static Camera GetMainCamera()
    {
        int frameCount = Time.frameCount;
        if (Utils.lastFrameCheck == frameCount)
            return Utils.lastMainCamera;
        Utils.lastMainCamera = Camera.main;
        Utils.lastFrameCheck = frameCount;
        return Utils.lastMainCamera;
    }

    public static Color Vec3ToColor(Vector3 c) => new Color(c.x, c.y, c.z);

    public static Vector3 ColorToVec3(Color c) => new Vector3(c.r, c.g, c.b);

    public static float LerpStep(float l, float h, float v) =>
        Mathf.Clamp01((float)(((double)v - (double)l) / ((double)h - (double)l)));

    public static float SmoothStep(float p_Min, float p_Max, float p_X)
    {
        float num = Mathf.Clamp01((float)(((double)p_X - (double)p_Min) / ((double)p_Max - (double)p_Min)));
        return (float)((double)num * (double)num * (3.0 - 2.0 * (double)num));
    }

    public static double LerpStep(double l, double h, double v) => Utils.Clamp01((v - l) / (h - l));

    public static double Clamp01(double v)
    {
        if (v > 1.0)
            return 1.0;
        return v < 0.0 ? 0.0 : v;
    }

    public static float Fbm(Vector3 p, int octaves, float lacunarity, float gain) =>
        Utils.Fbm(new Vector2(p.x, p.z), octaves, lacunarity, gain);

    public static float FbmMaxValue(int octaves, float gain)
    {
        float num1 = 0.0f;
        float num2 = 1f;
        for (int index = 0; index < octaves; ++index)
        {
            num1 += num2;
            num2 *= gain;
        }

        return num1;
    }

    public static float Fbm(Vector2 p, int octaves, float lacunarity, float gain)
    {
        float num1 = 0.0f;
        float num2 = 1f;
        Vector2 vector2 = p;
        for (int index = 0; index < octaves; ++index)
        {
            num1 += num2 * Mathf.PerlinNoise(vector2.x, vector2.y);
            num2 *= gain;
            vector2 *= lacunarity;
        }

        return num1;
    }

    public static Quaternion SmoothDamp(
        Quaternion rot,
        Quaternion target,
        ref Quaternion deriv,
        float smoothTime,
        float maxSpeed,
        float deltaTime)
    {
        float num1 = (double)Quaternion.Dot(rot, target) > 0.0 ? 1f : -1f;
        target.x *= num1;
        target.y *= num1;
        target.z *= num1;
        target.w *= num1;
        Vector4 normalized = new Vector4(
            Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, smoothTime, maxSpeed, deltaTime),
            Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, smoothTime, maxSpeed, deltaTime),
            Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, smoothTime, maxSpeed, deltaTime),
            Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, smoothTime, maxSpeed, deltaTime)).normalized;
        float num2 = 1f / deltaTime;
        deriv.x = (normalized.x - rot.x) * num2;
        deriv.y = (normalized.y - rot.y) * num2;
        deriv.z = (normalized.z - rot.z) * num2;
        deriv.w = (normalized.w - rot.w) * num2;
        return new Quaternion(normalized.x, normalized.y, normalized.z, normalized.w);
    }

    public static long GenerateUID()
    {
        IPGlobalProperties globalProperties = IPGlobalProperties.GetIPGlobalProperties();
        return (long)((globalProperties == null || globalProperties.HostName == null
                          ? "unkown"
                          : globalProperties.HostName) + ":" +
                      (globalProperties == null || globalProperties.DomainName == null
                          ? "domain"
                          : globalProperties.DomainName))
            .GetHashCode() + (long)UnityEngine.Random.Range(1, int.MaxValue);
    }

    public static bool TestPointInViewFrustum(Camera camera, Vector3 worldPos)
    {
        Vector3 viewportPoint = camera.WorldToViewportPoint(worldPos);
        return (double)viewportPoint.x >= 0.0 && (double)viewportPoint.x <= 1.0 && (double)viewportPoint.y >= 0.0 &&
               (double)viewportPoint.y <= 1.0;
    }

    public static Vector3 ParseVector3(string rString)
    {
        string[] strArray = rString.Substring(1, rString.Length - 2).Split(',');
        return new Vector3(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]));
    }

    public static int GetMinPow2(int val)
    {
        if (val <= 1)
            return 1;
        if (val <= 2)
            return 2;
        if (val <= 4)
            return 4;
        if (val <= 8)
            return 8;
        if (val <= 16)
            return 16;
        if (val <= 32)
            return 32;
        if (val <= 64)
            return 64;
        if (val <= 128)
            return 128;
        if (val <= 256)
            return 256;
        if (val <= 512)
            return 512;
        if (val <= 1024)
            return 1024;
        if (val <= 2048)
            return 2048;
        return val <= 4096 ? 4096 : 1;
    }

    public static void NormalizeQuaternion(ref Quaternion q)
    {
        float f = 0.0f;
        for (int index = 0; index < 4; ++index)
            f += q[index] * q[index];
        float num = 1f / Mathf.Sqrt(f);
        for (int index = 0; index < 4; ++index)
            q[index] *= num;
    }

    public static Vector3 Project(Vector3 v, Vector3 onTo)
    {
        float num = Vector3.Dot(onTo, v);
        return onTo * num;
    }

    public static float Length(float x, float y) => Mathf.Sqrt((float)((double)x * (double)x + (double)y * (double)y));

    public static float DistanceSqr(Vector3 v0, Vector3 v1)
    {
        double num1 = (double)v1.x - (double)v0.x;
        float num2 = v1.y - v0.y;
        float num3 = v1.z - v0.z;
        return (float)(num1 * num1 + (double)num2 * (double)num2 + (double)num3 * (double)num3);
    }

    public static float DistanceXZ(Vector3 v0, Vector3 v1)
    {
        double num1 = (double)v1.x - (double)v0.x;
        float num2 = v1.z - v0.z;
        return Mathf.Sqrt((float)(num1 * num1 + (double)num2 * (double)num2));
    }

    public static float LengthXZ(Vector3 v) =>
        Mathf.Sqrt((float)((double)v.x * (double)v.x + (double)v.z * (double)v.z));

    public static Vector3 DirectionXZ(Vector3 dir)
    {
        dir.y = 0.0f;
        dir.Normalize();
        return dir;
    }

    public static Vector3 Bezier2(Vector3 Start, Vector3 Control, Vector3 End, float delta) =>
        (float)((1.0 - (double)delta) * (1.0 - (double)delta)) * Start +
        (float)(2.0 * (double)delta * (1.0 - (double)delta)) * Control + delta * delta * End;

    public static float FixDegAngle(float p_Angle)
    {
        while ((double)p_Angle >= 360.0)
            p_Angle -= 360f;
        while ((double)p_Angle < 0.0)
            p_Angle += 360f;
        return p_Angle;
    }

    public static float DegDistance(float p_a, float p_b)
    {
        if ((double)p_a == (double)p_b)
            return 0.0f;
        p_a = Utils.FixDegAngle(p_a);
        p_b = Utils.FixDegAngle(p_b);
        float num = Mathf.Abs(p_b - p_a);
        if ((double)num > 180.0)
            num = Mathf.Abs(num - 360f);
        return num;
    }

    public static float GetYawDeltaAngle(Quaternion q1, Quaternion q2) =>
        Mathf.DeltaAngle(q1.eulerAngles.y, q2.eulerAngles.y);

    public static float YawFromDirection(Vector3 dir) => Utils.FixDegAngle(57.29578f * Mathf.Atan2(dir.x, dir.z));

    public static float DegDirection(float p_a, float p_b)
    {
        if ((double)p_a == (double)p_b)
            return 0.0f;
        p_a = Utils.FixDegAngle(p_a);
        p_b = Utils.FixDegAngle(p_b);
        double f = (double)p_a - (double)p_b;
        float num = f > 0.0 ? 1f : -1f;
        if ((double)Mathf.Abs((float)f) > 180.0)
            num *= -1f;
        return num;
    }

    public static void RotateBodyTo(Rigidbody body, Quaternion rot, float alpha)
    {
    }

    public static bool IsEnabledInheirarcy(GameObject go, GameObject root)
    {
        while (go.activeSelf)
        {
            if ((UnityEngine.Object)go == (UnityEngine.Object)root)
                return true;
            go = go.transform.parent.gameObject;
            if (!((UnityEngine.Object)go != (UnityEngine.Object)null))
                return true;
        }

        return false;
    }

    public static bool IsParent(Transform go, Transform parent)
    {
        while (!((UnityEngine.Object)go == (UnityEngine.Object)parent))
        {
            go = go.parent;
            if (!((UnityEngine.Object)go != (UnityEngine.Object)null))
                return false;
        }

        return true;
    }

    public static Transform FindChild(Transform aParent, string aName)
    {
        foreach (Transform aParent1 in aParent)
        {
            if (aParent1.name == aName)
                return aParent1;
            Transform child = Utils.FindChild(aParent1, aName);
            if ((UnityEngine.Object)child != (UnityEngine.Object)null)
                return child;
        }

        return (Transform)null;
    }

    public static void AddToLodgroup(LODGroup lg, GameObject toAdd)
    {
        List<Renderer> rendererList = new List<Renderer>((IEnumerable<Renderer>)lg.GetLODs()[0].renderers);
        Renderer[] componentsInChildren = toAdd.GetComponentsInChildren<Renderer>();
        rendererList.AddRange((IEnumerable<Renderer>)componentsInChildren);
        lg.GetLODs()[0].renderers = rendererList.ToArray();
    }

    public static void RemoveFromLodgroup(LODGroup lg, GameObject toRemove)
    {
        List<Renderer> rendererList = new List<Renderer>((IEnumerable<Renderer>)lg.GetLODs()[0].renderers);
        foreach (Renderer componentsInChild in toRemove.GetComponentsInChildren<Renderer>())
            rendererList.Remove(componentsInChild);
        lg.GetLODs()[0].renderers = rendererList.ToArray();
    }

    public static void DrawGizmoCircle(Vector3 center, float radius, int steps)
    {
        float num = 6.283185f / (float)steps;
        Vector3 to = center + new Vector3(Mathf.Cos(0.0f) * radius, 0.0f, Mathf.Sin(0.0f) * radius);
        Vector3 vector3 = to;
        for (float f = num; (double)f <= 6.28318548202515; f += num)
        {
            Vector3 from = center + new Vector3(Mathf.Cos(f) * radius, 0.0f, Mathf.Sin(f) * radius);
            Gizmos.DrawLine(from, vector3);
            vector3 = from;
        }

        Gizmos.DrawLine(vector3, to);
    }

    public static void ClampUIToScreen(RectTransform transform)
    {
        Vector3[] fourCornersArray = new Vector3[4];
        transform.GetWorldCorners(fourCornersArray);
        if ((UnityEngine.Object)Utils.GetMainCamera() == (UnityEngine.Object)null)
            return;
        float num1 = 0.0f;
        float num2 = 0.0f;
        if ((double)fourCornersArray[2].x > (double)Screen.width)
            num1 -= fourCornersArray[2].x - (float)Screen.width;
        if ((double)fourCornersArray[0].x < 0.0)
            num1 -= fourCornersArray[0].x;
        if ((double)fourCornersArray[2].y > (double)Screen.height)
            num2 -= fourCornersArray[2].y - (float)Screen.height;
        if ((double)fourCornersArray[0].y < 0.0)
            num2 -= fourCornersArray[0].y;
        Vector3 position = transform.position;
        position.x += num1;
        position.y += num2;
        transform.position = position;
    }

    public static float Pull(
        Rigidbody body,
        Vector3 target,
        float targetDistance,
        float speed,
        float force,
        float smoothDistance,
        bool noUpForce = false,
        bool useForce = false,
        float power = 1f)
    {
        Vector3 vector3_1 = target - body.position;
        float magnitude = vector3_1.magnitude;
        if ((double)magnitude < (double)targetDistance)
            return 0.0f;
        Vector3 normalized = vector3_1.normalized;
        float num = (float)Math.Pow((double)Mathf.Clamp01((magnitude - targetDistance) / smoothDistance),
            (double)power);
        Vector3 vector3_2 = Vector3.Project(body.velocity, normalized.normalized);
        Vector3 vector3_3 = normalized.normalized * speed - vector3_2;
        if (noUpForce && (double)vector3_3.y > 0.0)
            vector3_3.y = 0.0f;
        ForceMode mode = useForce ? ForceMode.Impulse : ForceMode.VelocityChange;
        Vector3 force1 = vector3_3 * num * Mathf.Clamp01(force);
        body.AddForce(force1, mode);
        return num;
    }

    public static byte[] Compress(byte[] inputArray)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (GZipStream gzipStream = new GZipStream((Stream)memoryStream, (CompressionLevel)1))
                gzipStream.Write(inputArray, 0, inputArray.Length);
            return memoryStream.ToArray();
        }
    }

    public static byte[] Decompress(byte[] inputArray)
    {
        using (MemoryStream memoryStream = new MemoryStream(inputArray))
        {
            using (GZipStream gzipStream = new GZipStream((Stream)memoryStream, CompressionMode.Decompress))
            {
                using (MemoryStream destination = new MemoryStream())
                {
                    gzipStream.CopyTo((Stream)destination);
                    return destination.ToArray();
                }
            }
        }
    }

    public static string GetPath(this Transform obj) => (UnityEngine.Object)obj.parent == (UnityEngine.Object)null
        ? "/" + obj.name
        : obj.parent.GetPath() + "/" + obj.name;

    public static int CompareFloats(float a, float b)
    {
        if ((double)a > (double)b)
            return 1;
        return (double)a >= (double)b ? 0 : -1;
    }

    public static void IterateHierarchy(
        GameObject gameObject,
        Utils.ChildHandler childHandler,
        bool deepestFirst = false)
    {
        foreach (Transform transform in gameObject.transform)
        {
            if (!deepestFirst)
                childHandler(transform.gameObject);
            Utils.IterateHierarchy(transform.gameObject, childHandler);
            if (deepestFirst)
                childHandler(transform.gameObject);
        }
    }

    public static Vector3 RotatePointAroundPivot(
        Vector3 point,
        Vector3 pivot,
        Quaternion rotation)
    {
        Vector3 vector3 = point - pivot;
        point = rotation * vector3 + pivot;
        return point;
    }

    public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        object[] customAttributes =
            enumVal.GetType().GetMember(enumVal.ToString())[0].GetCustomAttributes(typeof(T), false);
        return customAttributes.Length == 0 ? default(T) : (T)customAttributes[0];
    }

    public static Quaternion RotateTorwardsSmooth(
        Quaternion from,
        Quaternion to,
        Quaternion last,
        float maxDegreesDelta,
        float acceleration = 1.2f,
        float deacceleration = 0.05f,
        float minDegreesDelta = 0.005f)
    {
        if (last == new Quaternion())
            last = from;
        Vector3 b = from * Vector3.forward * 100f;
        float num1 = Vector3.Distance(last * Vector3.forward * 100f, b);
        float num2 = Mathf.Clamp(Vector3.Distance(to * Vector3.forward * 100f, b) * deacceleration, minDegreesDelta,
            1f);
        float num3 = (double)num2 < 1.0 ? num2 : Mathf.Clamp(num1 * acceleration, minDegreesDelta, 1f);
        return Quaternion.RotateTowards(from, to, maxDegreesDelta * num3);
    }

    public static void IncrementOrSet<T>(this Dictionary<T, int> dict, T key, int amountToAdd = 1)
    {
        int num;
        if (dict.TryGetValue(key, out num))
            dict[key] = num + amountToAdd;
        else
            dict[key] = amountToAdd;
    }

    public static void IncrementOrSet<T>(this Dictionary<T, float> dict, T key, float amountToAdd = 1f)
    {
        float num;
        if (dict.TryGetValue(key, out num))
            dict[key] = num + amountToAdd;
        else
            dict[key] = amountToAdd;
    }

    public static void Write(this BinaryWriter bw, Vector3 value)
    {
        bw.Write(value.x);
        bw.Write(value.y);
        bw.Write(value.z);
    }

    public static Vector3 ReadVector3(this BinaryReader bw) =>
        new Vector3(bw.ReadSingle(), bw.ReadSingle(), bw.ReadSingle());

    public static void Write(this BinaryWriter bw, Quaternion value) => bw.Write(value.eulerAngles);

    public static Quaternion ReadQuaternion(this BinaryReader bw) => Quaternion.Euler(bw.ReadVector3());

    public static bool SignDiffers(float a, float b) => ((int)Utils.IntFloat.Convert(a) & int.MinValue) !=
                                                        ((int)Utils.IntFloat.Convert(b) & int.MinValue);

    [MethodImpl((MethodImplOptions)256)]
    public static float Sign(float f) =>
        Utils.IntFloat.Convert((uint)(1065353216 | (int)Utils.IntFloat.Convert(f) & int.MinValue));

    [MethodImpl((MethodImplOptions)256)]
    public static float Abs(float f) => Utils.IntFloat.Convert(Utils.IntFloat.Convert(f) & (uint)int.MaxValue);

    public static int Clamp(int num, int min, int max) => Math.Max(min, Math.Min(num, max));

    public static short ClampToShort(this int num) =>
        (short)Math.Max((int)short.MinValue, Math.Min(num, (int)short.MaxValue));


    public static bool CustomEndsWith(this string a, string b)
    {
        int index1 = a.Length - 1;
        int index2;
        for (index2 = b.Length - 1; index1 >= 0 && index2 >= 0 && (int)a[index1] == (int)b[index2]; --index2)
            --index1;
        return index2 < 0;
    }

    public static bool CustomStartsWith(this string a, string b)
    {
        int length1 = a.Length;
        int length2 = b.Length;
        int index1 = 0;
        int index2;
        for (index2 = 0; index1 < length1 && index2 < length2 && (int)a[index1] == (int)b[index2]; ++index2)
            ++index1;
        return index2 == length2;
    }

    public delegate void ChildHandler(GameObject go);

    [StructLayout(LayoutKind.Explicit)]
    private struct IntFloat
    {
        [FieldOffset(0)] private float f;
        [FieldOffset(0)] private uint i;

        public static uint Convert(float value) => new Utils.IntFloat()
        {
            f = value
        }.i;

        public static float Convert(uint value) => new Utils.IntFloat()
        {
            i = value
        }.f;
    }

    public static void Heightlight(IHighlightable highlightable, Color color)
    {
        Coroutines.Start(Heightlight(highlightable.GetRenderers(), color));
    }

    private static IEnumerator Heightlight(Renderer[] renderers, Color color)
    {
        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                if (material.HasProperty("_EmissionColor"))
                    material.SetColor("_EmissionColor", color * 0.7f);
                material.color = color;
            }
        }

        yield return new WaitForSeconds(0.5f);
        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                if (material.HasProperty("_EmissionColor"))
                    material.SetColor("_EmissionColor", Color.white * 0f);
                material.color = Color.white;
            }
        }
    }

    public static T Nearest<T>(List<T> all, Vector3 nearestTo) where T : MonoBehaviour
    {
        T current = default(T);
        float oldDistance = int.MaxValue;
        if (all == null || all.Count == 0) return current;
        foreach (T pos_ in all)
        {
            var pos = (pos_ as MonoBehaviour).transform.position;
            float dist = Utils.DistanceXZ(nearestTo, pos);
            if (dist < oldDistance)
            {
                current = pos_;
                oldDistance = dist;
            }
        }

        return current;
    }
}