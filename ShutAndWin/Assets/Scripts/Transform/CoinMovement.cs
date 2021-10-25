using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody rb;
    private Vector3 movement, target;

    private MagnetScript magnet;

    // Use this for initialization
    void Awake ()
    {
        rb = GetComponent<Rigidbody>();	
	}

    private void OnEnable()
    {
        movement = transform.position;
        movement += Random.insideUnitSphere * speed;
        movement.y = 0f;
    }

    private void Start()
    {
        magnet = FindObjectOfType<MagnetScript>();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        target = Vector3.Lerp(transform.position, movement, 1f * Time.fixedDeltaTime);

        if ((magnet.transform.position - transform.position).sqrMagnitude <= Mathf.Pow(magnet.magnetRange, 2f))
        {
            target = Vector3.Lerp(transform.position, magnet.transform.position, magnet.magnetPower * Time.fixedDeltaTime);
        }

        rb.MovePosition(target);
        rb.MoveRotation(Quaternion.Euler(target));
	}
}
