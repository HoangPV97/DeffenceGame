using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWindAlliance : Skill
{
    public float BounceStats = 1f;
    public int DamageStats = 50;
    public float FirerateStats = 20f;
    public float EffecttimeStats = 0.5f;
    GameObject WindAlliance;
    // Start is called before the first frame update
    protected override void Start()
    {
 
        var WindAlly = GetAlliance(Elemental.Wind)?.GetComponent<WindAllianceCharacter>();
        if (WindAlly != null)
        {
            WindAlly.bounceNumber += 1;
            WindAlly.ATK += DamageStats;
            WindAlly.ATKspeed += WindAlly.ATKspeed * FirerateStats / 100;
            GameplayController.Instance.GetSkill("ALLIANCE_WIND_SKILL_1").AddDatatAttribute("TimeEffect", EffecttimeStats);
        }
    }

}
