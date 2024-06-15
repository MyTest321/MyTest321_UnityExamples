using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYZAxisGizmos : GizmosBase {
    [SerializeField] bool _enableX = true;
    [SerializeField] bool _enableY = true;
    [SerializeField] bool _enableZ = true;

    [SerializeField, HideInInspector]
    GizmosDirInfo x = new GizmosDirInfo { color = Color.red, length = 5.0f };
    [SerializeField, HideInInspector]
    GizmosDirInfo y = new GizmosDirInfo { color = Color.green, length = 5.0f };
    [SerializeField, HideInInspector]
    GizmosDirInfo z = new GizmosDirInfo { color = Color.blue, length = 5.0f };

    protected override void OnMyDrawGizmos()
    {
        var pos = transform.position;
#if false
        x.Set(transform.right);
        y.Set(transform.up);
        z.Set(transform.forward);
#else // or use quaternion
        var q = transform.localRotation;
        x.Set(q * Vector3.right); // equals to transform.right
        y.Set(q * Vector3.up);
        z.Set(q * Vector3.forward);
#endif
        if (_enableX) GizmosEx.DrawLine(pos, x);
        if (_enableY) GizmosEx.DrawLine(pos, y);
        if (_enableZ) GizmosEx.DrawLine(pos, z);
    }
}
