using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Assumption:
    - PointToLineClosestDistance
    - Dot Product
    - Cross Product (Solution2)
*/
public class LineLineIntersect : GizmosBase {

    enum Demo
    {
        Solution1,
        Solution2,
        Solution3_TODO, // todo
    }

    [Header("Solution"), SerializeField]
    Demo demo = Demo.Solution1;

    [Header("Line1")]
    public Transform p1;
    public Transform p2;
    public Color line1Color = Color.red;

    [Header("Line2")]
    public Transform p3;
    public Transform p4;
    public Color line2Color = Color.red;

    [Header("Intersection")]
    Vector3 intersectPt;
    public Color intersectPtColor = Color.green;

    void Solution1()
    {
        if (p1 == null || p2 == null) return;
        if (p3 == null || p4 == null) return;

        var A = p1.position; var B = p2.position;
        var C = p3.position; var D = p4.position;

        Line3f line1 = new Line3f(A, B);
        Line3f line2 = new Line3f(C, D);

        Vector3 M;
        var isIntersectC = line1.GetClosestPoint(out M, C);
        var isIntersectD = !isIntersectC
            ? line1.GetClosestPoint(out M, D)
            : false;

        if (!isIntersectC && !isIntersectD) {
            var mid = (A + B + C + D) / 4.0f;
            ScopedDrawText(mid, "No Intersection");
            return;
        }

        Vector3 from, dir;
        if (isIntersectC) {
            from = C;
            dir = (D - C).normalized;
        } else {
            from = D;
            dir = (C - D).normalized;
        }
        var MFrom = M - from;
        var cosTheta = Vector3.Dot(MFrom.normalized, dir);
        var t = MFrom.magnitude / cosTheta;
        intersectPt = dir * t + from;

        if (!line1.IsPointOnSegmentLine(intersectPt) || !line2.IsPointOnSegmentLine(intersectPt)) {
            var mid = (A + B + C + D) / 4.0f;
            ScopedDrawText(mid, "No Intersection");
            return;
        }

        ScopedDrawTextPoint(intersectPt, "O", intersectPtColor);
    }

    void Solution2()
    {
        if (p1 == null || p2 == null) return;
        if (p3 == null || p4 == null) return;

        var A = p1.position; var B = p2.position;
        var C = p3.position; var D = p4.position;

        var BA = B - A;
        var DC = D - C;

        Line3f line1 = new Line3f(A, B);
        Line3f line2 = new Line3f(C, D);

        if (Vector3.Cross(BA, DC) == Vector3.zero)
        {
            // AB // CD, maybe so many intersect points, or maybe none.
            if (line1.IsPointOnSegmentLine(C)) {
                ScopedDrawTextPoint(C, "O", intersectPtColor); // just show single one intersect point for simple: C or D
            }
            if (line1.IsPointOnSegmentLine(D)) {
                ScopedDrawTextPoint(D, "O", intersectPtColor);
            }
            return;
        }

        var ABMagnitude = BA.magnitude;
        var CDMagnitude = DC.magnitude;

        // ||AB||*||CD||*sinTheta = ||AB.Cross(CD)||
        float sinTheta = Vector3.Cross(BA, DC).magnitude
                /
                (ABMagnitude * CDMagnitude);
#if false
#if false // use D as the from point
        var DA = D - A;
        float distFromDToAB = Vector3.Cross(BA, DA).magnitude / ABMagnitude;
        float t = distFromDToAB / sinTheta;

        var dir = (C - D).normalized;
        O = dir * t + D;
#else // use C as the from point
        var CA = C - A;
        float distFromCToAB = Vector3.Cross(BA, CA).magnitude / ABMagnitude;
        float t = distFromCToAB / sinTheta;

        var dir = DC.normalized;
        O = dir * t + C;
#endif
#endif
        Vector3 from, dir;
        from = C;
        dir = DC.normalized;

        var FromA = from - A;
        float distFromToAB = Vector3.Cross(BA, FromA).magnitude / ABMagnitude;
        float t = distFromToAB / sinTheta;
        intersectPt = dir * t + C;

        if (!line1.IsPointOnSegmentLine(intersectPt) || !line2.IsPointOnSegmentLine(intersectPt)) {
            return;
        }

        ScopedDrawTextPoint(intersectPt, "O", intersectPtColor);
    }

    void Solution3()
    {
        if (p1 == null || p2 == null) return;
        if (p3 == null || p4 == null) return;

        // Intersection point of two line segments in 2 dimensions: https://paulbourke.net/geometry/pointlineplane/

        var A = p1.position; var B = p1.position;
        var C = p1.position; var D = p1.position;
        
        // maybe no need, todo...
    }

    protected override void OnMyDrawGizmos()
    {
        ScopedDrawLine(p1, p2, line1Color);
        ScopedDrawLine(p3, p4, line2Color);

        switch (demo)
        {
            case Demo.Solution1: Solution1(); break;
            case Demo.Solution2: Solution2(); break;
            case Demo.Solution3_TODO: Solution3(); break; // todo
            default: break;
        }
    }
}
