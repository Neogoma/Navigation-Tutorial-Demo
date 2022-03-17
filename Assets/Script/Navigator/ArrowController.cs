using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neogoma.Stardust.Demo.Navigator
{
    /// <summary>
    /// Compass system points on only one axis towards the target direction 
    /// </summary>
    public class ArrowController : MonoBehaviour
    {
        /// <summary>
        /// this rect transform
        /// </summary>
        private RectTransform rt;
        /// <summary>
        /// determines if compass can update its angle and point to the target;
        /// </summary>
        private bool canRotate = false;
        /// <summary>
        /// angle to rotate compass
        /// </summary>
        private float angle;
        /// <summary>
        /// target position to point at.
        /// </summary>
        private Vector3 target;

        private void Start()
        {
            NavigationDemo.OnSetTarget += InitCompass;
        }
        private void Update()
        {
            if(canRotate)
            {             
                rt.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        /// <summary>
        /// Invoked when target is set. Also starts Compass.
        /// </summary>
        /// <param name="_target"> target position</param>
        private void InitCompass(Vector3 _target)
        {
            target = _target;
            Init();
        }

        /// <summary>
        /// Gets the rect transform component and begins arrow rotation.
        /// </summary>
        public void Init()
        {
            rt = GetComponent<RectTransform>();
            Vector3 objScreenPos = Camera.main.WorldToScreenPoint(target);
            Vector3 dir = (objScreenPos - rt.position).normalized;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            canRotate = true;
        }

        private void OnDisable()
        {
            canRotate = false;
            NavigationDemo.OnSetTarget -= InitCompass;
        }
    }
}
