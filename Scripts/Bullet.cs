using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 50;//�˺�

    public float speed = 20;//�ٶ�

    public GameObject explosionEffectPrefab;//��ը��Ч

    private float distanceArriveTarget = 1.2f;

    private Transform target;//Ŀ��

    public void SetTarget(Transform _target)
    {
        this.target = _target;
    }

    void Update()//�����ӵ����ƶ�
    {
        if (target == null)//���Ŀ�겻������,�����е��ӵ���������
        {
            Die();
            return;
        }

        transform.LookAt(target.position);//����Ŀ���λ��
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Vector3 dir = target.position - transform.position;
       
        if (dir.magnitude < distanceArriveTarget)
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
            Die();
        }
       
    }

    void Die()
    {
        //������Ч
        GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        Destroy(effect, 1);
        Destroy(this.gameObject);//���������Ϸ����,���ֻ��this��ֻ������Bullet���
    }
}
