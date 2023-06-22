using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 10f;
    public float moveSpeed = 2f;
    public GameObject player;

    Vector3 startPos;
    Vector3 endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = transform.position + Vector3.forward * moveDistance;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(startPos, endPos, (Mathf.Sin(Time.time * moveSpeed) + 1) / 2);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            player.gameObject.transform.parent = null;
        }
    }
}
