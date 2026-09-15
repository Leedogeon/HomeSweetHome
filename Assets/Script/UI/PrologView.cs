using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrologView : MonoBehaviour
{
    public GameObject[] Imgs;
    public CanvasGroup panel;
    float FadeOutTime = 1.5f;
    private int index = 0;
    bool chk = true;
    void Update()
    {
        if(chk)
        {
            // 클릭, 버튼이 아닐시 작동
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIButton())
            {
                if (index == Imgs.Length-1)
                {
                    StartCoroutine(FadeOut(panel));
                }
                else
                {
                    StartCoroutine(SlideShow(Imgs[index]));
                    index++;

                }

            }
        }

    }
    IEnumerator SlideShow(GameObject target)
    {
        float z = 0f;
        float timer = 0f;

        // 이미지라서 RectTransform을 이용해야됨
        RectTransform rt = target.GetComponent<RectTransform>();

        while (timer < 2f)
        {
            timer += Time.deltaTime;

            if (rt != null)
            {
                rt.anchoredPosition += new Vector2(6f, -11f) * Time.deltaTime * 100f;
                rt.rotation = Quaternion.Euler(0, 0, z);
            }

            z += Time.deltaTime * -40f;

            yield return null;
        }
    }
    bool IsPointerOverUIButton()
    {
        // 현재 마우스 위치를 저장
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        // 화면에 있는 모든 UI 요소들을 raycast해서 마우스 포인터 아래에 있는 요소들을 순서대로 저장
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // 버튼 컴포넌트가 있는지 리턴
        return results.Exists(r => r.gameObject.GetComponent<Button>() != null);
    }



    IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        chk = false;
        float startAlpha = 0f;
        float endAlpha = 1f;

        float time = 0f;
        while (time < FadeOutTime)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / FadeOutTime);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        if(Imgs.Length ==4)
        SceneManager.LoadSceneAsync("SampleScene");
        else SceneManager.LoadSceneAsync("MainMenu");
    }
    public void SkipBTN()
    {
        if (!chk) return;
        StartCoroutine(FadeOut(panel));
    }
}
