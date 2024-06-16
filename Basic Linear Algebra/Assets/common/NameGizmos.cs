using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGizmos : GizmosBase {

    [Range(20, 50)]
    public int fontSize = 25;
    public Color color = Color.white;

    protected override void OnMyDrawGizmos() {
        ScopedDrawText(transform.position, gameObject.name, color, fontSize);
    }
}
