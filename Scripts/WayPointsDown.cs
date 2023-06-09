using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointsDown : MonoBehaviour
{
    public static Transform[] positions; //����һ������positions����transform���һ�����飬ֻ���������������ƶ���translate���ȷ���
    //һ����Ҫ��ȡÿ������������꣬���ᶨ��Transform������飬����ϰ�߽���������Ϊpositions

    void Awake()
    {   //ע�����������transform.GetComponent���ַ����������������Ҳ���ϣ�����Ҫ������ķ�ʽ
        positions = new Transform[transform.childCount];//��ÿ�����鿪�ٿռ䣬�ȴӺ��ӵ�λ���������С
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.GetChild(i);//�����������õ�ÿһ����λ��
        }
    }
}
