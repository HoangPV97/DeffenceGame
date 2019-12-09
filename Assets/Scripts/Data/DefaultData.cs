﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DefaultData", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
[System.Serializable]
public class DefaultData : ScriptableObject
{
    public TextAsset MonsterDataBases, GameStageDataBase, GameEnemyDataBase, Weapons, AllianceDataBases, BaseDatabases;
    public List<GameObject> WeaponWindSkills;
    public List<GameObject> WeaponIceSkills;
    public List<GameObject> WeaponEarthSkills;
    public List<GameObject> WeaponFireSkills;
    public GameObject AllianceWindSkills;
    public GameObject AllianceIceSkills;
    public GameObject AllianceEarthSkills;
    public GameObject AllianceFireSkills;
    public List<DefaultDataSkill> TextWeaponSkills;
    public GameObject GetWeaponSkill(Elemental elemental, int index)
    {
        switch (elemental)
        {
            case Elemental.None:
                return null;
            case Elemental.Wind:
                return WeaponWindSkills[index];
            case Elemental.Ice:
                return WeaponIceSkills[index];
            case Elemental.Fire:
                return WeaponFireSkills[index];
            case Elemental.Earth:
                return WeaponEarthSkills[index];
        }
        return null;
    }
    public GameObject GetAllianceSkill(Elemental elemental)
    {
        switch (elemental)
        {
            case Elemental.None:
                return null;
            case Elemental.Wind:
                return AllianceWindSkills;
            case Elemental.Ice:
                return AllianceIceSkills;
            case Elemental.Fire:
                return AllianceFireSkills;
            case Elemental.Earth:
                return AllianceEarthSkills;
        }
        return null;
    }

    public string GetTextWeaponSkill(string SkillID)
    {
        for (int i = 0; i < TextWeaponSkills.Count; i++)
        {
            if (TextWeaponSkills[i].SkillID == SkillID)
                return TextWeaponSkills[i].SkillText;
        }
        return "";
    }
}
[System.Serializable]
public class DefaultDataSkill
{
    public string SkillID, SkillText;
}