using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWindAlliance : Skill
{
    public float BounceStats=1f;
    public int DamageStats=50;
    public float FirerateStats =20f;
    public float EffecttimeStats=0.5f;
    GameObject WindAlliance;
    // Start is called before the first frame update
    void Start()
    {
        if (GameplayController.Instance.Alliance_1.elementalType == Elemental.Wind)
        {
            WindAlliance = GameplayController.Instance.Alliance_1.gameObject;
        }
        else if (GameplayController.Instance.Alliance_2.elementalType == Elemental.Wind)
        {
            WindAlliance = GameplayController.Instance.Alliance_2.gameObject;
        }
        var WindAlly= WindAlliance.GetComponent<WindAllianceCharacter>();
        WindAlly.numberBounce += 1;
        WindAlly.ATK += DamageStats;
        WindAlly.ATKspeed += WindAlly.ATKspeed * FirerateStats / 100;
        GameplayController.Instance.GetSkill("ALLIANCE_WIND_SKILL_1").AddDatatAttribute("TimeEffect", EffecttimeStats);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
