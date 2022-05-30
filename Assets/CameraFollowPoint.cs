using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPoint : MonoBehaviour
{
    Transform player1Transform;
    Transform player2Transform;

    // Start is called before the first frame update
    void Start()
    {
        player1Transform = GameObject.Find("Player1").GetComponent<Transform>();
        if (player1Transform == null)
        {
            Debug.Log("Player 1 not found");
        }
        player2Transform = GameObject.Find("Player2").GetComponent<Transform>();
        if (player2Transform == null)
        {
            Debug.Log("Player 2 not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = (player1Transform.position + player2Transform.position) / 2;
        transform.position = newPos;
    }
}
