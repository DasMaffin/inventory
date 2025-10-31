using UnityEngine;

public static class ExtendedMathf
{
    public static int ClampWrap(int value, int min, int max)
    {
        int range = max - min;
        if (range == 0f) return min;
        value = (value - min) % range;
        if (value < 0) value += range;
        return value + min;
    }

    public static float ClampWrap(float value, float min, float max)
    {
        float range = max - min;
        if (range == 0f) return min;
        value = (value - min) % range;
        if (value < 0) value += range;
        return value + min;
    }
}
