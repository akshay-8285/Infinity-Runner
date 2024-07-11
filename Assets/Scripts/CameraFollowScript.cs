using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public Transform player; 
    public float cameraDistance = 2f; 
    public float cameraHeight = 1f; 
    public float cameraFollowSpeed = 5f; 

    void LateUpdate()
    {
        if (player != null)
        {
           
            Vector3 targetPosition = player.position - player.forward * cameraDistance + Vector3.up * cameraHeight;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraFollowSpeed);


            transform.LookAt(player);
        }
    }
}
