using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float speed = 20;
    public Transform target;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PLayer>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}
