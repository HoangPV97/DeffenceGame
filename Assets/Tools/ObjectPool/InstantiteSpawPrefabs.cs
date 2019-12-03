using UnityEngine;
namespace InviGiant.Tools
{
    public class InstantiteSpawPrefabs : MonoBehaviour
    {
        void Start()
        {
            PreloadResource("Bullets/Player/Bullet_Ashe", 5);
            PreloadResource("Effect/DashEnd", 1);
            PreloadResource("Effect/DashStart", 1);
            PreloadResource("Effect/DropInWater", 1);
            PreloadResource("Effect/IngameItem", 1);
        }

        void PreloadResource(string pathFolder, int numberPreload)
        {
            GameObject[] fxPrefabs = Resources.LoadAll<GameObject>(pathFolder);
            for (int i = 0; i < fxPrefabs.Length; i++)
                SmartPool.Instance.Preload(fxPrefabs[i], numberPreload);
        }
    }
}
