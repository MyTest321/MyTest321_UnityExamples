using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Assumption:
    - LineLineIntersect
*/
public class LinePlaneIntersect : GizmosBase {

    enum Demo
    {
        Solution1,
        Solution2,
    }

    [Header("Solution"), SerializeField]
    Demo demo = Demo.Solution1;

    [Header("Plane")]
    public Transform O;
    [Range(0, 100)]
    public float planeSize = 1f;
    public Color planeColor = new Color(255, 0, 0,100);

    [Header("Line")]
    public Transform p1;
    public Transform p2;
    public Color lineColor = Color.white;

    [Header("Intersection")]
    Vector3 intersectPt;
    public Color intersectPtColor = Color.green;

    void Solution1()
    {
        if (O == null || p1 == null || p2 == null) return;

        var A = p1.position;
        var B = p2.position;

        ScopedDrawLine(A, B, lineColor);

        Plane3f plane = new Plane3f(O.up, O.position);
//      ScopedDrawLine(O.position, O.position + O.up * 100, Color.blue);

        var distAToPlane = plane.Dot(A);
        var distBToPlane = plane.Dot(B);
        var C = A - (plane.normal * distAToPlane); // A project to plane
        var D = B - (plane.normal * distBToPlane); // B project to plane

        ScopedDrawTextPoint(C, "C");
        ScopedDrawTextPoint(D, "D");

        ScopedDrawLine(A, C);
        ScopedDrawLine(B, D);
        
        ScopedDrawLine(C, D, lineColor);

        if (MathGeometryUtil.GetLineLineIntersectPoint(out intersectPt, A, B, C, D))
        {
            ScopedDrawTextPoint(intersectPt, "O", intersectPtColor);
        } else
        {
            ScopedDrawTextPoint(O.position + O.up * 2, "No Intersection", intersectPtColor);
        }
    }

    void Solution2() // spec todo
    {

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
    }
}
