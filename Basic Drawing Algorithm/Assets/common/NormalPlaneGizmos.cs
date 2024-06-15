using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlaneGizmos : GizmosBase {

    [SerializeField]
    protected Vector3 _pos = Vector3.zero;

    [SerializeField]
    GizmosPlaneInfo _plane;

    public ref ShadeType ShadeType                  { get { return ref _plane.shadeType; } }
    public ref GizmosDirInfo Normal                 { get { return ref _plane.normal; } }
    public ref Vector3 Pos                          { get { return ref _pos; } }
    public ref float Size                           { get { return ref _plane.size; } }
    public ref GizmosPlaneInfo Plane                { get { return ref _plane; } }

    public void setShadeType(ShadeType shadeType)   { ShadeType = shadeType; }
    public void setNormal(Vector3 normal)           { Normal.Set(normal); }
    public void setNormalColor(Color color)         { Normal.SetColor(color); }
    public void setNormalLength(float length)       { Normal.SetLength(length); }
    public void setPos(Vector3 pos)                 { _pos = pos; }
    public void setPlaneColor(Color color)          { _plane.color = color; }
    public void setSize(float size)                 { Size = size; }

    protected override void OnMyDrawGizmos()
    {
        GizmosEx.DrawLine(_pos, Normal);
        GizmosEx.DrawPlane(_pos, _plane);
    }
}
