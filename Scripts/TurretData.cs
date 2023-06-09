using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretData
{
    public GameObject turretPrefab;//������ģ��
    public int cost;//�۸�
    public GameObject turretUpgradedPrefab;//������ģ��
    public int destroymoney;//�������Ǯ
    public int costUpgraded;//�����ļ۸�
    public TurretType type;
}

public enum TurretType
{
    LaserTurret,//������̨
    MissileTurret,//������̨
    StandardTurret,//��׼��̨
}