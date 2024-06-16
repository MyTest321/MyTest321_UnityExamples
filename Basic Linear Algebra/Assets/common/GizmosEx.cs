using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosEx {

    const float kPlaneThickness = 0.001f;

    public static void DrawCube(in Vector3 worldPos,
                                in Vector3 size,
                                in ShadeType shadeType = ShadeType.Shaded)
    {
        switch (shadeType)
        {
            case ShadeType.WireFrame:   Gizmos.DrawWireCube(worldPos, size); break;
            case ShadeType.Shaded:      Gizmos.DrawCube(worldPos, size); break;
            default:                    throw new System.Exception("not supported");
        }
    }

    public static void DrawCube(in Matrix4x4 matrix,
                                in ShadeType shadeType = ShadeType.Shaded)
    {
        using (new ScopedValueGizmos(matrix))
        {
            DrawCube(Vector3.zero, Vector3.one, shadeType);
        }
    }

    public static void DrawSphere(Matrix4x4 matrix, in ShadeType shadeType = ShadeType.Shaded)
    {
        using (new ScopedValueGizmos(matrix))
        {
            DrawSphere(Vector3.zero, 1.0f, shadeType);
        }
    }

    public static void DrawSphere(  in Vector3 worldPos,
                                    in float radius,
                                    in ShadeType shadeType = ShadeType.Shaded)
    {
        switch (shadeType)
        {
            case ShadeType.WireFrame:   Gizmos.DrawWireSphere(worldPos, radius); break;
            case ShadeType.Shaded:      Gizmos.DrawSphere(worldPos, radius); break;
            default:                    throw new System.Exception("not supported");
        }
    }

    public static void DrawPoint(   in Vector3 worldPos,
                                    in Color color,
                                    in float radius = 0.1f)
    {
        using (new ScopedValueGizmos(color))
        {
            DrawSphere(worldPos, radius, ShadeType.Shaded);
        }
    }

    public static void DrawPoint(   Matrix4x4 matrix,
                                    in Color color,
                                    in float radius = 0.1f)
    {
        var scale = Matrix4x4.Scale(new Vector3(radius, radius, radius));
        matrix = matrix * scale;
        using (new ScopedValueGizmos(color))
        {
            Draw(matrix, ShapeType.Sphere, ShadeType.Shaded);
        }
    }

    public static void Draw(in Matrix4x4 matrix,
                            in ShapeType shapeType = ShapeType.Sphere,
                            in ShadeType shadeType = ShadeType.WireFrame)
    {
        using (new ScopedValueGizmos(matrix))
        {
            switch (shapeType)
            {
                case ShapeType.Sphere:  DrawSphere(Vector3.zero, 1.0f, shadeType); break;
                case ShapeType.Cube:    DrawCube(Vector3.zero, Vector3.one, shadeType); break;
                default:                throw new System.Exception("not supported");
            }
        }
    }

    public static void Draw(in Matrix4x4 matrix,
                            in Color color,
                            in ShapeType shapeType = ShapeType.Sphere,
                            in ShadeType shadeType = ShadeType.WireFrame)
    {
        using (new ScopedValueGizmos(color))
        {
            Draw(matrix, shapeType, shadeType);
        }
    }

    public static void DrawLine(in Vector3 from, in Vector3 to)
    {
        Gizmos.DrawLine(from, to);
    }

    public static void DrawLine(in Vector3 startWorldPos, in GizmosDirInfo info)
    {
        using (new ScopedValueGizmos(info.color))
        {
            Vector3 endWorldPos = startWorldPos + info.Normalized * info.length;
            DrawLine(startWorldPos, endWorldPos);
        }
    }

    public static void DrawRay(in Vector3 from, in Vector3 direction)
    {
        Gizmos.DrawLine(from, from + direction);
    }

    public static void DrawPlane(in Vector3 worldPos, in GizmosPlaneInfo info)
    {
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, info.normal.Normalized);
        Matrix4x4 matrix = Matrix4x4.TRS(worldPos, rot, Vector3.one);
        DrawPlane(matrix, info.color, info.size, info.shapeType, info.shadeType);
    }

    public static void DrawPlane(   Matrix4x4 matrix,
                                    in Color color,
                                    in float size = 1.0f,
                                    in PlaneShapeType shapeType = PlaneShapeType.Rect,
                                    in ShadeType shadeType = ShadeType.Shaded)
    {
        using (new ScopedValueGizmos(color))
        {
            var scale = Matrix4x4.Scale(new Vector3(size, kPlaneThickness, size));
            matrix = matrix * scale;
            if (shapeType == PlaneShapeType.Rect)
            {
                DrawCube(matrix, shadeType);
            }
            else if (shapeType == PlaneShapeType.Circle)
            {
                DrawSphere(matrix, shadeType);
            }
        }
    }
}

public struct ScopedValueGizmos : IDisposable
{
    private Color?      _color;
    private Matrix4x4?  _matrix;

    public ScopedValueGizmos(in Color newColor)
    {
        _color = Gizmos.color;
        Gizmos.color = newColor;
        _matrix = null;
    }

    public ScopedValueGizmos(in Matrix4x4 newMatrix)
    {
        _matrix = Gizmos.matrix;
        Gizmos.matrix = newMatrix;
        _color = null;
    }

    public ScopedValueGizmos(in Color newColor, in Matrix4x4 newMatrix)
    {
        _color          = Gizmos.color;
        _matrix         = Gizmos.matrix;
        Gizmos.color    = newColor;
        Gizmos.matrix   = newMatrix;
    }

    public void Dispose()
    {
        if (_color.HasValue) Gizmos.color = _color.Value;
        if (_matrix.HasValue) Gizmos.matrix = _matrix.Value;
    }
}