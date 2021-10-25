using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float laserDuration = 3f, animSpeed = 2f;
    public ParticleSystem burstFx;

    private bool laserFired = false;
    private WaitForSeconds coroutineLaserDur;

    private Collider col;

    // Use this for initialization
    void Start ()
    {
        coroutineLaserDur = new WaitForSeconds(laserDuration);

        col = GetComponent<Collider>();
        col.enabled = false;

        laserDuration = StatsManager.instance.GetStatsValue("Laser", StatsManager.instance.laserUpgList).laserDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !laserFired)
        {
            StartCoroutine(FireLaser());
        }           
	}

    public void ShootLaser()
    {
        if (!laserFired)
            StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser()
    {
        col.enabled = true;

        laserFired = true;

        transform.localScale = Vector3.zero;

        burstFx.Play();

        while (transform.localScale.sqrMagnitude < 1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, animSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localScale = Vector3.one;

        yield return coroutineLaserDur;

        while (transform.localScale.sqrMagnitude > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, animSpeed * Time.deltaTime);
            yield return null;
        }

        burstFx.Stop();

        transform.localScale = Vector3.zero;

        laserFired = false;

        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();

            if (health)
            {
                health.TakeDamage(100f, other);
            }
        }
    }
}
