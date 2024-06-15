using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class MyCommon
{
    public static Vector3 HermiteCurveLerp( float t,
                                            Vector3 p1,
                                            Vector3 p2,
                                            Vector3 tan1,
                                            Vector3 tan2)
    {
        float tt = t * t;
        float tt2 = tt * 2;
        float tt3 = tt * 3;
        float ttt = tt * t;
        float ttt2 = ttt * 2;
        return p1 * (ttt2 - tt3 + 1) + tan1 * (ttt - tt2 + t) + p2 * (-ttt2 + tt3) + tan2 * (ttt - tt);
    }

    public static ulong NextPow2(ulong x)
    {
        x -= 1;
        x |= (x >> 1);
        x |= (x >> 2);
        x |= (x >> 4);
        x |= (x >> 8);
        x |= (x >> 16);
        x |= (x >> 32);
        return x + 1;
    }

    public static float Degrees(float radians) { return radians * Mathf.Rad2Deg; }
    public static float Radians(float degrees) { return degrees * Mathf.Deg2Rad; }

    public static bool Equals(float a, float b, float? epsilon = null)
    {
        return Mathf.Abs(a - b) <= (epsilon??Epsilon<float>.Value);
    }

    public static bool Equals(double a, double b, double? epsilon = null)
    {
        return Math.Abs(a - b) <= (epsilon??Epsilon<double>.Value);
    }
}

public struct Epsilon<T> {
    public static T Value { get; private set; }

    static Epsilon() {
        Epsilon<float>.Value  = 1.192092896e-07F;
        Epsilon<double>.Value = 2.2204460492503131e-016;
    }
}

#if ENABLE_UNSAFE
public sealed unsafe class ScopedValue<T> : IDisposable
    where T : unmanaged
{
    private readonly T _oldValue;
    unsafe T* _p = null;

    public unsafe ScopedValue(T* p, in T newValue)
    {
        _oldValue = *p;
        _p = p;

        *p = newValue;
    }

    public unsafe void Dispose()
    {
        if (_p != null)
        {
            *_p = _oldValue;
            _p = null;
        }
    }
/*
 * example:
            int test = 10;
            unsafe
            {
                using (new ScopedValue<int>(&test, 100))
                {
                    Debug.Log($"test={test}");
                }
            }
            Debug.Log($"test={test}");
*/
}
#else
public sealed class ScopedValue<T> : IDisposable
{
    readonly T _oldValue;
    Action<T> _disposeFunc;

    public ScopedValue(ref T p, T newValue, Action<T> disposeFunc)
    {
        _oldValue = p;
        _disposeFunc = disposeFunc;
        p = newValue;
    }

    public void Dispose()
    {
        if (_disposeFunc != null)
        {
            _disposeFunc(_oldValue);
            _disposeFunc = null;
        }
    }
/*
    * example:
        int test = 10;
        using (new ScopedValue<int>(ref test, 100, (o) => { test = o; })) {
            Debug.Log($"test1={test}");
        }
        Debug.Log($"test2 ={test}");
*/
}
#endif