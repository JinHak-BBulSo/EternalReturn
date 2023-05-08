using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://rito15.github.io/posts/fog-of-war/
//[DefaultExecutionOrder(-100)]
public class FowManager : SingleTonBase<FowManager>
{
    private Material _fogMaterial;
    public GameObject _rendererPrefab;

    void InitFogTexture()
    {
        var renderer = Instantiate(_rendererPrefab, transform);
        renderer.transform.localPosition = Vector3.zero;
        renderer.transform.localScale = new Vector3(_fogWidthX * 0.5f, 1, _fogWidthZ * 0.5f);
        _fogMaterial = renderer.GetComponentInChildren<Renderer>().material;
    }

    void UpdateFogTexture()
    {
        if (Map.FogTexture != null)
        {
            _fogMaterial.SetTexture("_MainTex", Map.FogTexture);
        }
    }

    [Space]
    public LayerMask _mapLayer;
    public float _fogWidthX = 40;
    public float _fogWidthZ = 40;
    public float _tileSize = 1;       // 타일 하나의 크기
    public float _updateCycle = 0.5f; // 시야 계산 주기

    [System.Serializable]
    public class FogAlpha
    {
        [Range(0, 1)] public float sight = 0.0f;
        [Range(0, 1)] public float fog = 0.96f;
    }
    [Space]
    public FogAlpha _fogAlpha = new FogAlpha();

    [Space]
    public bool _showSightGizmos = true;
    public bool _showMapGizmos = true;

    public FowMap Map { get; private set; }

    /// <summary> 각 타일에 위치한 지형들의 높이 </summary>
    private float[,] heightMap;
    private List<AllyFowUnit> allyUnitList; // Fow 시스템이 추적할 유닛들
    private List<FoeFowUnit> foeUnitList; // Fow 시스템이 추적할 유닛들

    protected override void Awake()
    {
        base.Awake();
        allyUnitList = new List<AllyFowUnit>();
        foeUnitList = new List<FoeFowUnit>();
        InitMap();
        InitFogTexture();

        Debug.Log(Application.targetFrameRate);
    }
    private void OnEnable()
    {
        StartCoroutine(UpdateFogRoutine());
    }

    protected override void Update()
    {
        base.Update();
        Map.LerpBlur();
        UpdateFogTexture();
    }

    private void OnDestroy()
    {
        Map.Release();
    }

    public static void AddAllyUnit(AllyFowUnit unit)
    {
        if (!Instance.allyUnitList.Contains(unit))
        {
            Instance.allyUnitList.Add(unit);
        }
    }
    public static void RemoveAllyUnit(AllyFowUnit unit)
    {
        if (Instance.allyUnitList.Contains(unit))
        {
            Instance.allyUnitList.Remove(unit);
        }
    }

    public static void AddFoeUnit(FoeFowUnit unit)
    {
        if (!Instance.foeUnitList.Contains(unit))
        {
            Instance.foeUnitList.Add(unit);
        }
    }
    public static void RemoveFoeUnit(FoeFowUnit unit)
    {
        if (Instance.foeUnitList.Contains(unit))
        {
            Instance.foeUnitList.Remove(unit);
        }
    }

    public void InitMap()
    {
        heightMap = new float[(int)(_fogWidthX / _tileSize), (int)(_fogWidthZ / _tileSize)];
        for (int i = 0; i < heightMap.GetLength(0); i++)
        {
            for (int j = 0; j < heightMap.GetLength(1); j++)
            {
                var tileCenter = GetTileCenterPoint(i, j);

                // -Y방향 레이캐스트를 통해 지형 높이 구하기
                Vector3 ro = new Vector3(tileCenter.x, 100f, tileCenter.y);
                Vector3 rd = Vector3.down;

                float height = 0f;
                if (Physics.Raycast(ro, rd, out var hit, 200f, _mapLayer))
                {
                    height = hit.point.y;
                }

                heightMap[i, j] = height;
            }
        }

        Map = new FowMap();
        Map.InitMap(heightMap);
    }

    /// <summary> 대상 유닛의 위치를 타일좌표(x, y, height)로 환산 </summary>
    public TilePos GetTilePos(FowUnit unit)
    {
        int x = (int)((unit.transform.position.x - transform.position.x + _fogWidthX * 0.5f) / _tileSize);
        int y = (int)((unit.transform.position.z - transform.position.z + _fogWidthZ * 0.5f) / _tileSize);
        float height = unit.transform.position.y;

        return new TilePos(x, y, height);
    }

    /// <summary> 해당 (x,y) 인덱스의 타일 중심 좌표 구하기 </summary>
    private Vector2 GetTileCenterPoint(in int x, in int y)
    {
        return new Vector2(
            x * _tileSize + _tileSize * 0.5f - _fogWidthX * 0.5f,
            y * _tileSize + _tileSize * 0.5f - _fogWidthZ * 0.5f
        );
    }

    public IEnumerator UpdateFogRoutine()
    {
        var waitForSeconds = new WaitForSeconds(_updateCycle);

        while (true)
        {
            if (Map != null)
            {
                Map.RefreshFog();

                foreach (var unit in allyUnitList)
                {
                    TilePos pos = GetTilePos(unit);
                    Map.ComputeFog(pos, unit.sightRange / _tileSize, unit.sightHeight, out visibleTiles);
                }

                foreach (var unit in foeUnitList)
                {
                    TilePos pos = GetTilePos(unit);

                    foreach (var tile in visibleTiles)
                    {
                        unit.isHidden = true;
                        if (new Vector2Int(tile.pos.x, tile.pos.y) == new Vector2Int(pos.x, pos.y))
                        {
                            //Debug.Log($"[!!!] {unit.gameObject}: {new Vector2Int(tile.pos.x, tile.pos.y)} {new Vector2Int(pos.x, pos.y)}");
                            unit.isHidden = false;
                            break;
                        }
                    }
                }
            }

            yield return waitForSeconds;
        }
    }

    List<FowTile> visibleTiles = new List<FowTile>();

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (_showSightGizmos)
        {
            foreach (var tile in visibleTiles)
            {
                Vector2 pos = GetTileCenterPoint(tile.X, tile.Y);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(new Vector3(pos.x, 0f, pos.y), new Vector3(_tileSize, 1f, _tileSize));
            }
        }

        if (!_showMapGizmos)
            return;

        if (heightMap != null)
        {
            // 전체 타일 그리드, 장애물 그리드 보여주기
            for (int i = 0; i < heightMap.GetLength(0); i++)
            {
                for (int j = 0; j < heightMap.GetLength(1); j++)
                {
                    Vector2 center = GetTileCenterPoint(i, j);

                    Gizmos.color = new Color(heightMap[i, j] - transform.position.y, 0.1f, 0.1f);
                    Gizmos.DrawWireCube(new Vector3(center.x, heightMap[i, j] / 2, center.y), new Vector3(_tileSize - 0.02f, heightMap[i, j], _tileSize - 0.02f));
                }
            }
            foreach (var unit in allyUnitList)
            {
                TilePos tilePos = GetTilePos(unit);
                Vector2 center = GetTileCenterPoint(tilePos.x, tilePos.y);

                Gizmos.color = Color.blue;
                Gizmos.DrawCube(new Vector3(center.x, 0f, center.y),
                    new Vector3(_tileSize, 1f, _tileSize));

            }
            foreach (var unit in foeUnitList)
            {
                TilePos tilePos = GetTilePos(unit);
                Vector2 center = GetTileCenterPoint(tilePos.x, tilePos.y);

                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(new Vector3(center.x, 0f, center.y),
                    new Vector3(_tileSize, 1f, _tileSize));

            }
        }
    }
}