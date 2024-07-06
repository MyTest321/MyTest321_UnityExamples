using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    public MyVector2 center;
    public float radius;
    public Color circleColor;
    public Color outLineCircle;

    public enum CircleType
    {
        Circle,
        CircleOutLine,
        CircleAndOutLine,
    }

    public CircleType type;

    public void MyDraw(Texture2D tex)
    {
        switch (type)
        {
            case CircleType.Circle:
                MyDrawCircle(tex);
                break;
            case CircleType.CircleOutLine:
                MyDrawCircleOutLine(tex);
                break;
            case CircleType.CircleAndOutLine:
                MyDrawCircle(tex);
                MyDrawCircleOutLine(tex);
                break;
            default:
                break;
        }
    }

    void MyDrawCircle(Texture2D tex)
    {
        // circle = semicircle * 2
        int x0 = (int)center.x;
        int y0 = (int)center.y;
        int r = (int)radius;

        for (int y = 0; y <= r; y ++)
        {
            int dx = (int)Mathf.Sqrt(r * r - y * y);
            for (int x = -dx; x <= dx; x++)
            {
                tex.SetPixel(x0 + x, y0 + y, circleColor); // lower semicircle
                tex.SetPixel(x0 + x, y0 - y, circleColor); // upper semicircle
            }
        }

        // Circle's origin
        tex.SetPixel(x0, y0, Color.red);
        tex.SetPixel(x0 + 1, y0, Color.red);
        tex.SetPixel(x0, y0 + 1, Color.red);
        tex.SetPixel(x0 + 1, y0 + 1, Color.red);
    }

    void MyDrawCircleOutLine(Texture2D tex)
    {
        int x0 = (int)center.x;
        int y0 = (int)center.y;
        int r  = (int)radius;

        // Circle's origin
        tex.SetPixel(x0, y0, Color.red);
        tex.SetPixel(x0 + 1, y0, Color.red);
        tex.SetPixel(x0, y0 + 1, Color.red);
        tex.SetPixel(x0+1, y0 + 1, Color.red);

        for (int dx = 0, dy = r; ;) // start draw from top
        {
            // first quadrant: 1/8 circle (upper side)
            tex.SetPixel(x0 - dx, y0 + dy, outLineCircle); // Left Top
            tex.SetPixel(x0 + dx, y0 + dy, outLineCircle); // Right Top
            tex.SetPixel(x0 - dx, y0 - dy, outLineCircle); // Left Bottom
            tex.SetPixel(x0 + dx, y0 - dy, outLineCircle); // Right Bottom

            // first quadrant 1/8 circle (lower side) symmetry axis of y=x
            tex.SetPixel(x0 + dy, y0 - dx, outLineCircle); // Left Top
            tex.SetPixel(x0 + dy, y0 + dx, outLineCircle); // Right Top
            tex.SetPixel(x0 - dy, y0 - dx, outLineCircle); // Left Bottom
            tex.SetPixel(x0 - dy, y0 + dx, outLineCircle); // Right Bottom

            int new_dx = dx + 1;
            int new_dy = dy - 1;
            float a = new_dx * new_dx + dy * dy;         // step right
            float b = dx * dx + new_dy * new_dy;         // step down
            float c = new_dx * new_dx + new_dy * new_dy; // step right&down

            a = Mathf.Abs(Mathf.Sqrt(a) - r);
            b = Mathf.Abs(Mathf.Sqrt(b) - r);
            c = Mathf.Abs(Mathf.Sqrt(c) - r);
            if (a < b && a < c) // move right close to r
            {
                dx = new_dx;
            }
            else if (b < a && b < c) // move down close to r
            {
                dy = new_dy;
            }
            else // move right&down close to r
            {
                dx = new_dx;
                dy = new_dy;
            }
            if (dx > dy) break;
        }
    }
}
