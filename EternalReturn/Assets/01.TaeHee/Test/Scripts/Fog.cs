using System;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public Material cookieCreator;
    public Material cookieBlur;
    public Transform center;
    public int castResolution = 128;
    public int textureResolution = 128;

    //public GameObject CastPointGO;
    public float castPointHeight = 1.5f;
    [Range(0, 12)]
    public int pastTexCount = 12;
    public float radius = 3;

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
        center = PlayerManager.Instance.Player.transform;
        cookieMask = new RenderTexture(textureResolution, textureResolution, 0);
        cookieBlurred = new RenderTexture(textureResolution, textureResolution, 0);
        //PastTexCount = CookieBlur.GetInt("_PastTexCount");
        pastMasks = new RenderTexture[pastTexCount];
        for (int i = 0; i < pastTexCount; i++)
        {
            pastMasks[i] = new RenderTexture(textureResolution / 2, textureResolution / 2, 0);
            cookieBlur.SetTexture("_PastTex" + i.ToString(), pastMasks[i]);
        }

        projector = GetComponent<Projector>();
        projector.material.SetTexture("_ShadowTex", cookieBlurred);
        projectorSize = projector.orthographicSize * 2;

        data = new float[castResolution * 2 + 3];
        data[0] = castResolution;
        d_angle = 360 * Mathf.Deg2Rad / castResolution;
    }

    public void SetCookie()
    {
        // cookieBlur.SetInt("_PastTexCount", pastTexCount);

        Vector3 castPoint = center.position;
        //castPoint.y = CastPointHeight;

        //castPoint.y = CastPointGO.transform.position.y;
        castPoint.y = center.position.y + castPointHeight - 1f;
        for (int i = 0; i < castResolution; i++)
        {
            Vector3 dir = Vector3.one;
            dir.x *= Mathf.Cos(d_angle * i);
            dir.z *= Mathf.Sin(d_angle * i);
            dir.y = 0;
            if (Physics.Raycast(castPoint, dir, out hit, radius))
            {
                data[3 + i * 2] = (hit.point.x - transform.position.x) / projectorSize + 0.5f;
                data[4 + i * 2] = (hit.point.z - transform.position.z) / projectorSize + 0.5f;
                //Debug.Log($"O {hit.transform.gameObject}");
            }
            else
            {
                Vector3 point = dir.normalized * radius + center.transform.position;
                data[3 + i * 2] = (point.x - transform.position.x) / projectorSize + 0.5f;
                data[4 + i * 2] = (point.z - transform.position.z) / projectorSize + 0.5f;
            }
        }
        data[1] = (castPoint.x - transform.position.x) / projectorSize + 0.5f;
        data[2] = (castPoint.z - transform.position.z) / projectorSize + 0.5f;
        cookieCreator.SetFloatArray("_Data", data);
        Graphics.Blit(null, cookieMask, cookieCreator);
        Graphics.Blit(cookieMask, cookieBlurred, cookieBlur);
        Graphics.Blit(cookieBlurred, pastMasks[pastMaskIndex]);

        pastMaskIndex = pastMaskIndex >= (pastTexCount - 1) ? 0 : pastMaskIndex + 1;
    }
}