using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEarthAlliance : Skill
{
    public int DamageStats = 20;
    public float FirerateStats = 20f;
    public float EffecttimeStats = 0.5f;
    GameObject WindAlliance;
    // Start is called before the first frame update
    protected override void Start()
    {
        if (GameplayController.Instance.Alliance_1.elementalType == Elemental.Wind)
        {
            WindAlliance = GameplayController.Instance.Alliance_1.gameObject;
        }
        else if (GameplayController.Instance.Alliance_2.elementalType == Elemental.Wind)
        {
            WindAlliance = GameplayController.Instance.Alliance_2.gameObject;
        }
        var EarthAlliance = WindAlliance.GetComponent<EarthAllianceCharacter>();
        if (EarthAlliance != null)
        {
            EarthAlliance.ATK += DamageStats;
            EarthAlliance.ATKspeed += EarthAlliance.ATKspeed * FirerateStats / 100;
            GameplayController.Instance.GetSkill("ALLIANCE_EARTH_SKILL_1").AddDatatAttribute("TimeEffect", EffecttimeStats);
        }
    }
}
