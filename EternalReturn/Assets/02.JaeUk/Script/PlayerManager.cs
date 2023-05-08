using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingleTonBase<PlayerManager>
{
    public GameObject canvas;
    public GameObject Player;
    public int PlayerNumber = 1;
    public int areaIndex = 6;
    public bool IsSelect;
    public int characterNum;
    public bool SelectChk;
    public int LoadingFinishPlayer;
    public bool IsGameStart;
    public Vector3 PlayerPos;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
}
