using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    public ShootProfile shootProfile;
    public GameObject bulletPrefabs;
    public Transform firePoint;

    private float totalSpread;
    private WaitForSeconds rate, interval;

	// Use this for initialization
	private void OnEnable ()
    {
        SetIntervalValue();

        if (firePoint == null)
            firePoint = transform;

        StartCoroutine(ShootingSequence());
    }

    public void SetIntervalValue()
    {
        interval = new WaitForSeconds(shootProfile.interval);
        rate = new WaitForSeconds(shootProfile.fireRate);

        totalSpread = shootProfile.spread * shootProfile.amount;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SwitchProfile(ShootProfile newProfile)
    {
        shootProfile = newProfile;
    }

    IEnumerator ShootingSequence()
    {
        yield return rate;

        while (true)
        {
            float angle = 0f;

            for (int i = 0; i < shootProfile.amount; i++)
            {
                angle = totalSpread * (i / (float)shootProfile.amount);
                angle -= (totalSpread / 2f) - (shootProfile.spread / shootProfile.amount);

                Shoot(angle);

                if (shootProfile.fireRate > 0f)
                    yield return rate;
            }

            yield return interval;
        }
    }

    void Shoot(float angle)
    {
        Quaternion bulletRotation = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, 0f);

        GameObject temp = PoolingManager.instance.UseObject(bulletPrefabs, firePoint.position, bulletRotation);
        temp.name = shootProfile.damage.ToString();
        temp.transform.Rotate(Vector3.up, angle);
        temp.GetComponent<BulletMove>().speed = shootProfile.speed;
        PoolingManager.instance.ReturnObject(temp, shootProfile.destroyRate);
    }
}
