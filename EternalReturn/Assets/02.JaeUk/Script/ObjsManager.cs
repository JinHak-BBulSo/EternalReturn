using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjsManager : MonoBehaviour
{

    private static ObjsManager instance;

    public static ObjsManager Instance
    {
        get
        {
            if (instance == null)
            {

                if (instance == null)
                {
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<ObjsManager>();

                }
            }
            return instance;
        }
    }
    public GameObject itemCanvas;
    public GameObject fog;
    public GameObject CameraPivot;

    private void Update()
    {
        if (PlayerManager.Instance.IsGameStart)
        {
            itemCanvas.gameObject.SetActive(true);
            fog.gameObject.SetActive(true);
            CameraPivot.GetComponent<MoveCamera>().enabled = true;
            CameraPivot.GetComponent<MoveCamera>().player = PlayerManager.Instance.Player.GetComponent<PlayerBase>();
            ItemManager.Instance.Player = PlayerManager.Instance.Player.GetComponent<PlayerBase>();
            ItemManager.Instance.ItemCanvas = itemCanvas;
            PlayerUI.Instance.InitializeCharacterUI();
            PlayerManager.Instance.Player.GetComponent<PlayerMaterialSelector>().InitializeMaterial();
            Destroy(this.gameObject);
        }
    }

}