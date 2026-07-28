using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;

    [SerializeField] private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                Debug.LogError("No target assigned and no GameObject tagged as player. Please ensure a reference for the target variable");
                return;
            }

            target = player.transform;
        }
    }

    //Inputs being polled in update - Physics generally are applied in FixedUpdate - and camera movement is done in late update

    //Update is with the computer tick rate
    //Fixed update is a fixed rate at which your game updates
    //Late update happens as the last possible update for that frame

    // Update is called once per frame
    void LateUpdate()
    {
        //early return - if we don't have a target, we can't follow anything so we shouldn't do anything
        if (target == null) return;

        //Store our current position
        Vector3 currentPos = transform.position;

        //update our X pos to be the same as our target's x pos - but we will want to clamp it between our minimum and maximum values
        currentPos.x = Mathf.Clamp(target.position.x, minXPos, maxXPos);

        //apply the postion back to the camera
        transform.position = Vector3.MoveTowards(transform.position, currentPos, 5f * Time.deltaTime);
    }
}
