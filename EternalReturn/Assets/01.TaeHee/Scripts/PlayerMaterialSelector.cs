using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerMaterialSelector : MonoBehaviour
{
    private const string CHARACTER_SILHOUETTE_PATH = "00.Characters/SilhouetteMaterials/";

    [SerializeField] private List<Renderer> playerModels = new List<Renderer>();

    private void Awake()
    {
        Renderer[] playerRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in playerRenderers)
        {
            Debug.Log("GetType" + renderer.GetType());
            if (renderer.GetType() == typeof(MeshRenderer) || renderer.GetType() == typeof(SkinnedMeshRenderer))
            {
                playerModels.Add(renderer);
            }
        }

        for (int i = 0; i < playerModels.Count; i++)
        {
            string materialName = playerModels[i].sharedMaterial.name.Replace("(instance)", "");

            Debug.Log("[Mat name]" + materialName);
            playerModels[i].sharedMaterial = Resources.Load<Material>($"{CHARACTER_SILHOUETTE_PATH}{materialName}");

            if (playerModels[i].sharedMaterial == null)
            {
                Debug.LogWarning("===Not found material===");
                Material newMaterial = new Material(playerModels[i].material);
                newMaterial.shader = Shader.Find("Custom/Silhouette");
                newMaterial.name = materialName;
                File.Create($"Assets/{CHARACTER_SILHOUETTE_PATH}{materialName}.mat");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}