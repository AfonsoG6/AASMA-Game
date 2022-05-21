using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform cameraTransf;
    public Transform playerTransf;
    [Range(0, 5f)][SerializeField] float maxCamDistance = 0.8f;
    
    void Start() {
        cameraTransf = GetComponent<Transform>();
        cameraTransf.position = new Vector3(playerTransf.position.x, playerTransf.position.y, cameraTransf.position.z);
    }

    
    void Update() {
        float distance = playerTransf.position.x - cameraTransf.position.x;
        float x = cameraTransf.position.x;
        float y = playerTransf.position.y;
        float z = cameraTransf.position.z;
        if (distance > maxCamDistance) {
            x += distance - maxCamDistance;
        }
        else if (distance < -maxCamDistance) {
            x += distance + maxCamDistance;
        }
        cameraTransf.position = new Vector3(x, y, z);
    }
}
