using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGizmos : GizmosBase {

    [Range(0.1f, 50f)]
    public float radius = 10f;
    public Color color = Color.red;

    protected override void OnMyDrawGizmos()
    {
        GizmosEx.DrawPlane(transform.localToWorldMatrix, color, radius);
    }
}
