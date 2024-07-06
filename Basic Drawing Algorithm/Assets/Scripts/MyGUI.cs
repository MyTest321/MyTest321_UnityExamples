using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MyGUI : MonoBehaviour
{
	public enum MyEnum
    {
        ALL,
        Rect,
        Line,
        Circle,
        Triangle,
        BizierCurve,
    }
    public MyEnum myEnum = MyEnum.ALL;

    static MyGUI s_instance = null;

	public const int WIDTH  = 512;
    public const int HEIGHT = 512;

	public DrawRect[]			MyDrawRects;
    public DrawLine[]			MyDrawLines;
    public DrawCircle[]			MyDrawCircles;
    public DrawTriangle[]		MyDrawTriangles;
    public DrawBizierCurve[]	MyDrawBizierCurves;

    private Texture2D tex 	= null;
	private bool isRun 		= false;

	void Start()
	{
		if (!s_instance) s_instance = this;
		tex = new Texture2D(WIDTH, HEIGHT);
		isRun = true;
	}

	void Update()
    {
		if (!s_instance) s_instance = this;
		if (!isRun) return;
		if (!tex)
			tex = new Texture2D(WIDTH, HEIGHT);

		Clear();

        switch (myEnum)
        {
            case MyEnum.ALL:
                MyDrawRect();
                MyDrawLine();
                MyDrawCircle();
                MyDrawTriangle();
                MyDrawBizierCurve();
                break;
            case MyEnum.Rect:
                MyDrawRect();
                break;
            case MyEnum.Line:
                MyDrawLine();
                break;
            case MyEnum.Circle:
                MyDrawCircle();
                break;
            case MyEnum.Triangle:
                MyDrawTriangle();
                break;
            case MyEnum.BizierCurve:
                MyDrawBizierCurve();
                break;
            default:
                break;
        }

		Apply();
	}

	void Apply()
	{
		tex.Apply(false);
	}

	void MyDrawRect()
    {
        if (MyDrawRects == null) return;
        for (int i = 0; i < MyDrawRects.Length; i++)
        {
            if (!MyDrawRects[i]) continue;
            MyDrawRects[i].MyDraw(tex);
        }
    }

    void MyDrawLine()
    {
        if (MyDrawLines == null) return;
        for (int i = 0; i < MyDrawLines.Length; i++)
        {
            if (!MyDrawLines[i]) continue;
            MyDrawLines[i].MyDraw(tex);
        }
    }

    void MyDrawCircle()
    {
        if (MyDrawCircles == null) return;
        for (int i = 0; i < MyDrawCircles.Length; i++)
        {
            if (!MyDrawCircles[i]) continue;
            MyDrawCircles[i].MyDraw(tex);
        }
    }

    void MyDrawTriangle()
    {
        if (MyDrawTriangles == null) return;
        for (int i = 0; i < MyDrawTriangles.Length; i++)
        {
            if (!MyDrawTriangles[i]) continue;
            MyDrawTriangles[i].MyDraw(tex);
        }
    }

    void MyDrawBizierCurve()
    {
        if (MyDrawBizierCurves == null) return;
        for (int i = 0; i < MyDrawBizierCurves.Length; i++)
        {
            if (!MyDrawBizierCurves[i]) continue;
            MyDrawBizierCurves[i].MyDraw(tex);
        }
    }

    void Clear()
    {
        var c = new Color(0, 0, 0, 1);
        for (int i = 0; i < WIDTH; i++)
            for (int j = 0; j < HEIGHT; j++)
                tex.SetPixel(i, j, c);
        tex.Apply(false);
    }

    void OnGUI()
    {
		if (isRun) {
			if (tex == null) return;
			GUI.DrawTexture(new Rect(0, 0, WIDTH, HEIGHT), tex);
			return;
		}

		var oldColor		= GUI.color;
		var oldLabelSize	= GUI.skin.label.fontSize;
		var oldLabelAlign	= GUI.skin.label.alignment;

		GUI.color					= Color.red;
		GUI.skin.label.fontSize		= 60;
		GUI.skin.label.alignment	= TextAnchor.MiddleCenter;
			GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Ctrl + Shift + F5\nRun in Edit Mode");
		GUI.skin.label.alignment	= oldLabelAlign;
		GUI.skin.label.fontSize		= oldLabelSize;
		GUI.color					= oldColor;

	}

	private void OnApplicationQuit()
	{
		s_instance.isRun = false;
	}

	[UnityEditor.MenuItem("MyTest/Basic Drawing Algorithm %#F5")] // Ctrl + Shift + F5
	static void MemuItem()
	{
		if (!s_instance) return;
		s_instance.isRun = !s_instance.isRun;

		Debug.Log($"<color=#FFFF00>{(s_instance.isRun ? "Running" : "Stoped")}</color>");

		if (s_instance.isRun)
		{
			s_instance.Apply();
		} else
		{
			s_instance.Clear();
		}
	}
}
