using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGizmos : GizmosBase {
    [SerializeField] protected ShapeType _shapeType    = ShapeType.Sphere;
    [SerializeField] protected ShadeType _shadeType    = ShadeType.WireFrame;
    [SerializeField] protected Color _color            = Color.red;

    protected override void OnMyDrawGizmos()
    {
        GizmosEx.Draw(transform.localToWorldMatrix, _color, _shapeType, _shadeType);
    }
}
