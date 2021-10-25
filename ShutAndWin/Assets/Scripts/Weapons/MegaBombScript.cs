using UnityEngine;

public class MegaBombScript : MonoBehaviour
{
    public float radius = 2f;
    public float damage = 5f;
    public ParticleSystem mbFX;

    // Use this for initialization
    void Start ()
    {
        var partMain = mbFX.main;
        partMain.startSize = radius * partMain.startSize.constant;

        radius = StatsManager.instance.GetStatsValue("MegaBomb", StatsManager.instance.megaBombUpgList).radius;
        damage = StatsManager.instance.GetStatsValue("MegaBomb", StatsManager.instance.megaBombUpgList).damage;
    }

    // Update is called once per frame
    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.B))
        {
            DeployBomb();
        }
	}

    public void DeployBomb()
    {
        print("Bomb Deployed!");

        mbFX.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            HealthSystem healthSystem = colliders[i].GetComponent<HealthSystem>();

            if (healthSystem != null && colliders[i].CompareTag("Enemy"))
            {
                healthSystem.TakeDamage(damage, colliders[i]);

                print(colliders[i].name + " is being hit by the bomb");
            }
        }
    }
}
