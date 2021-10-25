using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStartData : MonoBehaviour
{
    public AutoShoot autoShootComponent;
    public string weaponStatName;
	
    // Use this for initialization
	void Start ()
    {
        if (weaponStatName == "Blaster")
        {
            autoShootComponent.SwitchProfile(StatsManager.instance.GetStatsValue(weaponStatName,
            StatsManager.instance.blasterUpgradeList));
        }
        else if (weaponStatName == "Missile")
        {
            autoShootComponent.SwitchProfile(StatsManager.instance.GetStatsValue(weaponStatName,
            StatsManager.instance.missileUpgradeList));
        }

        autoShootComponent.SetIntervalValue();
	}
}
