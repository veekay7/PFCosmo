// Referenced from: http://wiki.unity3d.com/index.php/Mathfx
using UnityEngine;

public static class Interpolation
{
    // Ease in out
    public static float Hermite(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
    }

    public static Vector2 Hermite(Vector2 start, Vector2 end, float value)
    {
        return new Vector2(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value));
    }

    public static Vector3 Hermite(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value), Hermite(start.z, end.z, value));
    }


    // Ease out
    public static float Sinerp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
    }

    public static Vector2 Sinerp(Vector2 start, Vector2 end, float value)
    {
        return new Vector2(
            Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)), 
            Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)));
    }

    public static Vector3 Sinerp(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(
            Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)), 
            Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)), 
            Mathf.Lerp(start.z, end.z, Mathf.Sin(value * Mathf.PI * 0.5f)));
    }


    // Ease in
    public static float Coserp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
    }

    public static Vector2 Coserp(Vector2 start, Vector2 end, float value)
    {
        return new Vector2(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value));
    }

    public static Vector3 Coserp(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value), Coserp(start.z, end.z, value));
    }
     

    // Boing
    public static float Berp(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (
            Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * 
            Mathf.Pow(1f - value, 2.2f) + value) * 
            (1f + (1.2f * (1f - value)));
        return start + (end - start) * value;
    }

    public static Vector2 Berp(Vector2 start, Vector2 end, float value)
    {
        return new Vector2(Berp(start.x, end.x, value), Berp(start.y, end.y, value));
    }

    public static Vector3 Berp(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(Berp(start.x, end.x, value), Berp(start.y, end.y, value), Berp(start.z, end.z, value));
    }


    // Like lerp with ease in ease out
    public static float SmoothStep(float x, float min, float max)
    {
        x = Mathf.Clamp(x, min, max);
        float v1 = (x - min) / (max - min);
        float v2 = (x - min) / (max - min);
        return -2 * v1 * v1 * v1 + 3 * v2 * v2;
    }

    public static Vector2 SmoothStep(Vector2 vec, float min, float max)
    {
        return new Vector2(SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max));
    }

    public static Vector3 SmoothStep(Vector3 vec, float min, float max)
    {
        return new Vector3(SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max), SmoothStep(vec.z, min, max));
    }

    public static float Lerp(float start, float end, float value)
    {
        return ((1.0f - value) * start) + (value * end);
    }


    public static float Cosine(float a, float b, float t)
    {
        float t2 = (1.0f - Mathf.Cos(t * Mathf.PI)) / 2.0f;
        return (a * (1.0f - t2) + b * t2);
    }


    public static Vector2 Cosine(Vector2 a, Vector2 b, float t)
    {
        Vector2 output = Vector2.zero;
        output.x = Cosine(a.x, b.x, t);
        output.y = Cosine(a.y, b.y, t);
        return output;
    }


    public static Vector3 Cosine(Vector3 a, Vector3 b, float t)
    {
        Vector3 output = Vector3.zero;
        output.x = Cosine(a.x, b.x, t);
        output.y = Cosine(a.y, b.y, t);
        output.z = Cosine(a.z, b.z, t);
        return output;
    }


    public static float Cubic(float y0, float y1, float y2, float y3, float t)
    {
        float t2 = t * t;
        float a0 = y3 - y2 - y0 + y1;
        float a1 = y0 - y1 - a0;
        float a2 = y2 - y0;
        float a3 = y1;

        return (a0 * t * t2 + a1 * t2 + a2 * t + a3);
    }


    public static float CatmullRom(float y0, float y1, float y2, float y3, float t)
    {
        float t2 = t * t;
        float a0 = -0.5f * y0 + 1.5f * y1 - 1.5f * y2 + 0.5f * y3;
        float a1 = y0 - 2.5f * y1 + 2.0f * y2 - 0.5f * y3;
        float a2 = -0.5f * y0 + 0.5f * y2;
        float a3 = y1;

        return (a0 * t * t2 + a1 * t2 + a2 * t + a3);
    }
}
