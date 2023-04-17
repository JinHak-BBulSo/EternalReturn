using UnityEngine;
using UnityEngine.Rendering;

public class PreRenderCalls : MonoBehaviour
{
    public bool UsingURP;
    public Fog _Fog;

    void Awake()
    {
        if (UsingURP)
        {
            RenderPipelineManager.beginCameraRendering += PreRenderURP;
        }
    }

    void OnDisable()
    {
        if (UsingURP)
        {
            RenderPipelineManager.beginCameraRendering -= PreRenderURP;
        }
    }

    void PreRenderURP(ScriptableRenderContext renderContext, Camera obj)
    {
        Debug.Log("PreRender");
        OnPreRender();
    }

    void OnPreRender()
    {
        Debug.Log("Set");
        // FOG CALL
        _Fog.SetCookie();
    }
}
