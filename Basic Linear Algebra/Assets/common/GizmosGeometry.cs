using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GizmosDirInfo {
    public const float kLength = 5.0f;

    public Vector3 data;
    public float  length;
    public Color  color;

    public Vector3  Normalized   { get { return data.normalized; } }
    public float    Magnitude    { get { return data.magnitude; } }

    public GizmosDirInfo(in Line3f line) : this() {
        var dir = line.end - line.start;
        Set(dir);
        SetLength(dir.magnitude);
    }

    public static implicit operator Vector3(GizmosDirInfo v) { return v.data; }

    public void SetLength(in float length)                  { this.length = length; }
    public void SetColor(in Color color)                    { this.color = color; }
    public void Set(in float x, in float y, in float z)     { data.Set(x, y, z); }
    public void Set(in Vector3 dir)                         { data.Set(dir.x, dir.y, dir.z); }
    public void Set(in Quaternion q)                        { data.Set(q.x, q.y, q.z); }
}

[Serializable]
public class GizmosPlaneInfo {
    public const float kSize = 8.0f;
    public const float kPlaneThickness = 0.001f;

    public PlaneShapeType shapeType = PlaneShapeType.Circle;
    public ShadeType shadeType      = ShadeType.WireFrame;

    public GizmosDirInfo normal     = new GizmosDirInfo {
        color   = Color.green,
        length  = 8.0f,
    };
    public Color color = Color.white;
    public float size = kSize;

#if false
    public const uint  kWireFrameCircleSmoothCount = 5;
    static int sCirclePointCount = (int)Math.Pow(2, kWireFrameCircleSmoothCount + 1);
    static int sCircleLinePointCount = sCirclePointCount * 2;

    Vector3[] _circlePoints;
    Vector3[] _circleLinePoints;

    public GizmosPlaneInfo()
    {
        _circlePoints       = new Vector3[sCirclePointCount];
        _circleLinePoints   = new Vector3[sCircleLinePointCount];
        GenCirclePoints();
    }

    void GenCirclePoints()
    {
        float radStep = (2.0f*Mathf.PI) / sCirclePointCount;
        for (int i = 0; i < sCirclePointCount; ++i)
        {
            float rad = radStep * i;
            float y = Mathf.Sin(rad);
            float x = Mathf.Cos(rad);
            _circlePoints[i].Set(x, 0, y);
        }
    }

    void GenCircleLinePoints()
    {
        if (shadeType == ShadeType.WireFrame) {
            for (int from = 0, to = 1, i = 0; from < sCirclePointCount; ++from)
            {
                _circleLinePoints[i++] = _circlePoints[from];
                _circleLinePoints[i++] = _circlePoints[to];
                to = (to + 1) % sCirclePointCount;
            }
        }
        else
        {
            int from = sCirclePointCount / 2 / 2;
            int to = from;
            for (int k = 0, i = 0; k < sCirclePointCount; ++k)
            {
                _circleLinePoints[i++] = _circlePoints[from];
                _circleLinePoints[i++] = _circlePoints[to];
                from = (from + 1) % sCirclePointCount;
                to = (to - 1 + sCirclePointCount) % sCirclePointCount;
            }
        }
    }

    public ReadOnlySpan<Vector3> GetCircleLinePoints(in Vector3 pos) {

        GenCircleLinePoints();

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, normal.normalized);
        var trs = Matrix4x4.TRS(pos, rot, Vector3.one * size);

        for (int i = 0; i < sCircleLinePointCount; i++) {
            _circleLinePoints[i] = trs.MultiplyPoint(_circleLinePoints[i]);
        }
        return _circleLinePoints;
    }
#endif

}