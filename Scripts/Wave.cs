using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����ÿһ��������������Ҫ������
[System.Serializable]//��ʾ���Ա����л���������ʾ��Inspector����ϣ�
                     //��ȻEnemySpawner�Ǳ��޷����������ʽ����
public class Wave
{
    public GameObject enemyPrefab;//����������ɵ���
    public int count;//���ɸ���
    public float rate;//���ɵ�ʱ����
}
