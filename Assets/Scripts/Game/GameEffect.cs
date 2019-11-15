using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Effect { Stun, freeze, Slow };
public class GameEffect : MonoBehaviour
{
    
    public Effect currentEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetEffect(Effect _effect,bool status,float? _time=null)
    {
        switch (_effect)
        {
            case Effect.freeze:
                if (status)
                {
                    Debug.Log("<Color=red>Freeze</Color>");
                }
                else
                {
                    Debug.Log("<Color=green>Destroy Freeze</Color>");
                }
                break;
            case Effect.Slow:
                if (status)
                {
                    Debug.Log("<Color=red>Slow</Color>");
                }
                else
                {
                    Debug.Log("<Color=green>Destroy Slow</Color>");
                }
                
                break;
            case Effect.Stun:
                if (status)
                {
                    Debug.Log("<Color=red>Stun</Color>");
                }
                else
                {
                    Debug.Log("<Color=green>Destroy Stun</Color>");
                }
                
                break;
        }
    }
    public GameObject GetElementalEffect(Elemental _elemental,Vector2 _position)
    {
        GameObject effect=null;
        switch (_elemental)
        {
            case Elemental.Fire:
                effect= ObjectPoolManager.Instance.SpawnObject("fireimpact", _position, Quaternion.identity);
                break;
            case Elemental.Ice:
                effect = ObjectPoolManager.Instance.SpawnObject("iceimpact", _position, Quaternion.identity);
                break;
            case Elemental.Wind:
                effect = ObjectPoolManager.Instance.SpawnObject("windimpact", _position, Quaternion.identity);
                break;
        }
        return effect;
    }
    
}
