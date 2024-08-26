using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow (i.e., the player's ship)
    public Vector3 offset = new Vector3(0, 100, 0); // Offset from the target

    private bool followTarget = true; // Control whether the camera should follow the target

    void LateUpdate()
    {
        if (followTarget && target != null)
        {
            // Set the position of the camera to be the same as the target's position plus the offset
            transform.position = target.position + offset;

            // Lock the camera rotation to look straight down
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        else if (followTarget && target == null)
        {
            target = PlayerController.instance.transform;
            if (target != null)
            {
                transform.position = target.position + offset;
            }
        }
    }

    // Method to stop following the target
    public void StopFollowing()
    {
        followTarget = false;
    }
}
