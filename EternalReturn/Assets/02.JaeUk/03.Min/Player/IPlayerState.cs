
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void StateEnter(PlayerController Controller_);
    void StateFixedUpdate();
    void StateUpdate();
    void StateExit();

}
