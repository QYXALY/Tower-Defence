using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//保存每一波敌人生成所需要的属性
[System.Serializable]//表示可以被序列化，可以显示在Inspector面板上，
                     //不然EnemySpawner那边无法以数组的形式创建
public class Wave
{
    public GameObject enemyPrefab;//根据这个生成敌人
    public int count;//生成个数
    public float rate;//生成的时间间隔
}
