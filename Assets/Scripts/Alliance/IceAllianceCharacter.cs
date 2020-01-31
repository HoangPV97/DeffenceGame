using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spine;
using UnityEngine;

public class IceAllianceCharacter : AllianceController
{
    #region Target
    [Serializable]
    public class targetInsight
    {
        public EnemyController enemy;
        public float distance;
        public targetInsight(EnemyController _enemy, float _dis)
        {
            this.enemy = _enemy;
            this.distance = _dis;
        }
    }
    #endregion
    int numberTarget = 3;
    float DamagePlus;
    public List<EnemyController> targetList;
    public override void Start()
    {
        base.Start();
        targetList = new List<EnemyController>();
    }
    #region comment
    //public override void SetDataWeapon(Elemental elemental, float Atkspeed, float atk, float BulletSpeed)
    //{
    //    numberTarget = 3;
    //    base.SetDataWeapon(elemental, Atkspeed, atk, BulletSpeed);
    //}
    //public override void UpdateEnemy()
    //{
    //    base.Update();

    //    //if (listEnemies.Count > 0)
    //    //{
    //    //    listEnemies = listEnemies.OrderBy(obj => (obj.transform.position - transform.position).magnitude).ToList();
    //    //    if (targetList.Count == 0)
    //    //    {
    //    //        targetList.Add(listEnemies[0]);
    //    //    }
    //    //    else
    //    //    {
    //    //        for (int j = 0; j < targetList.Count; j++)
    //    //        {
    //    //            if (!targetList.Contains(listEnemies[j]) && targetList.Count < numberTarget)
    //    //            {
    //    //                targetList.Add(listEnemies[j]);
    //    //            }
    //    //        }
    //    //    }
    //    //    nearestEnemy = listEnemies[0];
    //    //    Alliance.target = nearestEnemy;
    //    //}


    //    //    int index = 0;
    //    //shortestDistance = Mathf.Infinity;
    //    //if (listEnemies.Count > 0)
    //    //{
    //    //    for (int i = 0; i < listEnemies.Count; i++)
    //    //    {
    //    //        float distancetoEnemy = Vector3.Distance(transform.position, listEnemies[i].transform.position);
    //    //        if (distancetoEnemy < shortestDistance)
    //    //        {
    //    //            if (targetList.Count == 0)
    //    //            {
    //    //                targetInsight obj = new targetInsight(listEnemies[i], distancetoEnemy);
    //    //                targetList.Add(obj);
    //    //            }
    //    //            else
    //    //            {
    //    //                int temp = 0;
    //    //                for (int j = 0; j < targetList.Count; j++)
    //    //                {
    //    //                    if(!targetList[j].enemy.isLive)
    //    //                        targetList.RemoveAt(j);
    //    //                    if (targetList[j].enemy.gameObject != listEnemies[i].gameObject)
    //    //                    {
    //    //                        if (targetList.Count < numberTarget)
    //    //                        {
    //    //                            if (targetList[j].distance < distancetoEnemy)
    //    //                            {
    //    //                                temp = j + 1;
    //    //                            }
    //    //                            targetInsight obj = new targetInsight(listEnemies[i], distancetoEnemy);
    //    //                            targetList.Add(obj);
    //    //                            return;
    //    //                        }
    //    //                        else
    //    //                        {
    //    //                            targetList.RemoveAt(temp);
    //    //                            targetInsight obj = new targetInsight(listEnemies[i], distancetoEnemy);
    //    //                            targetList.Add(obj);
    //    //                            return;
    //    //                        }
    //    //                    }
    //    //                }
    //    //                shortestDistance = distancetoEnemy;
    //    //            }



    //    //            index = i;
    //    //        }
    //    //    }

    //    //else
    //    //{
    //    //    Alliance.target = null;
    //    //    characterState = CharacterState.Idle;
    //    //}
    //}
    #endregion

    public override void SetDataWeapon(Elemental elemental, float Atkspeed, float atk, float BulletSpeed, float _range)
    {
        ATK = Mathf.RoundToInt(atk* DataController.Instance.InGameBaseData.achi_AddedDmgAllianceIce);
        base.SetDataWeapon(elemental, Atkspeed, atk, BulletSpeed, _range);
    }
    public override void Shoot()
    {
        for (int i = 0; i < listEnemies.Count; i++)
        {
            if (i >= numberTarget)
            {
                return;
            }
            characterState = CharacterState.Attack;
            GameObject bullet = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet, Barrel.transform.position, Quaternion.identity);
            var alianceBullet = bullet.GetComponent<BulletController>();
            if (alianceBullet != null)
            {
                alianceBullet.elementalBullet = elementalType;
                alianceBullet.SetTarget(listEnemies[i]);
                alianceBullet.setDirection(listEnemies[i].transform.position - transform.position);
                alianceBullet.Move(listEnemies[i].transform.position - transform.position);
                alianceBullet.SetDataBullet(BulletSpeed, ATK,DamagePlus);
            }
        }
    }
    public void IceSkill(Vector3 _position)
    {
        GameObject iceskill = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet_Skill, _position, Quaternion.identity);
        float particleTime = iceskill.GetComponentInChildren<ParticleSystem>().main.duration;
        SoundManager.Instance.PlayClipOneShot(SoundManager.Instance.Explosion);
        GameObject effectStart = ObjectPoolManager.Instance.SpawnObject(Alliance.EffectStart, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        CheckDestroyEffect(effectStart, particleTime);
        CheckDestroyEffect(iceskill, particleTime);
    }
}
