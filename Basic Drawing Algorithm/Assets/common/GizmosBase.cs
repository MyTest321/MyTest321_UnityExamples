using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GizmosBase : MonoBehaviour
{
    const int kDefaultFontSize = 20;
    static Color sDefaultColor = Color.white;

    [SerializeField]
    protected bool enable = true;

    protected abstract void OnMyDrawGizmos();

    void OnDrawGizmos() {
        MyDrawGizmos();
    }

    void MyDrawGizmos()
    {
        if (!enable) return;
        OnMyDrawGizmos();
    }

    protected void ScopedDrawLine(  in GameObject from,
                                    in GameObject to,
                                    in Color? scopedColor = null)
    {
        if (from == null || to == null) return;
        ScopedDrawLine(from.transform, to.transform, scopedColor);
    }

    protected void ScopedDrawLine(  in Transform from,
                                    in Transform to,
                                    in Color? scopedColor = null)
    {
        if (from == null || to == null) return;
        ScopedDrawLine(from.position, to.position, scopedColor);
    }

    protected void ScopedDrawLine(  in Vector3 from,
                                    in Vector3 to,
                                    in Color? scopedColor = null)
    {
        using (new ScopedValueGizmos(scopedColor??sDefaultColor))
        {
            GizmosEx.DrawLine(from, to);
        }
    }

    protected void ScopedDrawText(  in Vector3 worldPos,
                                    in string text,
                                    in Color? color = null,
                                    in int fontSize = kDefaultFontSize)
    {
        GUIEx.DrawText(worldPos, text, color??sDefaultColor, fontSize);
    }

    protected void ScopedDrawText(  in float sx,
                                    in float sy,
                                    in string text,
                                    in Color? color = null,
                                    in int fontSize = kDefaultFontSize)
    {
        GUIEx.DrawText(sx, sy, text, color??sDefaultColor, fontSize);
    }

    protected void ScopedDrawTextPoint( in Vector3 worldPos,
                                        in string text,
                                        in Color? color = null,
                                        in int fontSize = kDefaultFontSize,
                                        in float radius = 0.2f)
    {
        var c = color??sDefaultColor;
        ScopedDrawText(worldPos, text, c, fontSize);
        GizmosEx.DrawPoint(worldPos, c, radius);
    }
}
