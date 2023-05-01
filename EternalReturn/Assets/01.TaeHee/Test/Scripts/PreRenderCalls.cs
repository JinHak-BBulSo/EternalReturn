using UnityEngine;
using UnityEngine.Rendering;

public class PreRenderCalls : MonoBehaviour
{
    public Fog fog;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnPreRender()
    {
        if (timer < 0.1f)
            return;
        else
            timer = 0f;

        if (fog.gameObject.activeSelf)
        {
            fog.SetCookie();
        }
    }
}
