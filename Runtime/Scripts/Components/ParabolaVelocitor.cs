using LCHFramework.Components;
using UnityEngine;

namespace LCHFramework
{
    [RequireComponent(typeof(Rigidbody))]
    public class ParabolaVelocitor : LCHMonoBehaviour
    {
        public void SetVelocity(Vector3 velocity)
        {
#if !UNITY_6000_0_OR_NEWER
            GetComponent<Rigidbody>().velocity = velocity;
#else
            GetComponent<Rigidbody>().linearVelocity = velocity;
#endif
        }

        public void SetForce(Vector3 force)
        {
            GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }

        public Vector3 GetVelocity(Vector3 currentPos, Vector3 targetPos, float initialAngle)
        {
            float gravity = Physics.gravity.magnitude;
            float angle = initialAngle * Mathf.Deg2Rad;

            Vector3 planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
            Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

            float distance = Vector3.Distance(planarTarget, planarPosition);
            float yOffset = currentPos.y - targetPos.y;

            float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

            Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

            float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (targetPos.x > currentPos.x ? 1 : -1);
            Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

            return finalVelocity;
        }
    }
}

