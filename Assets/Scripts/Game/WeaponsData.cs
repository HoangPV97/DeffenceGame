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

}
