using System.Collections;
using UnityEngine;

public class AyunCounter : MonoBehaviour, IIneractionable
{
    //private NotifyImageComponent notifyImageComponent;

    //private WaitingPointContainer waitingPointContainer;
    //public WaitingPointContainer WaitingPointContainer => waitingPointContainer;

    private MoneyDummy moneyDummy;

    private bool isLooping = false;

    private void Awake()
    {
        //notifyImageComponent = GetComponent<NotifyImageComponent>();
        //waitingPointContainer = GetComponent<WaitingPointContainer>();
        moneyDummy = transform.Find("MoneyDummy").GetComponent<MoneyDummy>();
    }

    public void EnterInteraction()
    {
        //notifyImageComponent.SetNotifySensorImage(1.1f);

        if (false == isLooping)
        {
            Debug.Log("들어옴");
            StartCoroutine(CheckPayLoop());
        }
    }

    public void ExitInteraction()
    {
        Debug.Log("나감");
        //notifyImageComponent.SetNotifySensorImage(1.0f);
    }

    // 지불 확인 작업 (플레이어가 카운터에 상호작용하고 있을 때만 실행)
    private IEnumerator CheckPayLoop()
    {
        yield return null;
    }
}
