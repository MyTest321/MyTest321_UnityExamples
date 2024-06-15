using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationGizmos : GizmosBase {

    [SerializeField]
    float size = 10.0f;
    [SerializeField]
    bool isShowLocalRot = false;
    [SerializeField]
    bool isShowGlobalRot = true;

    [SerializeField]
    Color localColor    = Color.red;
    [SerializeField]
    Color globalColor   = Color.green;
    
    [SerializeField]
    bool enableForward  = true;
    [SerializeField]
    bool enableUp       = false;
    [SerializeField]
    bool enableRight    = false;
    [SerializeField]
    bool enableAxis     = false;

    void OnDrawGizmosDir(Vector3 dir)
    {
        if (isShowLocalRot)
        {
            Gizmos.color = localColor;
            Gizmos.DrawLine(transform.position, transform.position + dir.normalized * size);
        }

        if (isShowGlobalRot)
        {
            Gizmos.color = globalColor;
            Gizmos.DrawLine(transform.position, transform.position + dir.normalized * size);
        }
    }

    void OnDrawApplyRotationDir(Vector3 dir)
    {
        if (isShowLocalRot)
        {
            OnDrawGizmosDir(transform.localRotation * dir);
        }

        if (isShowGlobalRot)
        {
            OnDrawGizmosDir(transform.rotation * dir);
        }
    }

    void OnDrawQuatAxis()
    {
        Gizmos.color = Color.white;
        Vector3 axis = Vector3.zero;
        float angle;

        if (isShowLocalRot)
        {
            this.transform.localRotation.ToAngleAxis(out angle, out axis);
        }

        if (isShowGlobalRot)
        {
            this.transform.rotation.ToAngleAxis(out angle, out axis);
        }

        OnDrawGizmosDir(axis);
    }

    protected override void OnMyDrawGizmos()
    {
        if (enableForward)
        {
            OnDrawApplyRotationDir(Vector3.forward);
        }
        if (enableUp)
        {
            OnDrawApplyRotationDir(Vector3.up);
        }
        if (enableRight)
        {
            OnDrawApplyRotationDir(Vector3.right);
        }
        if (enableAxis)
        {
            OnDrawQuatAxis();
        }
    }
}
