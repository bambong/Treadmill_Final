using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class GameStartProduction : MonoBehaviour
{
    [SerializeField] UnityEvent onProductionEnded;

    [SerializeField] UiShadow uiShadow;
    [SerializeField] Transform Count_3_Tf;
    [SerializeField] Transform Count_2_Tf;
    [SerializeField] Transform Count_1_Tf;
    [SerializeField] PausePageController pauseController;

    [SerializeField] Vector2 countSize;

    void Awake()
    {
        countSize = Count_3_Tf.localScale;
    }

    public void ProudctionStart()
    {
        if (production_C != null)
            StopCoroutine(production_C);
        production_C = productionC();
        StartCoroutine(production_C);
    }


    WaitForSeconds shadowOn_WFS = new WaitForSeconds(1.5f);
    WaitForSeconds oneSecond_WFS = new WaitForSeconds(0.7f);
    WaitForSeconds oneSecond_WFS2 = new WaitForSeconds(0.3f);
    IEnumerator production_C;
    IEnumerator productionC()
    {
        //±×¸²ÀÚ ÄÑÁü
        uiShadow.Active(true);
        pauseController.PauseBtnInteractBlockActive(true);

        yield return shadowOn_WFS;

        //3
        Count_3_Tf.gameObject.SetActive(true);
        Count_3_Tf.localScale = Vector2.zero;
        Count_3_Tf.DOKill();
        Count_3_Tf.DOScale(countSize, 0.7f).SetEase(Ease.OutBounce);

        yield return oneSecond_WFS;

        Count_3_Tf.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutQuart);

        yield return oneSecond_WFS2;

        //2
        Count_2_Tf.gameObject.SetActive(true);
        Count_2_Tf.localScale = new Vector2(0, 0);
        Count_2_Tf.DOKill();
        Count_2_Tf.DOScale(countSize, 0.7f).SetEase(Ease.OutBounce);

        yield return oneSecond_WFS;

        Count_2_Tf.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutQuart);

        yield return oneSecond_WFS2;

        //1
        Count_1_Tf.gameObject.SetActive(true);
        Count_1_Tf.localScale = new Vector2(0, 0);
        Count_1_Tf.DOKill();
        Count_1_Tf.DOScale(countSize, 0.7f).SetEase(Ease.OutBounce);

        yield return oneSecond_WFS;

        Count_1_Tf.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutQuart);

        yield return oneSecond_WFS2;

        //±×¸²ÀÚ ²¨Áü
        uiShadow.Active(false);
        pauseController.PauseBtnInteractBlockActive(false);




        yield return oneSecond_WFS2;
        onProductionEnded.Invoke();
    }
}