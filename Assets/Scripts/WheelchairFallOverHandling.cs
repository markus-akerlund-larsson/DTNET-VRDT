using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelchairFallOverHandling : MonoBehaviour
{
    public float maxRollAngle = 10;
    public float maxAttackAngle = 15;
    // Start is called before the first frame update

    private void LateUpdate()
    {
        Balance();
    }

    private void Balance()
    {

        // Calculate the current roll and attack angles
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        float originalRollAngle = eulerRotation.z;
        float originalAttackAngle = eulerRotation.x;

        // Convert angles to the range -180 to 180 degrees
        if (originalRollAngle > 180f)
            originalRollAngle -= 360f;
        if (originalAttackAngle > 180f)
            originalAttackAngle -= 360f;

        // Clamp the roll and attack angles within the specified ranges
        float clampedRollAngle = Mathf.Clamp(originalRollAngle, -maxRollAngle, maxRollAngle);
        float clampedAttackAngle = Mathf.Clamp(originalAttackAngle, -maxAttackAngle, maxAttackAngle);

        // Calculate the difference in angles
        float rollAngleDifference = clampedRollAngle - originalRollAngle;
        float attackAngleDifference = clampedAttackAngle - originalAttackAngle;

        // Apply the clamped roll and attack angles to the object's rotation
        Quaternion clampedRotation = Quaternion.Euler(clampedAttackAngle, eulerRotation.y, clampedRollAngle);
        transform.rotation = clampedRotation;

        // Calculate the transformed forward direction after rotation
        Vector3 forwardDirection = clampedRotation * Vector3.forward;

        // Apply the proportional boost based on the angle differences

        float boostFactor = -1.2f; // Adjust this value based on the desired boost strength
        transform.position += forwardDirection * attackAngleDifference * boostFactor * Time.deltaTime;

        GetComponent<Rigidbody>().velocity = Vector3.zero;

        if(attackAngleDifference > Mathf.Epsilon)
        {
            Rigidbody[] wheels = GetComponentsInChildren<Rigidbody>();
            for(int i = 0; i < wheels.Length; i++)
            {
                wheels[i].angularVelocity = Vector3.zero;
            }
        }

    }
}
