using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBizierCurve : MonoBehaviour
{
    public MyVector2[] Points;
    public MyVector2[] tangentDir;
    public Color color;

    [Range(0.1f, 1f)]
    public float errDistance = 0.25f;

    public enum BizierCurveType
    {
        Quad_th_BizierCurve,
        MyNth_BizierCurve,
        Nth_BizierCurve,
        Cubic_BizierCurve,
    }

    public BizierCurveType type;

	public bool isShowControlLine = true;
	public Color controlLinecolor = Color.white;

	public bool isShowControlPt = true;
	[Range(1, 6)] public int controlPtSize = 3;
	public Color controlPtcolor = Color.red;

	[Range(0, 1)] public float normalized_t = 1;

	public void MyDraw(Texture2D tex)
    {
        if (Points == null || Points.Length < 1) return;

        switch (type)
        {
            case BizierCurveType.Quad_th_BizierCurve:
                Quad_th_BizierCurve(tex);
                break;
            case BizierCurveType.Cubic_BizierCurve:
				p0 = new Vector3(Points[0].x, Points[0].y, 0);
				p1 = new Vector3(Points[1].x, Points[1].y, 0);
				p2 = new Vector3(Points[2].x, Points[2].y, 0);
				p3 = new Vector3(Points[3].x, Points[3].y, 0);
				Cubic_BizierCurve(tex);
                break;
            case BizierCurveType.MyNth_BizierCurve:
                MyNth_BizierCurve(tex);
                break;
            case BizierCurveType.Nth_BizierCurve:
                Nth_BizierCurve(tex);
                break;
            default:
                break;
        }

		ShowControls(tex);
    }

	void ShowControls(Texture2D tex) {
		if (!isShowControlPt && !isShowControlLine)
			return;

        int n = Points.Length;

		if (isShowControlPt)
        	MyCommon.DrawPoint(tex, (int)Points[0].x, (int)Points[0].y, controlPtcolor, controlPtSize);

        for (int i = 0, j = 1; j < n; i++, j++)
        {
            var pre = Points[i];
            var cur = Points[j];
            int x0 = (int)pre.x, y0 = (int)pre.y;
            int x1 = (int)cur.x, y1 = (int)cur.y;

			if (isShowControlLine)
            	DrawLine.Draw(tex, x0, y0, x1, y1, controlLinecolor, DrawLine.LineType.Bresenham);

			if (isShowControlPt)
            	MyCommon.DrawPoint(tex, x1, y1, controlPtcolor, controlPtSize);
        }
	}

    // https://www.jasondavies.com/animated-bezier/
    void Quad_th_BizierCurve(Texture2D tex, float t = 0)
    {
        /* t 在[0, 1]
         * n = 2 -> B1(t) = P0 + (P1 - p0)t
         * n = 3 -> B2(t) = (1-t)²*P0 + (1-t)*t*P1 + t²*P2
        */
        if (Points == null || Points.Length < 3) return;
        if (t >= 1) return;

        MyVector2 p1 = Points[0];
        MyVector2 p2 = Points[1];
        MyVector2 p3 = Points[2];

        int x1 = (int)p1.x, y1 = (int)p1.y;
        int x2 = (int)p2.x, y2 = (int)p2.y;
        int x3 = (int)p3.x, y3 = (int)p3.y;
        DrawLine.Draw(tex, x1, y1, x2, y2, Color.white, DrawLine.LineType.Bresenham);
        DrawLine.Draw(tex, x2, y2, x3, y3, Color.white, DrawLine.LineType.Bresenham);
        MyCommon.DrawPoint(tex, x1, y1, Color.red);
        MyCommon.DrawPoint(tex, x2, y2, Color.green);
        MyCommon.DrawPoint(tex, x3, y3, Color.blue);

        // p' = (1-t)*p0 + t*p1
        var pa = (1 - t) * p1 + t * p2;
        var pb = (1 - t) * p2 + t * p3;
        var o = (1 - t) * pa + t * pb;
        tex.SetPixel((int)o.x, (int)o.y, Color.red);
        Quad_th_BizierCurve(tex, t + 0.01f);
    }

    MyVector2 Get_B(MyVector2[] p, int n, float t)
    {
        if (n == 1) return p[0];
        MyVector2[] backup = new MyVector2[n - 1];
        for (int i = 0; i < n - 1; i ++) {
            backup[i] = (1 - t) *p[i] + t * p[i + 1];
        }
        return Get_B(backup, n - 1, t);
    }

    void MyNth_BizierCurve(Texture2D tex)
    {
		int N = 100;
		for (int i = 0; i <= N; i ++) {
			float t = (float)i / N;
			if (t >= 1 || t >= normalized_t) return;
			MyNth_BizierCurve_inner(tex, t);
		}
    }

	void MyNth_BizierCurve_inner(Texture2D tex, float t)
	{
		int n = Points.Length;
		var o = Get_B(Points, n, t);
		tex.SetPixel((int)o.x, (int)o.y, color);
	}

	// https://en.wikipedia.org/wiki/B%C3%A9zier_curve#Constructing_B.C3.A9zier_curves
	void Nth_BizierCurve(Texture2D tex)
    {
        if (Points == null || Points.Length < 1) return;
        
        int n = Points.Length;
		int N = 100;
		for (int c = 0; c < N; c ++)
        {
            float t = (float)c / N;
            if (t >= normalized_t) return;

            MyVector2 o = new MyVector2(0, 0);
            // C[a][b] combination count: choose b from a
            float[][] C = new float[n+1][];
            for (int i = 0; i <= n; i++)
            {
                C[i] = new float[n + 1];
                for (int j = 0; j <= i; j++)
                {
                    if (j == 0) C[i][j] = 1;
                    else C[i][j] = C[i - 1][j - 1] + C[i - 1][j];
                }
            }

            for (int i = 0; i < n; i ++)
            {
                var pi = Points[i];
                var k = C[n-1][i] * Mathf.Pow(t, i) * Mathf.Pow(1 - t, n - 1 - i);
                o += k * pi;
            }
            tex.SetPixel((int)o.x, (int)o.y, color);
        }
    }

	public Vector3 p0;
	public Vector3 p1;
	public Vector3 p2;
	public Vector3 p3;
	public float tol = 0.1f;

	public int pointCount = 0;

	Vector3 eval(float t)
	{
		float it = 1 - t;
		float it2 = it * it;
		float t2 = t * t;

		Vector3 o = it2 * it * p0
				  + 3f * it2 * t * p1
				  + 3f * it * t2 * p2
				  + t * t2 * p3;
		return o;
	}

	public Vector3 offset;
	List<Vector3> points = new List<Vector3>();

	static public float GetDistPointToLine(Vector3 origin, Vector3 direction, Vector3 point)
	{
		Vector3 point2origin = origin - point;
		Vector3 point2closestPointOnLine = point2origin - Vector3.Dot(point2origin, direction) * direction;
		return point2closestPointOnLine.magnitude;
	}

	void genPoints(Vector3 a, Vector3 b, float t, int lv)
	{
		if (lv > 10) return;
		var e = eval(t);
		var dis = GetDistPointToLine(a, (b - a).normalized, e);
		if (dis < tol)
		{
			points.Add(b);
			return;
		}

		float step = 1.0f / (1 << lv);
		genPoints(a, e, t - step, lv + 1);
		genPoints(e, b, t + step, lv + 1);
	}

	// http://blog.sklambert.com/finding-the-control-points-of-a-bezier-curve/
	void Cubic_BizierCurve(Texture2D tex)
    {
#if false
		MyCommon.DrawPoint(tex, (int)p0.x, (int)p0.y, Color.red);
		MyCommon.DrawPoint(tex, (int)p1.x, (int)p1.y, Color.red);
		MyCommon.DrawPoint(tex, (int)p2.x, (int)p2.y, Color.red);
		MyCommon.DrawPoint(tex, (int)p3.x, (int)p3.y, Color.red);
#endif
		{
			points.Clear();
			genPoints(p0, p3, 0.5f, 2);
			Vector3 last = p0 + offset;
			for (int i = 1; i < points.Count; i++)
			{
				var p = points[i] + offset;
				DrawLine.Draw(tex, (int)last.x, (int)last.y, (int)p.x, (int)p.y, Color.white, DrawLine.LineType.Bresenham);
				last = p;
			}

			pointCount = points.Count;
		}
#if false
		for (int i = 0; i < pointCount; ++i) {
			tex.SetPixel((int)points[i].x, (int)points[i].y, color);
		}
#endif
    }
}
