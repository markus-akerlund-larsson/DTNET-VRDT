using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayUpright : MonoBehaviour
{
    [SerializeField] float maxRollAngle;
    [SerializeField] float maxYValue;
    private void LateUpdate()
    {
        // Calculate the current roll angle
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        float rollAngle = eulerRotation.x;

        // Convert roll angle to the range -180 to 180 degrees
        if (rollAngle > 180f)
            rollAngle -= 360f;

        // Clamp the roll angle within the specified range
        float clampedRollAngle = Mathf.Clamp(rollAngle, -maxRollAngle, maxRollAngle);

        // Apply the clamped roll angle to the object's rotation
        Quaternion clampedRotation = Quaternion.Euler(clampedRollAngle, eulerRotation.y, eulerRotation.z);
        transform.rotation = clampedRotation;
        
        if(transform.position.y > maxYValue)
        {
            transform.position = new Vector3(transform.position.x, maxYValue, transform.position.z);
        }
    }
}
