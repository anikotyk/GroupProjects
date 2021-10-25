using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float speed;
    protected Rigidbody myRb;

	// Use this for initialization
	void Start ()
    {
        myRb = GetComponent<Rigidbody>();	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        myRb.velocity = transform.forward * speed;	
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Activator") || other.CompareTag("Activator"))
        {
            PoolingManager.instance.ReturnObject(gameObject);
        }
    }
}
