using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSphereIntersect : GizmosBase {

    enum Demo
    {
        Solution1,
    }

    [Header("Solution"), SerializeField]
    Demo demo = Demo.Solution1;

    public GameObject O, A, B;
    [Range(0.1f, 100f)] public float radius;
    public Color SphereColor;
    public Color resultPointColor;

    void Solution1()
    {
        if (O == null || A == null || B == null)
			return;

        var o = O.transform.position;
        var a = A.transform.position;
        var b = B.transform.position;

        var ab = (b - a).normalized;
        var d = Vector3.Dot(ab, o - a);
        var m = a + d * ab;
        var distance2 = (o - m).sqrMagnitude;
        var u2 = radius * radius - distance2;
        var u = Mathf.Sqrt(u2);
        var b2Sphere = m + ab * u;
        var a2Sphere = m - ab * u;

        // output
        Gizmos.color = resultPointColor;
        Gizmos.DrawSphere(a2Sphere, 0.1f);
        Gizmos.DrawSphere(b2Sphere, 0.1f);

		ScopedDrawText(a2Sphere, $"a'{a2Sphere}", Color.white, 15);
		ScopedDrawText(b2Sphere, $"b'{b2Sphere}", Color.white, 15);
		ScopedDrawText((m + a2Sphere) / 2, $"d={u}", Color.white, 15);

        // debug
        Gizmos.color = Color.white;

		ScopedDrawText(o, $"{O.name}{o}", Color.white, 15);
		ScopedDrawText(a, $"{A.name}{a}", Color.white, 15);
		ScopedDrawText(b, $"{B.name}{b}", Color.white, 15);
		ScopedDrawText(m, $"m{m}", Color.white, 15);

		ScopedDrawLine(a, b, Color.white);
		GizmosEx.DrawPoint(m, Color.yellow, 0.2f);
		ScopedDrawLine(o, m, Color.red);

		var distOM = Mathf.Sqrt(distance2);
		bool isIntersect = distOM <= radius;
		ScopedDrawText(10, 10, $"OM={distOM}" + (isIntersect ? "(Intersected)": "(No Intersection)"), Color.white, 30);

        {
            Gizmos.color = SphereColor;
            Gizmos.matrix = O.transform.localToWorldMatrix;
            Gizmos.DrawSphere(Vector3.zero, radius);
        }
    }

    protected override void OnMyDrawGizmos()
    {
        switch (demo)
        {
            case Demo.Solution1: Solution1(); break;
            default: break;
        }
    }
}
