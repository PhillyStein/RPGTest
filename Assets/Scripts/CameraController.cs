using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    /*
    public GameObject followTarget;
    private Vector3 targetPos;
    public float moveSpeed;
    */

    public Transform target;

    public Tilemap theMap;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    private float halfHeight;
    private float halfWidth;

    // Start is called before the first frame update
    void Start()
    {

        if(theMap == null)
        {
            theMap = FindObjectOfType<Tilemap>();
        }

        //target = PlayerController.instance.transform;
        target = FindObjectOfType<PlayerController>().transform;

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);
        topRightLimit = theMap.localBounds.max - new Vector3(halfWidth, halfHeight, 0f);


        PlayerController.instance.SetBounds(theMap.localBounds.min, theMap.localBounds.max);
    }

    // LateUpdate is called once per frame after Update
    void LateUpdate()
    {
        /*
        if (followTarget == null)
        {
            followTarget = GameObject.FindGameObjectWithTag("Player");
        }
        targetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        */

        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Keep the camera inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }
}
