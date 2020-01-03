using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DefaultData", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
[System.Serializable]
public class DefaultData : ScriptableObject
{
    public TextAsset MonsterDataBases, GameStageDataBase, GameEnemyDataBase, Weapons, AllianceDataBases, BaseDatabases, ItemDataBase;
    public List<GameObject> WeaponWindSkills;
    public List<GameObject> WeaponIceSkills;
    public List<GameObject> WeaponEarthSkills;
    public List<GameObject> WeaponFireSkills;
    public GameObject AllianceWindSkills;
    public GameObject AllianceIceSkills;
    public GameObject AllianceEarthSkills;
    public GameObject AllianceFireSkills;
    public List<DefaultDataSkill> TextWeaponSkills;
    public TextAsset AssetBundleItem;
    public AssetBundle Item;
    public TextAsset BossDataBase;
    public Sprite GetSpriteItem(ITEM_TYPE _TYPE)
    {
        if (Item == null)
        {
            byte[] bundleData = AssetBundleItem.bytes.Clone() as byte[];
            var bundle = AssetBundle.LoadFromMemory(bundleData);
            Item = bundle;
        }
        return Item.LoadAsset<Sprite>(_TYPE.ToString());
    }
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

    public List<string> GetWeaponSkillID(Elemental elemental)
    {
        switch (elemental)
        {
            case Elemental.None:
                break;
            case Elemental.Wind:
                return new List<string>() { "WEAPON_WIND_SKILL_1", "WEAPON_WIND_SKILL_2", "WEAPON_WIND_SKILL_3", "WEAPON_WIND_SKILL_4" };
            case Elemental.Ice:
                return new List<string>() { "WEAPON_ICE_SKILL_1", "WEAPON_ICE_SKILL_2", "WEAPON_ICE_SKILL_3", "WEAPON_ICE_SKILL_4" };
            case Elemental.Earth:
                return new List<string>() { "WEAPON_EARTH_SKILL_1", "WEAPON_EARTH_SKILL_2", "WEAPON_EARTH_SKILL_3", "WEAPON_EARTH_SKILL_4" };
            case Elemental.Fire:
                return new List<string>() { "WEAPON_FIRE_SKILL_1", "WEAPON_FIRE_SKILL_2", "WEAPON_FIRE_SKILL_3", "WEAPON_FIRE_SKILL_4" };
        }
        return null;
    }

    public List<string> GetArcherySkillID()
    {
        return new List<string>() { "ARCHERY_SKILL_1", "ARCHERY_SKILL_2", "ARCHERY_SKILL_3", "ARCHERY_SKILL_4" };
    }

    public List<string> GetTempleSkillID()
    {
        return new List<string>() { "ARCHERY_SKILL_1", "ARCHERY_SKILL_2", "ARCHERY_SKILL_3", "ARCHERY_SKILL_4" };
    }

    public List<string> GetFortressSkillID()
    {
        return new List<string>() { "ARCHERY_SKILL_1", "ARCHERY_SKILL_2", "ARCHERY_SKILL_3", "ARCHERY_SKILL_4" };
    }

    public List<string> GetAllianceSkillID(Elemental elemental)
    {
        switch (elemental)
        {
            case Elemental.None:
                break;
            case Elemental.Wind:
                return new List<string>() { "ALLIANCE_WIND_SKILL_1", "ALLIANCE_WIND_SKILL_2", "ALLIANCE_WIND_SKILL_3", "ALLIANCE_WIND_SKILL_4" };
            case Elemental.Ice:
                return new List<string>() { "ALLIANCE_ICE_SKILL_1", "ALLIANCE_ICE_SKILL_2", "ALLIANCE_ICE_SKILL_3", "ALLIANCE_ICE_SKILL_4" };
            case Elemental.Earth:
                return new List<string>() { "ALLIANCE_EARTH_SKILL_1", "ALLIANCE_EARTH_SKILL_2", "ALLIANCE_EARTH_SKILL_3", "ALLIANCE_EARTH_SKILL_4" };
            case Elemental.Fire:
                return new List<string>() { "ALLIANCE_FIRE_SKILL_1", "ALLIANCE_FIRE_SKILL_2", "ALLIANCE_FIRE_SKILL_3", "ALLIANCE_FIRE_SKILL_4" };
        }
        return null;
    }

    public string GetTextSkill(string SkillID)
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