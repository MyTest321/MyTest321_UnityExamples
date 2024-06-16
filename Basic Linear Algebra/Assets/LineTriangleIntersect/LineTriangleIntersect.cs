using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Assumption:
    - LinePlaneIntersect
    - PointInTriangle(Same Side Technique): https://blackpawn.com/texts/pointinpoly/
*/
public class LineTriangleIntersect : GizmosBase
{
    enum Demo
    {
        Solution1,
    }
    [Header("Solution"), SerializeField]
    Demo demo = Demo.Solution1;

    [Header("Triangle")]
    public Transform t1;
    public Transform t2;
    public Transform t3;
    public Color triangleColor = Color.white;

    [Header("Line")]
    public Transform p1;
    public Transform p2;
    public Color lineColor = Color.white;

    [Header("Intersection")]
    public Color intersectColor = Color.green;

    bool SameSide(Vector3 p1, Vector3 p2, Vector3 a, Vector3 b)
    {
        var cp1 = Vector3.Cross(b - a, p1 - a);
        var cp2 = Vector3.Cross(b - a, p2 - a);
        return Vector3.Dot(cp1, cp2) >= 0;
    }

    bool PointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        return SameSide(p, a, b, c) && SameSide(p, b, a, c) && SameSide(p, c, a, b);
    }

    void Solution1()
    {
        if (p1 == null || p2 == null) return;
        if (t1 == null || t2 == null || t3 == null) return;

        var V0 = t1.position;
        var V1 = t2.position;
        var V2 = t3.position;

        var A = p1.position;
        var B = p2.position;

        var plane = new Plane3f();
        plane.SetByTriangle(V0, V1, V2);

        var line = new Line3f(A, B);

        Vector3 intersectPt;
        if (plane.GetIntersectPoint(out intersectPt, line) && PointInTriangle(intersectPt, V0, V1, V2))
        {
            ScopedDrawTextPoint(intersectPt, "O", intersectColor);
        } else
        {
            var mid = (V0 + V1 + V2) / 3.0f;
            ScopedDrawText(mid, "No Intersection");
        }
    }

    protected override void OnMyDrawGizmos()
    {
        ScopedDrawLine(p1, p2, lineColor);
        if (t1 == null || t2 == null || t3 == null) return;
        ScopedDrawLine(t1, t2, triangleColor);
        ScopedDrawLine(t1, t3, triangleColor);
        ScopedDrawLine(t2, t3, triangleColor);

        switch (demo)
        {
            case Demo.Solution1: Solution1(); break;
            default: break;
        }
    }
}
