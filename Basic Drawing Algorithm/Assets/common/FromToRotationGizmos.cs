using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromToRotationGizmos : NormalPlaneGizmos
{
    const int kCurvePointCount = 120;

    [SerializeField]
    GizmosDirInfo _from;
    [SerializeField]
    GizmosDirInfo _to;
    [SerializeField]
    Color _angleColor = Color.red;

    Quaternion  _q;
    Vector3     _axis;
    [SerializeField]
    float _angle = 0;

    Vector3[] _curvePoints = new Vector3[kCurvePointCount];

    public ref GizmosDirInfo From   { get { return ref _from; } }
    public ref GizmosDirInfo To     { get { return ref _to; } }

    void OnDrawAngleGizmos()
    {
        if (_curvePoints == null) return;
        if (_curvePoints.Length != kCurvePointCount) return;

        var p1 = _pos + _from.Normalized;
        var p2 = _pos + _to.Normalized;
        var tan1 = Vector3.Cross(Normal, _from).normalized;
        var tan2 = Vector3.Cross(Normal, _to).normalized;

        float n = kCurvePointCount - 1;
        for (int i = 0; i < kCurvePointCount; i++)
        {
            float t = i / n;
            _curvePoints[i] = MyCommon.HermiteCurveLerp(t, p1, p2, tan1, tan2);
        }
        Gizmos.color = _angleColor;
        //Gizmos.DrawLineList(_curvePoints);
    }

    void OnDrawFromToDir()
    {
        Gizmos.DrawLine(_pos, _from);
        Gizmos.DrawLine(_pos, _to);
    }

    void OnDrawNormalPlaneGizmos()
    {
        setNormal(_axis.normalized);
        base.OnMyDrawGizmos();
    }

    protected override void OnMyDrawGizmos()
    {
        _q.SetFromToRotation(_from, _to);
        _q.Normalize();
        _q.ToAngleAxis(out _angle, out _axis);

        if (_angle != 0)
        {
            OnDrawNormalPlaneGizmos();
            OnDrawAngleGizmos();
        }
        OnDrawFromToDir();
    }
}
