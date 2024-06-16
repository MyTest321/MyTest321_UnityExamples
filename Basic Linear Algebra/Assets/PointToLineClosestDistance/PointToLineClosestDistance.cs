using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Point to line(Ray/Segment) shortest distance

/*
Assumption:
    -Dot Product
*/
public class PointToLineClosestDistance : GizmosBase {

    enum Demo
    {
        Solution1,
        Solution2,
    }

    [Header("Solution"), SerializeField]
    Demo demo = Demo.Solution1;

    [Header("Line")]
    public Transform p1;
    public Transform p2;
    public Color lineColor = Color.white;

    public Transform p;

    Vector3 O; // intersect point (hit point)
    public Color shortestLineColor = Color.green;

    struct OutputInfo // just for gizmos
    {
        public bool isOnSegmentLine;

        public Vector3 targetPt;

        public Vector3 closestPt; // intersect point (hit point)
        public Vector3 notOnSegmentLinePt;
    }
    OutputInfo outPutInfo;

    void Solution1()
    {
        if (p1 == null || p2 == null) return;

        var A = p1.position;
        var B = p2.position;

        if (p == null) return;
        var C = p.position;

        var BA = (B - A).normalized;
        var t = Vector3.Dot(C - A, BA);

        O = BA * t + A;

        bool isOnSegmentLine = MathGeometryUtil.IsPointOnSegmentLine(O, A, B);
        if (!isOnSegmentLine) {
            outPutInfo.notOnSegmentLinePt = O;
            O = t < 0 ? A : B;
        } 

        outPutInfo.targetPt = C;
        outPutInfo.closestPt = O;
        outPutInfo.isOnSegmentLine = isOnSegmentLine;
    }

    void Solution2()
    {
        // Minimum Distance between a 2D Point and a 2D Line : http://paulbourke.net/geometry/pointlineplane/
        // CM.dot(BA) = 0

        if (p1 == null || p2 == null) return;
        var A = p1.position;
        var B = p2.position;

        if (p == null) return;
        var C = p.position;

        var BA = B - A;

        var x1 = A.x; var y1 = A.y; var z1 = A.z;
        var x2 = B.x; var y2 = B.y; var z2 = B.z;
        var x3 = C.x; var y3 = C.y; var z3 = C.z;

        var t = ((x3-x1)*(x2-x1) + (y3-y1)*(y2-y1) + (z3-z1)*(z2-z1))
                /
                BA.sqrMagnitude;

        O = BA*t + A;

        bool isOnSegmentLine = MathGeometryUtil.IsPointOnSegmentLine(O, A, B);
        if (!isOnSegmentLine)
        {
            outPutInfo.notOnSegmentLinePt = O;
            O = t < 0 ? A : B;
        }

        outPutInfo.targetPt = C;
        outPutInfo.closestPt = O;
        outPutInfo.isOnSegmentLine = isOnSegmentLine;
    }

    void DrawOutputGizmos()
    {
        ScopedDrawTextPoint(outPutInfo.closestPt, "O", shortestLineColor);
        if (!outPutInfo.isOnSegmentLine)
        {
            ScopedDrawTextPoint(outPutInfo.notOnSegmentLinePt, "O'", shortestLineColor);
            ScopedDrawLine(outPutInfo.targetPt, outPutInfo.notOnSegmentLinePt, Color.white);
            ScopedDrawLine(outPutInfo.closestPt, outPutInfo.notOnSegmentLinePt, Color.white);
        }
        ScopedDrawLine(outPutInfo.targetPt, outPutInfo.closestPt, shortestLineColor);

        var dist = Vector3.Distance(outPutInfo.targetPt, outPutInfo.closestPt);
        ScopedDrawText(50, 10, $"Dist:{dist}");
    }

    protected override void OnMyDrawGizmos()
    {
        ScopedDrawLine(p1, p2, lineColor);

        switch (demo)
        {
            case Demo.Solution1: Solution1(); break;
            case Demo.Solution2: Solution2(); break;
            default: break;
        }

        DrawOutputGizmos();
    }
}
