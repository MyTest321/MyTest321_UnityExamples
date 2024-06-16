using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGizmos : GizmosBase {

    [Range(0.1f, 5f)]
    public float radius = 0.5f;

    public Color color = Color.red;

    protected override void OnMyDrawGizmos()
    {
        GizmosEx.DrawPoint(transform.localToWorldMatrix, color, radius);
    }
}
