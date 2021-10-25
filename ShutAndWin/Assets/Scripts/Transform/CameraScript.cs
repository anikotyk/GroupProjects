using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float minX, maxX, speed = 2f;

    private Vector3 position;

	// Update is called once per frame
	void LateUpdate()
    {
        if (player == null)
            return;

        position = transform.localPosition;
        position.x = player.localPosition.x;
        position.x = Mathf.Clamp(position.x, minX, maxX);

        transform.localPosition = Vector3.Lerp(transform.localPosition, position, speed * Time.deltaTime);
	}
}
