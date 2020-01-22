using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpawnEnemy : MonoBehaviour
{
    public List<GameObject> PositionList;
    private const float SizeX = 10.6f;
    private void Start()
    {
        int count = GameplayController.Instance.AllianceController.Count;
        float SpaceX = SizeX / (count + 2);

        if (count % 2 == 0)
        {
            Vector3 StartPos = new Vector3(0, -6.97f, 0);
            SetPos(count, StartPos, SpaceX);
        }
        else
        {
            Vector3 StartPos = new Vector3(SpaceX / 2, -6.97f, 0);
            SetPos(count, StartPos, SpaceX);
        }
    }
    private void SetPos(int count, Vector3 StartPos, float SpaceX)
    {
        for (int i = 0; i < count; i++)
        {
            if (i % 2 == 0)
            {
                StartPos += new Vector3(SpaceX, 0, 0) * i;
                PositionList[i].transform.position = StartPos;

            }
            else
            {
                StartPos -= new Vector3(SpaceX, 0, 0) * i;
                PositionList[i].transform.position = StartPos;
            }
        }
    }
}
