using UnityEngine;
using UnityEngine.UI;

enum CardState
{
    Cooling,
    WaitingSun,
    Ready
}

public class Card : MonoBehaviour
{
    public PlantData plantData; // PlantData
    public Image cardMask; //遮罩，冷却用
    public GameObject cardLight; //正常状态
    public GameObject cardGray; //不可用状态

    private float currTimer = 0f; //冷却计时器
    private CardState cardState = CardState.Cooling; //卡片状态

    private void Start()
    {
        cardMask.fillAmount = 1f;
    }

    private void Update()
    {
        switch (cardState)
        {
            case CardState.Cooling:
                CoolingUpdate();
                break;
            case CardState.WaitingSun:
                WaitingSunUpdate();
                break;
            case CardState.Ready:
                ReadyUpdate();
                break;
        }
    }


    //冷却状态处理
    private void CoolingUpdate()
    {
        currTimer += Time.deltaTime;
        cardMask.fillAmount = (plantData.cdTime - currTimer) / plantData.cdTime;

        if (currTimer >= plantData.cdTime)
            TransitionToWaitingSun();
    }

    //等待阳光状态处理
    private void WaitingSunUpdate()
    {
        if (plantData.sunCost <= SunManager.Instance.getSunPoint())
            TransitionToReady();
    }

    //准备终止状态处理
    private void ReadyUpdate()
    {
        if (plantData.sunCost > SunManager.Instance.getSunPoint())
            TransitionToWaitingSun();
    }

    //卡片可点击
    public void OnClick()
    {
        if (cardState != CardState.Ready) return;

        if (plantData.sunCost > SunManager.Instance.getSunPoint()) return;

        SunManager.Instance.setSunPoint(SunManager.Instance.getSunPoint() - plantData.sunCost);
        PlantManager.Instance.SelectPlant(plantData); // 工厂模式种植
        TransitionToCooling();
    }


    private void TransitionToWaitingSun()
    {
        cardState = CardState.WaitingSun;
        cardLight.SetActive(false);
        cardGray.SetActive(true);
        cardMask.gameObject.SetActive(false);
    }

    private void TransitionToReady()
    {
        cardState = CardState.Ready;
        cardLight.SetActive(true);
        cardGray.SetActive(false);
        cardMask.gameObject.SetActive(false);
    }

    private void TransitionToCooling()
    {
        cardState = CardState.Cooling;
        currTimer = 0;
        cardLight.SetActive(false);
        cardGray.SetActive(true);
        cardMask.gameObject.SetActive(true);
    }
}
