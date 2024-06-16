using System;
using UnityEngine;

public enum ShapeType
{
    Sphere,
    Cube,
}

public enum ShadeType
{
    WireFrame,
    Shaded,
}

public enum PlaneShapeType
{
    Rect,
    Circle,
}

public struct Triangle3f
{
    public Vector3 v0, v1, v2;

    public Triangle3f(in Vector3 v0, in Vector3 v1, in Vector3 v2)
    {
        this.v0 = v0;
        this.v1 = v1;
        this.v2 = v2;
    }

    public bool GetIntersectPoint(out Vector3 outPt, Line3f line) {
		var plane = new Plane3f(Normal(), v0);
		var ok = plane.GetIntersectPoint(out outPt, line);

		if (ok && IsPtInside(outPt))
			return true;

		outPt = Vector3.zero;
		return false;
    }

    public Vector3 Normal() {
        var v1v0 = v1 - v0;
        var v2v0 = v2 - v0;
        return Vector3.Cross(v1v0, v2v0);
    }

    public bool IsPtInside(Vector3 pt) {
        return s_SameSide(pt, v0, v1, v2) && s_SameSide(pt, v1, v0, v2) && s_SameSide(pt, v2, v0, v1);
    }

	private static bool s_SameSide(Vector3 p1, Vector3 p2, Vector3 a, Vector3 b)
    {
        var cp1 = Vector3.Cross(b - a, p1 - a);
        var cp2 = Vector3.Cross(b - a, p2 - a);
        return Vector3.Dot(cp1, cp2) >= 0;
    }
}

public struct Plane3f
{
    public Vector3  normal;
    public float    distance; // distance from origin, instead of "public Vector3 worldPos;"

    public Plane3f(in Vector3 normal, in Vector3 point)
    {
        this.normal = normal;
        this.distance = Vector3.Dot(normal, point);
    }

    public void Set(in Vector3 normal, in float distance)
    {
        this.normal   = normal;
        this.distance = distance;
    }

    public void SetByTriangle(in Vector3 v0, in Vector3 v1, in Vector3 v2)
    {
        var v1v0 = v1 - v0;
        var v2v0 = v2 - v0;
        this.normal = Vector3.Cross(v1v0, v2v0).normalized;
        this.distance = Vector3.Dot(normal, v0);
    }

    public float Dot(in Vector3 point) {
        return Vector3.Dot(normal, point) - distance;
    }

    public bool GetIntersectPoint(out Vector3 outPt, Line3f line)
    {
        ref Vector3 A = ref line.start;
        ref Vector3 B = ref line.end;

        var distAToPlane = Dot(A);
        var distBToPlane = Dot(B);

        Vector3 C = A - (normal * distAToPlane);
        Vector3 D = B - (normal * distBToPlane);

        var projectedLine = new Line3f(C, D);
        return line.GetIntersectPoint(out outPt, projectedLine);
    }
}

public struct Line2f {
    public Vector2 start;
    public Vector2 end;

    public Line2f(in Vector2 start, in Vector2 end)
    {
        this.start = start;
        this.end   = end;
    }

    public bool IsPointOnSegmentLine(in Vector2 pt)
    {
        var min = Vector2.Min(start, end);
        var max = Vector2.Max(start, end);
        if (pt.x != Mathf.Clamp(pt.x, min.x, max.x)) return false;
        if (pt.y != Mathf.Clamp(pt.y, min.y, max.y)) return false;
        return true;
    }

    public bool GetClosestPoint(out Vector2 outPoint, in Vector2 inPoint)
    {
        var BA = (end - start).normalized;
        var t = Vector2.Dot(inPoint - start, BA);

        outPoint = BA * t + start;
        bool res = IsPointOnSegmentLine(outPoint);
        if (!res) {
            outPoint = t < 0 ? start : end;
        }
        return res;
    }

    public float DistanceToPoint(in Vector2 pt)
    {
        Vector2 o;
        GetClosestPoint(out o, pt);
        return Vector2.Distance(o, pt);
    }
}

public struct Line3f
{
    public Vector3 start;
    public Vector3 end;

    public Line3f(in Vector3 start, in Vector3 end) {
        this.start = start;
        this.end   = end;
    }

    public bool IsPointOnSegmentLine(in Vector3 pt)
    {
        var min = Vector3.Min(start, end);
        var max = Vector3.Max(start, end);
        if (pt.x != Mathf.Clamp(pt.x, min.x, max.x)) return false;
        if (pt.y != Mathf.Clamp(pt.y, min.y, max.y)) return false;
        if (pt.z != Mathf.Clamp(pt.z, min.z, max.z)) return false;
        var cosTheta = Vector3.Dot((end - start).normalized, (pt - start).normalized);
        return MyCommon.Equals(cosTheta, 1);
    }

    public bool GetClosestPoint(out Vector3 outPt, in Vector3 inPoint) {
        var BA = (end - start).normalized;
        var t = Vector3.Dot(inPoint - start, BA);

        outPt = BA * t + start;
        bool res = IsPointOnSegmentLine(outPt);
        if (!res) {
            outPt = t < 0 ? start : end;
        }
        return res;
    }

    public float DistanceToPoint(in Vector3 pt)
    {
        Vector3 o;
        GetClosestPoint(out o, pt);
        return Vector3.Distance(o, pt);
    }

    public bool GetIntersectPoint(out Vector3 outPt, Line3f rhs)
    {
        ref Vector3 C = ref rhs.start;
        ref Vector3 D = ref rhs.end;

        Vector3 M;
        var isIntersectC = GetClosestPoint(out M, C);
        var isIntersectD = !isIntersectC
            ? GetClosestPoint(out M, D)
            : false;

        if (!isIntersectC && !isIntersectD) {
            outPt = Vector3.zero;
            return false;
        }

        Vector3 from, dir;
        if (isIntersectC)
        {
            from = C;
            dir = (D - C).normalized;
        }
        else
        {
            from = D;
            dir = (C - D).normalized;
        }
        var MFrom = M - from;
        var cosTheta = Vector3.Dot(MFrom.normalized, dir);
        var t = MFrom.magnitude / cosTheta;
        outPt = dir * t + from;

        if (!IsPointOnSegmentLine(outPt) || !rhs.IsPointOnSegmentLine(outPt)) {
            outPt = Vector3.zero;
            return false;
        }
        return true;
    }
}

public static class MathGeometryUtil
{
    public static bool IsPointOnSegmentLine(in Vector3 pt, in Vector3 start, in Vector3 end)
    {
        Line3f line = new Line3f(start, end);
        return line.IsPointOnSegmentLine(pt);
    }

    public static bool IsPointOnSegmentLine(in Vector2 pt, in Vector2 start, in Vector2 end)
    {
        Line2f line = new Line2f(start, end);
        return line.IsPointOnSegmentLine(pt);
    }

    public static bool GetLineLineIntersectPoint(out Vector3 outPt, in Vector3 A, in Vector3 B, in Vector3 C, in Vector3 D)
    {
        Line3f line1 = new Line3f(A, B);
        Line3f line2 = new Line3f(C, D);
        return line1.GetIntersectPoint(out outPt, line2);
    }
}