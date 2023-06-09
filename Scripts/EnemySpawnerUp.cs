using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawnerUp : MonoBehaviour
{
    
    public static int CountEnemyAlive = 0;//��ǰ���˴���������Ĭ��Ϊ0
    public Wave[] waves;//��Inspector����ϵ�Waves����Դ���SizeΪ4�ˣ�����������ȡ����д��
    public Transform START;//���ɵĿ�ʼλ�ã�Ȼ���STARTֱ���Ϲ���
    public float waveRate = 3;//ÿ����һ����ͣ3��


    void Start()
    {
        StartCoroutine("SpawnEnemy");
    }

    public void Stop()
    {
        StopCoroutine("SpawnEnemy");
    }

    //����һ��Э��
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(5);
        foreach (Wave wave in waves)//����ÿһ�����ˣ���count���������ɣ���rate���зָ�
        {
            for (int i = 0; i < wave.count; i++)
            {
                //Quaternion.identity��ʾ����ת
                GameObject.Instantiate(wave.enemyPrefab, START.position, Quaternion.identity);
                CountEnemyAlive++;
                if (i != wave.count - 1)//���������ǲ����Ⲩ���һ�����ˣ���������һ������ֱ���ߺ����Ǹ��ȴ�ʱ��
                {
                    yield return new WaitForSeconds(wave.rate);//���rateʱ�����������һ��
                }
            }
            while (CountEnemyAlive > 0 && EnemySpawnerDown.CountEnemyAlive > 0)
            {
                yield return 0;//������е��˴��ڣ���ô��ͣ0֡
            }
            yield return new WaitForSeconds(waveRate);
        }
        while (CountEnemyAlive > 0 && EnemySpawnerDown.CountEnemyAlive > 0)//��Ϸʤ���������ǵ��˶����ɡ�����Ҳ��������. ����0˵�����˻��д���ô��return 0����ͣ0֡
        {
            yield return 0;
        }
        while (CountEnemyAlive > 0 && EnemySpawnerDown.CountEnemyAlive == 0)//��Ϸʤ���������ǵ��˶����ɡ�����Ҳ��������. ����0˵�����˻��д���ô��return 0����ͣ0֡
        {
            yield return 0;
        }
        while (CountEnemyAlive == 0 && EnemySpawnerDown.CountEnemyAlive > 0)//��Ϸʤ���������ǵ��˶����ɡ�����Ҳ��������. ����0˵�����˻��д���ô��return 0����ͣ0֡
        {
            yield return 0;
        }

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(10);
    }
  
}
