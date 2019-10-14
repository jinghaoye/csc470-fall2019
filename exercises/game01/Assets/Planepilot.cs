using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planepilot : MonoBehaviour
{
    public float speed = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 moveCamto = transform.position - transform.forward * 10.0f + Vector3.up * 5.0f;
   
        float bias = 0.96f;
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamto * (1.0f - bias);
        speed -= transform.forward.y * Time.deltaTime * 50.0f;
        if(speed < 35.0f){
            speed = 35.0f;
        }
        Camera.main.transform.LookAt(transform.position + transform.forward * 30.0f);
        transform.position += transform.forward * Time.deltaTime * speed;
        transform.Rotate(-Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal"));
      
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
