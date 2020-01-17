using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
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
    public SkeletonDataAsset[] WeaponsUISkeletonDataAsset, AllianceUISkeletonDataAsset;
    public List<DailyQuestDatabase> dailyQuestDatabases;
    public List<AchievementDatabase> achievementDatabases;

    public AchievementDatabase GetAchievementDatabase(ACHIEVEMENT_TYPE _TYPE)
    {
        for (int i = 0; i < achievementDatabases.Count; i++)
        {
            if (achievementDatabases[i]._TYPE == _TYPE)
                return achievementDatabases[i];
        }
        return achievementDatabases[0];
    }

    public DailyQuestDatabase GetDailyQuestDatabase(QUEST_TYPE _TYPE)
    {
        for (int i = 0; i < dailyQuestDatabases.Count; i++)
        {
            if (dailyQuestDatabases[i]._TYPE == _TYPE)
                return dailyQuestDatabases[i];
        }
        return dailyQuestDatabases[0];
    }

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
[System.Serializable]
public class DailyQuestDatabase
{
    public QUEST_TYPE _TYPE;
    public List<int> Target;
    public List<ListItem> BaseReward;
    public List<QuestReward> Reward1;
    public List<QuestReward> Reward2;
    public List<QuestReward> Reward3;
    public int MAX_LEVEL
    {
        get
        {
            return Target.Count - 1;
        }
    }
    public List<Item> GetRandomReward(int Level)
    {
        List<Item> items = new List<Item>();
        items.AddRange(BaseReward[Level].items);
        if (Reward1.Count > 0)
        {
            int ran = Random.Range(0, 2);
            if (ran > 0)
            {
                var item1 = InviGiant.Tools.IGMaths.GetRandom(Reward1[Level].ITEMs, Reward1[Level].Number);
                for (int i = 0; i < item1.Count; i++)
                {
                    bool b = true;
                    for (int j = 0; j < items.Count; j++)
                    {
                        if (items[j].Type == item1[i])
                        {
                            items[j].Quality++;
                            b = false;
                        }
                    }
                    if (b)
                        items.Add(new Item()
                        {
                            Quality = 1,
                            Type = item1[i]
                        });
                }
                var item2 = InviGiant.Tools.IGMaths.GetRandom(Reward2[Level].ITEMs, Reward2[Level].Number);
                for (int i = 0; i < item2.Count; i++)
                {
                    bool b = true;
                    for (int j = 0; j < items.Count; j++)
                    {
                        if (items[j].Type == item2[i])
                        {
                            items[j].Quality++;
                            b = false;
                        }
                    }
                    if (b)
                        items.Add(new Item()
                        {
                            Quality = 1,
                            Type = item2[i]
                        });
                }
            }
            else
            {
                var item2 = InviGiant.Tools.IGMaths.GetRandom(Reward3[Level].ITEMs, Reward3[Level].Number);
                for (int i = 0; i < item2.Count; i++)
                {
                    bool b = true;
                    for (int j = 0; j < items.Count; j++)
                    {
                        if (items[j].Type == item2[i])
                        {
                            items[j].Quality++;
                            b = false;
                        }
                    }
                    if (b)
                        items.Add(new Item()
                        {
                            Quality = 1,
                            Type = item2[i]
                        });
                }
            }
        }
        return items;
    }
}
public enum QUEST_TYPE
{
    QUEST_1 = 0,
    QUEST_2,
    QUEST_3,
    QUEST_4,
    QUEST_5,
    QUEST_6,
    QUEST_7,
    QUEST_8
}
public enum ACHIEVEMENT_TYPE
{
    ACHIEVEMENT_1 = 0,
    ACHIEVEMENT_2,
    ACHIEVEMENT_3,
    ACHIEVEMENT_4,
    ACHIEVEMENT_5,
    ACHIEVEMENT_6,
    ACHIEVEMENT_7,
    ACHIEVEMENT_8,
    ACHIEVEMENT_9,
    ACHIEVEMENT_10,
    ACHIEVEMENT_11,
    ACHIEVEMENT_12,
    ACHIEVEMENT_13,
    ACHIEVEMENT_14,
    ACHIEVEMENT_15
}
[System.Serializable]
public class QuestReward
{
    public int Number;
    public List<ITEM_TYPE> ITEMs;
}
[System.Serializable]
public class AchievementDatabase
{
    public ACHIEVEMENT_TYPE _TYPE;
    public List<int> Target;
    public List<int> Reward;
    public int MAX_LEVEL { get { return Reward.Count; } }
    public int GetTarget(int Level)
    {
        if (Level == 0)
            return 0;
        return Target[Level - 1];
    }
    public int GetReward(int Level)
    {
        if (Level == 0)
            return 0;
        return Reward[Level - 1];
    }
}