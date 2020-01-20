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
        var EarthAlliance = GetEarthAlliance();
        if (EarthAlliance != null)
        {
            EarthAlliance.ATK += DamageStats;
            EarthAlliance.ATKspeed += EarthAlliance.ATKspeed * FirerateStats / 100;
            GameplayController.Instance.GetSkill("ALLIANCE_EARTH_SKILL_1").AddDatatAttribute("TimeEffect", EffecttimeStats);
        }
    }
    public EarthAllianceCharacter GetEarthAlliance()
    {
        for (int i = 0; i < GameplayController.Instance.AllianceController.Count; i++)
        {
            if (GameplayController.Instance.AllianceController[i].elementalType == Elemental.Earth)
            {
                return GameplayController.Instance.AllianceController[i].gameObject.GetComponent<EarthAllianceCharacter>();
            }
        }
        return null;
    }
}
