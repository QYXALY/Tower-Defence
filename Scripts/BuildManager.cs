using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public TurretData laserTurretData;//��Inspector��彫Ԥ�������룬����д�����������
    public TurretData missileTurretData;//��Inspector��彫Ԥ�������룬����д�����������
    public TurretData standardTurretData;//��Inspector��彫Ԥ�������룬����д�����������

    //��ʾ��ǰѡ�����̨(Ҫ�������̨)
    private TurretData selectedTurretData;
    //UI����ʾ��ѡ�����̨��д������̨��ѡ�񷽷���
    //ͨ��ע��������̨��Toggle�¼���ʶ���ĸ���ѡ����


    AudioClip clip;
    AudioClip clip1;

    //��ʾ��ǰѡ�����̨(�����е���Ϸ����)
    private MapCube selectedMapCube;//3D������ѡ�����̨

    public Text moneyText;

    public Animator moneyAnimator;

    private int money = 500;//��ʼ��ǮΪ1000

    public GameObject upgradeCanvas;//����UI����

    private Animator upgradeCanvasAnimator;//����UI��ʾ���صĶ���ת��״̬��

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
        upgradeCanvasAnimator = upgradeCanvas.GetComponent<Animator>();//�õ�״̬��
    }
    void Update()
    {
        if (selectedTurretData !=null && Input.GetMouseButtonDown(0))
        {
            //��������UI���棬��������; EventSystem.current���ص���Hierarchy��EventSystem��EventSystem(Script)�����
            //IsPointerOverGameObject��ʾ����Ƿ�����UI��
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                //������̨�Ľ���,�����ж�����������ĸ�MapCube�ϣ���Ҫʹ�����߼���ˣ��õ�һ������ray
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//�����ĵ�ת��������
                RaycastHit hit;
                //Physics.Raycast���������߼�⣬�����ߣ�RaycastHit���߼���ʲô����������ײ�Ľ����maxDistance�����룬layerMask����һ�������߼���粻ָ�����Ǻ����еĲ㣩
                bool isCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube"));//�õ��Ƿ���ײ��MapCube��
                if (isCollider)
                {
                    //TODO ������̨
                    MapCube mapCube = hit.collider.GetComponent<MapCube>();
                    if (selectedTurretData != null && mapCube.turretGo == null)//ѡ����ĳ����̨��ui����map�ϻ�û������̨
                    {
                        //���Դ��� 
                        if (money > selectedTurretData.cost)
                        {
                            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 1);
                            ChangeMoney(-selectedTurretData.cost);
                            mapCube.BuildTurret(selectedTurretData);
                        }
                        else
                        {
                            //��ʾǮ����
                            moneyAnimator.SetTrigger("Flicker");
                        }
                    }
                    else if (mapCube.turretGo != null)
                    {
                        //����ڶ��ε������̨�˲���UI�ļ���������true
                        if (mapCube == selectedMapCube && upgradeCanvas.activeInHierarchy)
                        {
                            StartCoroutine(HideUpgradeUI());//��UI���أ���Э�̵ķ�ʽ
                        }
                        else
                        //������ʾ����/���UI��壬�ڶ���������boolֵ���Ƿ�����̨�ж���������Բ���if�ж�ֱ�Ӵ�����
                        {
                            ShowUpgradeUI(mapCube.transform.position, mapCube.isUpgraded);
                        }
                        selectedMapCube = mapCube;//�ѵ������̨�����������̨

                    }
                }

            }
        }
    }

    //��Canvas����豸����On Value Changed�����GameManager��Ȼ��ѡ���Ӧ�����淽����ֻҪ�ǵ���豸ֵ�����ı��ˣ�Ҳ����is on�����ı��ˣ����ᴥ��
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
        StopCoroutine("HideUpgradeUI");//���������HideUpgradeUIЭ�̷�����û�������У��еĻ��ȸ���ͣ����û��Ҳ����Ӱ�졣
        upgradeCanvas.SetActive(false);
        upgradeCanvas.SetActive(true);
        upgradeCanvas.transform.position = pos;
        buttonUpgrade.interactable = !isDisableUpgrade;//�������߽���������ť
    }

    IEnumerator HideUpgradeUI()
    {
        upgradeCanvasAnimator.SetTrigger("Hide");
        //upgradeCanvas.SetActive(false);
        yield return new WaitForSeconds(0.8f);//��ʧ��Ч����������ȥ��������
        upgradeCanvas.SetActive(false);//���ص�ʱ����ֱ�Ӱѻ������ã���Ȼ���޷����Ž��õĶ�����
    }

    public void OnUpgradeButtonDown()//�������������ķ���
    {
        if (money >= selectedMapCube.turretData.costUpgraded)//���������������Ҫ��Ǯ
        {
            ChangeMoney(-selectedMapCube.turretData.costUpgraded);
            selectedMapCube.UpgradeTurret();
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 1);
        }
        else
        {
            moneyAnimator.SetTrigger("Flicker");//��UI���ص�
        }

        StartCoroutine(HideUpgradeUI());
    }
    public void OnDestroyButtonDown()//���²�������ķ���
    {
        selectedMapCube.DestroyTurret();
        ChangeMoney(+selectedTurretData.destroymoney);
        StartCoroutine(HideUpgradeUI());
        AudioSource.PlayClipAtPoint(clip1, Camera.main.transform.position, 1);
    }

}
