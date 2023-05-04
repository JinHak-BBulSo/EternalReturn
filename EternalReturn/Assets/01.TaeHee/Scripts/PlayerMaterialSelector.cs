using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterialSelector : MonoBehaviour
{
    private const string CHARACTER_SILHOUETTE_PATH = "00.Characters/SilhouetteMaterials/";

    private List<Renderer> playerModels = new List<Renderer>();

    public void InitializeMaterial()
    {
        if (PlayerManager.Instance.Player != gameObject)
            return;

        Renderer[] playerRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in playerRenderers)
        {
            if (renderer.GetType() == typeof(MeshRenderer) || renderer.GetType() == typeof(SkinnedMeshRenderer))
            {
                playerModels.Add(renderer);
            }
        }

        for (int i = 0; i < playerModels.Count; i++)
        {
            string materialName = playerModels[i].sharedMaterial.name;
            Material loadedMaterial = Resources.Load<Material>($"{CHARACTER_SILHOUETTE_PATH}{materialName}");

            if (loadedMaterial == null)
            {
                Debug.LogWarning("===Not found material===");
                continue;
            }

            playerModels[i].sharedMaterial = loadedMaterial;
        }
    }
}