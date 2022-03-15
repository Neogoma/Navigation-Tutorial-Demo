using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private RectTransform rt;
    private bool canRotate = false;
    private float angle;
    /// <summary>
    /// Gets the rect transform component and begins arrow rotation.
    /// </summary>
    public void Init()
    {
        rt = GetComponent<RectTransform>();
        Vector3 objScreenPos = Camera.main.WorldToScreenPoint(NavControl.Instance.target);
        Vector3 dir = (objScreenPos - rt.position).normalized;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        canRotate = true;
    }

    private void Update()
    {
        if(canRotate)
        {             
            rt.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnDisable()
    {
        canRotate = false;
    }
}
