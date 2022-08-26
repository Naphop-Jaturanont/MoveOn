using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_CapsuleCast : MonoBehaviour
{
    public bool drawLocal;
    public bool drawWorld;
    public CapsuleCollider capsuleCollider;

    private void OnDrawGizmos()
    {
        if (capsuleCollider == null)
            return;

        if (drawLocal) { DrawLocal(); }
        if (drawWorld) { DrawWorld(); }
    }

    private void DrawWorld()
    {
        float radius = capsuleCollider.radius;

        Vector3 top = capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - radius);
        Vector3 bottom = capsuleCollider.center - Vector3.up * (capsuleCollider.height / 2 - radius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(top, radius);
        Gizmos.DrawWireSphere(bottom, radius);
    }

    private void DrawLocal()
    {
        float heightScale = MathF.Abs(transform.lossyScale.y);
        float radiusScale = MathF.Max(MathF.Abs(transform.lossyScale.x), MathF.Abs(transform.lossyScale.z));

        float radius = capsuleCollider.radius * radiusScale;
        float totalheights = Mathf.Max(capsuleCollider.height * heightScale, radius * 2);

        Vector3 direction = transform.up;
        Vector3 center = transform.TransformPoint(capsuleCollider.center);
        Vector3 top = center + direction * (totalheights / 2 - radius);
        Vector3 bottom = center - direction * (totalheights / 2 - radius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(top, radius);
        Gizmos.DrawWireSphere(bottom, radius);

    }
}
