using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingContor : MonoBehaviour
{
    public Image[] LoadingImg;
    public Text LoadingText;
    bool ischk = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartLoading");
    }

    // Update is called once per frame
    void Update()
    {
        if (ischk && RoomCheckManager.Instance.isEnter)
        {
            StartCoroutine("EndLoading");
        }
    }
    IEnumerator StartLoading()
    {
        StartCoroutine(FadeIn(LoadingImg[0]));
        yield return new WaitForSeconds(4f);
        StartCoroutine(FadeIn(LoadingImg[1]));
        StartCoroutine(FadeIn(LoadingImg[2]));
        StartCoroutine(FadeIn(LoadingText));
        yield return new WaitForSeconds(4f);
        ischk = true;

    }
    IEnumerator EndLoading()
    {
        StartCoroutine(FadeOut(GetComponent<Image>()));
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);

    }
    IEnumerator FadeIn(Image image)
    {
        float time = 2f;
        float frameTime = time / 240f;
        float alpha = image.color.a;
        while (alpha <= 1)
        {
            alpha += 0.01f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return new WaitForSeconds(frameTime);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeOut(image));

    }
    IEnumerator FadeIn(Text txt)
    {
        float time = 2f;
        float frameTime = time / 240f;
        float alpha = txt.color.a;
        while (alpha <= 1)
        {
            alpha += 0.01f;
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha);
            yield return new WaitForSeconds(frameTime);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeOut(txt));

    }
    IEnumerator FadeOut(Image image)
    {
        float time = 2f;
        float frameTime = time / 240f;
        float alpha = image.color.a;
        while (alpha >= 0)
        {
            alpha -= 0.01f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return new WaitForSeconds(frameTime);
        }

    }
    IEnumerator FadeOut(Text txt)
    {
        float time = 2f;
        float frameTime = time / 240f;
        float alpha = txt.color.a;
        while (alpha >= 0)
        {
            alpha -= 0.01f;
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha);
            yield return new WaitForSeconds(frameTime);
        }

    }


}
