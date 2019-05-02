using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.SystemControls
{
    public class SystemControls
    {

        public static Vector2 Axis
        {
            get
            {
                return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }
        }

        /// <summary>
        /// The movement of the character looking to forward direction.
        /// </summary>
        /// <param name="t">Transform component of the character</param>
        /// <param name="moveSpeed"></param>
        public static void Movement(Transform t, float moveSpeed, float rotationSpeed)
        {
            /*t.Translate(Vector3.forward * Axis.magnitude * Time.deltaTime * moveSpeed);
            if(Axis.magnitude != 0f)
            {
                Vector3 fwd = new Vector3(Axis.x, 0f, Axis.y);
                t.rotation = Quaternion.LookRotation(fwd);
            }*/

            t.Translate(Vector3.forward * Axis.y * Time.deltaTime * moveSpeed);

            t.Rotate(Vector3.up * Axis.x * Time.deltaTime * rotationSpeed);
        }
    }
}
