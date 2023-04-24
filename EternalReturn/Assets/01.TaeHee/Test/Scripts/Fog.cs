using System;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public Material CookieCreator;
    public Material CookieBlur;
    public Transform Center;
    public int CastResolution = 128;
    public int TextureResolution = 128;

    public GameObject CastPointGO;
    public float CastPointHeight = 1.5f;
    [Range(0, 12)]
    public int PastTexCount = 12;
    public float Radius = 3;

    private RenderTexture cookieMask, cookieBlurred;
    private Projector projector;
    private RaycastHit hit;
    [SerializeField] private float[] data, data_sec;
    private float projectorSize;
    private float d_angle;
    private RenderTexture[] pastMasks;
    private int pastMaskIndex = 0;

    private float timer = 0;

    private void Start()
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

        projector = GetComponent<Projector>();
        projector.material.SetTexture("_ShadowTex", cookieBlurred);
        projectorSize = projector.orthographicSize * 2;

        data = new float[CastResolution * 2 + 3];
        data[0] = CastResolution;
        d_angle = 360 * Mathf.Deg2Rad / CastResolution;
    }

    public void SetCookie()
    {
        CookieBlur.SetInt("_PastTexCount", PastTexCount);

        Vector3 castPoint = Center.position;
        //castPoint.y = CastPointHeight;

        castPoint.y = CastPointGO.transform.position.y;
        //castPoint.y = Center.position.y + CastPointHeight - 1f;
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
        Graphics.Blit(cookieBlurred, pastMasks[pastMaskIndex]);

        pastMaskIndex = pastMaskIndex >= (PastTexCount - 1) ? 0 : pastMaskIndex + 1;
    }
}