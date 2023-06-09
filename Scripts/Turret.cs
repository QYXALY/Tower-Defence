using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private List<GameObject> enemys = new List<GameObject>();
    //���뵽�����¼���ֻ�������Ƚ��빥����Χ�ڵĵ���
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")//ͨ����ǩ���жϵ���
        {
            enemys.Add(col.gameObject);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
        {
            enemys.Remove(col.gameObject);
        }
    }

    public float attackRateTime = 1; //�����빥��һ��
    private float timer = 0;//���ü�ʱ���ж�ʱ���Ƿ�

    public GameObject bulletPrefab;//�������ӵ�,����Prefab�е�bulletԤ������קһ��
    public Transform firePosition;//�������ڿ�λ��,��ֱ����ק��StandarTurret����ק�������FirePosition��
    public Transform head;//������ͷ��

    public bool useLaser = false;//��ʾ�Ƿ�ʹ�ü���

    public float damageRate = 70;//������˺�ֵ/��

    public LineRenderer laserRenderer;//������

    public GameObject laserEffect;

    void Start()
    {
        timer = attackRateTime;
    }

    void Update()
    {
        if (enemys.Count > 0 && enemys[0] != null)
        {
            Vector3 targetPosition = enemys[0].transform.position;
            targetPosition.y = head.position.y;//������˺�head��z��Ĵ�ֱ����һ�£�Ҳ���Ǹ߶Ȳ�һ�£�����yֵ��Ⱥ�������
            head.LookAt(targetPosition);//����õ���
        }
        if (useLaser == false)
        {   //�ȵ����귽���ٽ��й���
            timer += Time.deltaTime;
            if (enemys.Count > 0 && timer >= attackRateTime)//����ео����Ҵ��ڹ���ʱ�䣬��timer���㣬��ʼ���й���
            {
                timer = 0;
                Attack();
            }
        }
        else if (enemys.Count > 0)//����һ���жϣ����������е���
        {
            //��Ч�������
            if (laserRenderer.enabled == false)
                laserRenderer.enabled = true;
            laserEffect.SetActive(true);

            if (enemys[0] == null) ; //�����һ�ŵ���Ϊ�գ��͸��µ���
            {
                UpdateEnemys();
            }
            if (enemys.Count > 0)//��������˺����ж��Ƿ��е��ˡ�
            {
                laserRenderer.SetPositions(new Vector3[] { firePosition.position, enemys[0].transform.position });
                enemys[0].GetComponent<Enemy>().TakeDamage(damageRate * Time.deltaTime);
                laserEffect.transform.position = enemys[0].transform.position;//������Чλ������˵�λ����ͬʱ����

                //��Ϊÿһ����̨��λ�ö��Ǹ�Cube��λ�ñ���һ�µģ�������ȡ����̨��λ�ã�
                //�������̨��Y�͵��˵ı���һ�£���Ϊ��Ч���ڵ��˵�λ�ã�Ȼ������Ч�������λ�á�
                Vector3 pos = transform.position;
                pos.y = enemys[0].transform.position.y;
                laserEffect.transform.LookAt(pos);
            }
        }
        else
        {
            laserEffect.SetActive(false);//��Ч�������
            laserRenderer.enabled = false;//�뿪��Ұ�ˣ���ʹ�õ�ʱ��Ѽ������
        }
    }

    void Attack()
    {
        if (enemys[0] == null)//���ڿգ�����ζ�ż������Ѿ��п�Ԫ����
        {
            UpdateEnemys();//��ʱ���µ���
        }
        if (enemys.Count > 0)
        {
            //ʵ�����ӵ�������Ĳ���Ϊ����ʵ����˭����ʼλ�ã��Ƿ���ת��
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform);////����Ŀ��Ĭ��ʹ�ü����еĵ�һ��Ԫ��
        }
        else
        {
            timer = attackRateTime;//���û�е��˾͹����ڵ�״̬���´������˾Ϳ���ֱ�ӽ��й�����
        }
    }

    void UpdateEnemys()
    {
        //enemys.RemoveAll(null);
        List<int> emptyIndex = new List<int>();
        for (int index = 0; index < enemys.Count; index++)
        {
            if (enemys[index] == null)
            {
                emptyIndex.Add(index);
            }
        }

        for (int i = 0; i < emptyIndex.Count; i++)
        {
            enemys.RemoveAt(emptyIndex[i] - i);//��Ϊÿɾ��һ������ߵ�ֵ�ͻ���ǰ�ƶ�һ��������Ҫ��һ�´��¶�λ��"-i"����
        }
    }
}
