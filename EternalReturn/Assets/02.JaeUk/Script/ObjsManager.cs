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
    public GameObject CameraPivot;
    public PlayerMaterialSelector materialSelector;

    private void Update()
    {
        if (PlayerManager.Instance.IsGameStart)
        {
            itemCanvas.gameObject.SetActive(true);
            CameraPivot.GetComponent<MoveCamera>().enabled = true;
            CameraPivot.GetComponent<MoveCamera>().player = PlayerManager.Instance.Player.GetComponent<PlayerBase>();
            ItemManager.Instance.ItemCanvas = itemCanvas;
            PlayerUI.Instance.InitializeCharacterUI();
            materialSelector.InitializeMaterial();
            Destroy(this.gameObject);
        }
    }

}