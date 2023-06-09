using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawnerUp : MonoBehaviour
{
    
    public static int CountEnemyAlive = 0;//当前敌人存活的数量，默认为0
    public Wave[] waves;//在Inspector面板上的Waves里可以创建Size为4了，下面依次拉取和填写。
    public Transform START;//生成的开始位置，然后把START直接拖过来
    public float waveRate = 3;//每生成一波暂停3秒


    void Start()
    {
        StartCoroutine("SpawnEnemy");
    }

    public void Stop()
    {
        StopCoroutine("SpawnEnemy");
    }

    //定义一个协程
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(5);
        foreach (Wave wave in waves)//便利每一波敌人，按count来进行生成，按rate进行分割
        {
            for (int i = 0; i < wave.count; i++)
            {
                //Quaternion.identity表示无旋转
                GameObject.Instantiate(wave.enemyPrefab, START.position, Quaternion.identity);
                CountEnemyAlive++;
                if (i != wave.count - 1)//看产生的是不是这波最后一个敌人，如果是最后一个，则直接走后面那个等待时间
                {
                    yield return new WaitForSeconds(wave.rate);//间隔rate时间后再生成下一个
                }
            }
            while (CountEnemyAlive > 0 && EnemySpawnerDown.CountEnemyAlive > 0)
            {
                yield return 0;//如果还有敌人存在，那么暂停0帧
            }
            yield return new WaitForSeconds(waveRate);
        }
        while (CountEnemyAlive > 0 && EnemySpawnerDown.CountEnemyAlive > 0)//游戏胜利的条件是敌人都生成、并且也都死亡了. 大于0说明敌人还有存活，那么就return 0，暂停0帧
        {
            yield return 0;
        }
        while (CountEnemyAlive > 0 && EnemySpawnerDown.CountEnemyAlive == 0)//游戏胜利的条件是敌人都生成、并且也都死亡了. 大于0说明敌人还有存活，那么就return 0，暂停0帧
        {
            yield return 0;
        }
        while (CountEnemyAlive == 0 && EnemySpawnerDown.CountEnemyAlive > 0)//游戏胜利的条件是敌人都生成、并且也都死亡了. 大于0说明敌人还有存活，那么就return 0，暂停0帧
        {
            yield return 0;
        }

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(10);
    }
  
}
