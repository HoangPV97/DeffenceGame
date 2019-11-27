using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class WeaponsData : MonoBehaviour
{
    public TestWeapon Weapons;
    public static WeaponsData Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public int GetDataAtackWeapon(int level,int tier,Elemental elemental)
    {
        Debug.Log(Weapons.Weapons.Where(obj => (obj.Tier.Equals(tier)) && (obj.Type.Equals(elemental))).SingleOrDefault().ATK[level]);
        return Weapons.Weapons.Where(obj => ( obj.Tier.Equals(tier))&& (obj.Type.Equals(elemental))).SingleOrDefault().ATK[level];
    }
}
