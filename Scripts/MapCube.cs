using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCube : MonoBehaviour
{
    [HideInInspector]//不会在面板上显示该属性
    public GameObject turretGo;//保存当前cube身上的炮台
    [HideInInspector]
    public TurretData turretData;
    [HideInInspector]
    public bool isUpgraded = false;//是否是升级的

    public GameObject buildEffect;//建筑特效

    //private Renderer renderer;
    public void BuildTurret(TurretData turretData)
    {
        this.turretData = turretData;//保存一下当前炮台的数据，下面升级的时候连参数都不用传递了
        isUpgraded = false;
        //实例化炮塔
        turretGo = GameObject.Instantiate(turretData.turretPrefab, transform.position, Quaternion.identity);
        //实例化特效，接收一下返回值要销毁的
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f); //1.5秒的时间播放，然后销毁
    }
    public void UpgradeTurret()
    {
        if (isUpgraded == true) return;

        Destroy(turretGo);
        isUpgraded = true;
        turretGo = GameObject.Instantiate(turretData.turretUpgradedPrefab, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);//实例化建筑特效，接收一下返回值要销毁的
        Destroy(effect, 1.5f);
    }
    public void DestroyTurret()
    {
        Destroy(turretGo);
        isUpgraded = false;
        turretGo = null;
        turretData = null;
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);//实例化建筑特效，接收一下返回值要销毁的
        Destroy(effect, 1.5f);
    }
}
