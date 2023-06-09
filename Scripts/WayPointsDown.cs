using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointsDown : MonoBehaviour
{
    public static Transform[] positions; //定义一个变量positions属于transform类的一个数组，只有这个组件里面有移动（translate）等方法
    //一般需要获取每个子物体的坐标，都会定义Transform类的数组，我们习惯将数组命名为positions

    void Awake()
    {   //注意这里如果用transform.GetComponent这种方法，会把自身的组件也带上，所以要用下面的方式
        positions = new Transform[transform.childCount];//给每个数组开辟空间，先从孩子点位里获得数组大小
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.GetChild(i);//根据索引来得到每一个子位置
        }
    }
}
