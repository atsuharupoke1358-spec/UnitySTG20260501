using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    public float speed = 2f;
    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}
