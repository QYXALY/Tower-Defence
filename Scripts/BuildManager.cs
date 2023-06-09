using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public TurretData laserTurretData;//在Inspector面板将预制体拉入，并填写其它相关数据
    public TurretData missileTurretData;//在Inspector面板将预制体拉入，并填写其它相关数据
    public TurretData standardTurretData;//在Inspector面板将预制体拉入，并填写其它相关数据

    //表示当前选择的炮台(要建造的炮台)
    private TurretData selectedTurretData;
    //UI上显示和选择的炮台，写三个炮台的选择方法，
    //通过注册三个炮台的Toggle事件来识别哪个被选择了


    AudioClip clip;
    AudioClip clip1;

    //表示当前选择的炮台(场景中的游戏物体)
    private MapCube selectedMapCube;//3D场景中选择的炮台

    public Text moneyText;

    public Animator moneyAnimator;

    private int money = 500;//初始金钱为1000

    public GameObject upgradeCanvas;//升级UI画板

    private Animator upgradeCanvasAnimator;//升级UI显示隐藏的动画转换状态机

    public Button buttonUpgrade;

    public void ChangeMoney(int change = 0)
    {
        money += change;
        moneyText.text = "$" + money;
    }

    void Start()
    {
        clip = Resources.Load("turret", typeof(AudioClip)) as AudioClip;
        clip1 = Resources.Load("chaichu", typeof(AudioClip)) as AudioClip;
        upgradeCanvasAnimator = upgradeCanvas.GetComponent<Animator>();//得到状态机
    }
    void Update()
    {
        if (selectedTurretData !=null && Input.GetMouseButtonDown(0))
        {
            //如果鼠标在UI上面，则不做处理; EventSystem.current返回的是Hierarchy里EventSystem里EventSystem(Script)组件。
            //IsPointerOverGameObject表示鼠标是否按在了UI上
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                //开发炮台的建造,首先判断鼠标点击到了哪个MapCube上，就要使用射线检测了，得到一个射线ray
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//把鼠标的点转化成射线
                RaycastHit hit;
                //Physics.Raycast来进行射线检测，（射线，RaycastHit射线检测跟什么东西做了碰撞的结果，maxDistance最大距离，layerMask和哪一层做射线检测如不指定就是和所有的层）
                bool isCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube"));//得到是否碰撞到MapCube上
                if (isCollider)
                {
                    //TODO 创建炮台
                    MapCube mapCube = hit.collider.GetComponent<MapCube>();
                    if (selectedTurretData != null && mapCube.turretGo == null)//选中了某个炮台的ui并且map上还没创建炮台
                    {
                        //可以创建 
                        if (money > selectedTurretData.cost)
                        {
                            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 1);
                            ChangeMoney(-selectedTurretData.cost);
                            mapCube.BuildTurret(selectedTurretData);
                        }
                        else
                        {
                            //提示钱不够
                            moneyAnimator.SetTrigger("Flicker");
                        }
                    }
                    else if (mapCube.turretGo != null)
                    {
                        //如果第二次点击此炮台了并且UI的激活属性是true
                        if (mapCube == selectedMapCube && upgradeCanvas.activeInHierarchy)
                        {
                            StartCoroutine(HideUpgradeUI());//将UI隐藏，用协程的方式
                        }
                        else
                        //否则显示升级/拆除UI面板，第二个参数的bool值与是否有炮台判断相符，所以不再if判断直接传即可
                        {
                            ShowUpgradeUI(mapCube.transform.position, mapCube.isUpgraded);
                        }
                        selectedMapCube = mapCube;//把点击的炮台赋给点击的炮台

                    }
                }

            }
        }
    }

    //在Canvas里的设备里有On Value Changed里添加GameManager，然后选择对应的下面方法，只要是点击设备值发生改变了，也就是is on发生改变了，都会触发
    public void OnLaserSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = laserTurretData;
        }
    }
    public void OnMissileSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = missileTurretData;
        }
    }
    public void OnStandardSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = standardTurretData;
        }
    }

    void ShowUpgradeUI(Vector3 pos, bool isDisableUpgrade = false)
    {
        StopCoroutine("HideUpgradeUI");//搜索下面的HideUpgradeUI协程方法有没有在运行，有的话先给暂停掉，没有也不会影响。
        upgradeCanvas.SetActive(false);
        upgradeCanvas.SetActive(true);
        upgradeCanvas.transform.position = pos;
        buttonUpgrade.interactable = !isDisableUpgrade;//开启或者禁用升级按钮
    }

    IEnumerator HideUpgradeUI()
    {
        upgradeCanvasAnimator.SetTrigger("Hide");
        //upgradeCanvas.SetActive(false);
        yield return new WaitForSeconds(0.8f);//消失的效果结束后再去调用下面
        upgradeCanvas.SetActive(false);//隐藏的时候不能直接把画布禁用，不然就无法播放禁用的动画了
    }

    public void OnUpgradeButtonDown()//按下升级触发的方法
    {
        if (money >= selectedMapCube.turretData.costUpgraded)//如果大于升级所需要的钱
        {
            ChangeMoney(-selectedMapCube.turretData.costUpgraded);
            selectedMapCube.UpgradeTurret();
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 1);
        }
        else
        {
            moneyAnimator.SetTrigger("Flicker");//把UI隐藏掉
        }

        StartCoroutine(HideUpgradeUI());
    }
    public void OnDestroyButtonDown()//按下拆除触发的方法
    {
        selectedMapCube.DestroyTurret();
        ChangeMoney(+selectedTurretData.destroymoney);
        StartCoroutine(HideUpgradeUI());
        AudioSource.PlayClipAtPoint(clip1, Camera.main.transform.position, 1);
    }

}
