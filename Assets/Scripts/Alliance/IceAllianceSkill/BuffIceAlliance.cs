using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIceAlliance : Skill
{
    public int DamageStats = 20;
    public float FirerateStats = 20f;
    public float EffecttimeStats = 0.5f;
    GameObject WindAlliance;
    // Start is called before the first frame update
    protected override void Start()
    {
        var EarthAlliance = GetAlliance(Elemental.Earth)?.GetComponent<EarthAllianceCharacter>();
        if (EarthAlliance != null)
        {
            EarthAlliance.ATK += DamageStats;
            EarthAlliance.ATKspeed += EarthAlliance.ATKspeed * FirerateStats / 100;
            GameplayController.Instance.GetSkill("ALLIANCE_EARTH_SKILL_1").AddDatatAttribute("TimeEffect", EffecttimeStats);
        }
    }
}
