using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fog : MonoBehaviour
{

    public bool URP;
    public Material CookieCreator;
    public Material CookieBlur;
    public Transform Center;
    public int CastResolution = 32;
    public int TextureResolution = 128;
    public float CastPointHeight = 1.5f;
    [Range(0, 12)]
    public int PastTexCount = 12;
    public float Radius = 3;

    private RenderTexture cookieMask, cookieBlurred;
    private Projector projector;
    private DecalProjector decal;
    private RaycastHit hit;
    [SerializeField] private float[] data, data_sec;
    private float projectorSize;
    private float d_angle;
    private RenderTexture[] pastMasks;
    private int c_pastMask = 0;

    private float timer = 0;

    void Start()
    {
        cookieMask = new RenderTexture(TextureResolution, TextureResolution, 0);
        cookieBlurred = new RenderTexture(TextureResolution, TextureResolution, 0);
        //PastTexCount = CookieBlur.GetInt("_PastTexCount");
        pastMasks = new RenderTexture[PastTexCount];
        for (int i = 0; i < PastTexCount; i++)
        {
            pastMasks[i] = new RenderTexture(TextureResolution / 2, TextureResolution / 2, 0);
            CookieBlur.SetTexture("_PastTex" + i.ToString(), pastMasks[i]);
        }
        if (URP)
        {
            decal = GetComponent<DecalProjector>();
            decal.material.SetTexture("_BaseColorMap", cookieBlurred);
            projectorSize = decal.size.x;
        }
        else
        {
            projector = GetComponent<Projector>();
            projector.material.SetTexture("_ShadowTex", cookieBlurred);
            projectorSize = projector.orthographicSize * 2;
        }

        data = new float[CastResolution * 2 + 3];
        data[0] = CastResolution;
        d_angle = 360 * Mathf.Deg2Rad / CastResolution;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public void SetCookie()
    {
        if (timer < 0.1f)
            return;
        else
            timer = 0f;

        CookieBlur.SetInteger("_PastTexCount", PastTexCount);

        Vector3 castPoint = Center.position;
        //castPoint.y = CastPointHeight;
        castPoint.y = Center.position.y + CastPointHeight - 1f;
        for (int i = 0; i < CastResolution; i++)
        {
            Vector3 dir = Vector3.one;
            dir.x *= Mathf.Cos(d_angle * i);
            dir.z *= Mathf.Sin(d_angle * i);
            dir.y = 0;
            if (Physics.Raycast(castPoint, dir, out hit, Radius))
            {
                data[3 + i * 2] = (hit.point.x - transform.position.x) / projectorSize + 0.5f;
                data[4 + i * 2] = (hit.point.z - transform.position.z) / projectorSize + 0.5f;
                Debug.Log($"O {hit.transform.gameObject}");
            }
            else
            {
                Vector3 point = dir.normalized * Radius + Center.transform.position;
                data[3 + i * 2] = (point.x - transform.position.x) / projectorSize + 0.5f;
                data[4 + i * 2] = (point.z - transform.position.z) / projectorSize + 0.5f;
                Debug.Log("X");
            }
        }
        data[1] = (castPoint.x - transform.position.x) / projectorSize + 0.5f;
        data[2] = (castPoint.z - transform.position.z) / projectorSize + 0.5f;
        CookieCreator.SetFloatArray("_Data", data);
        Graphics.Blit(null, cookieMask, CookieCreator);
        Graphics.Blit(cookieMask, cookieBlurred, CookieBlur);
        Graphics.Blit(cookieBlurred, pastMasks[c_pastMask++]);
        if (c_pastMask >= PastTexCount)
        {
            c_pastMask = 0;
        }
    }

}
