using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCube : MonoBehaviour
{
    [HideInInspector]//�������������ʾ������
    public GameObject turretGo;//���浱ǰcube���ϵ���̨
    [HideInInspector]
    public TurretData turretData;
    [HideInInspector]
    public bool isUpgraded = false;//�Ƿ���������

    public GameObject buildEffect;//������Ч

    //private Renderer renderer;
    public void BuildTurret(TurretData turretData)
    {
        this.turretData = turretData;//����һ�µ�ǰ��̨�����ݣ�����������ʱ�������������ô�����
        isUpgraded = false;
        //ʵ��������
        turretGo = GameObject.Instantiate(turretData.turretPrefab, transform.position, Quaternion.identity);
        //ʵ������Ч������һ�·���ֵҪ���ٵ�
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f); //1.5���ʱ�䲥�ţ�Ȼ������
    }
    public void UpgradeTurret()
    {
        if (isUpgraded == true) return;

        Destroy(turretGo);
        isUpgraded = true;
        turretGo = GameObject.Instantiate(turretData.turretUpgradedPrefab, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);//ʵ����������Ч������һ�·���ֵҪ���ٵ�
        Destroy(effect, 1.5f);
    }
    public void DestroyTurret()
    {
        Destroy(turretGo);
        isUpgraded = false;
        turretGo = null;
        turretData = null;
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);//ʵ����������Ч������һ�·���ֵҪ���ٵ�
        Destroy(effect, 1.5f);
    }
}
