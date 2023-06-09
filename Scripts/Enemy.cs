using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float speed = 4.5f;//敌人移动速度，每秒十米
    public float hp = 150;//敌人血量，public类型可以在unity中直接重新赋值修改
    private Transform[] positions;//
    private int index = 0;//默认的位置
    private float totalHp;
    public GameObject explosionEffect;
    public Slider hpSlider;

    public Text DieMoneyText;

    AudioClip clip;

    float num = 0f;
    void Start()
    {
        clip = Resources.Load("ExplosionSound", typeof(AudioClip)) as AudioClip;
        System.Random rd = new System.Random();
        num = rd.Next(0, 100);
        if (num >= 50)
        {
            positions = WayPointsUp.positions;
        }
        else
        {
            positions = WayPointsDown.positions;
        }
        totalHp = hp;
        hpSlider = GetComponentInChildren<Slider>();
    }

      
    void Update()
    {
        Move();
    }
    void Move()
    {
        if (index > positions.Length - 1) return;//当到达最后一个位置
        //（目标位置 - 当前位置）得到一个向量.单位化每次移动1，取得单位向量之后再做计算
        transform.Translate((positions[index].position - transform.position).normalized * Time.deltaTime * speed);
        //判断两个点的距离，也就是看敌人有没有到达目标位置，取得两个点位置是否小于一定距离
        if (Vector3.Distance(positions[index].position, transform.position) < 0.2f)
        {
            index++;
        }
        if (index > positions.Length - 1)//当到达最后一个位置
        {
            ReachDestination();
        }

    }
    //到达目的地，游戏就失败了
    void ReachDestination()
    {
        GameObject.Destroy(this.gameObject);
        SceneManager.LoadScene(9);
    }

    //被打掉销毁
    private void OnDestroy()
    {
        EnemySpawnerUp.CountEnemyAlive--;
        EnemySpawnerDown.CountEnemyAlive--;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 0.07f);
    }

    public void TakeDamage(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        hpSlider.value = (float)hp / totalHp;//hpSlider.value是一个0~1的值，所以它以百分比来计算得到
        if (hp <= 0)
        {
            Die();
            ReturnMoney();
        }
    }

    void Die()
    {
        GameObject effect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Destroy(this.gameObject);
    }

    void ReturnMoney()
    {
        GameObject.Find("GameManager").GetComponent<BuildManager>().ChangeMoney(15);
    }
}
