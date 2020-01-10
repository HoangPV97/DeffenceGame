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
    public List<GameObject> AllianceWindSkills;
    public List<GameObject> AllianceIceSkills;
    public List<GameObject> AllianceEarthSkills;
    public List<GameObject> AllianceFireSkills;
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
    public GameObject GetAllianceSkill(Elemental elemental, int index)
    {
        switch (elemental)
        {
            case Elemental.None:
                return null;
            case Elemental.Wind:
                return AllianceWindSkills[index];
            case Elemental.Ice:
                return AllianceIceSkills[index];
            case Elemental.Fire:
                return AllianceFireSkills[index];
            case Elemental.Earth:
                return AllianceEarthSkills[index];
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