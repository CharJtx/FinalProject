using UnityEngine;

public class SkyboxGenerator : MonoBehaviour
{
    public Material skyboxMaterial; // Skybox material to apply the textures to

    void Start()
    {
        // Generate 6 sides of star textures
        Texture2D[] starTextures = GenerateStarTextures();

        // Apply the textures to the skybox material
        ApplyTexturesToSkybox(starTextures);
    }

    Texture2D[] GenerateStarTextures()
    {
        int size = 512; // Size of the textures
        Texture2D[] textures = new Texture2D[6];

        for (int i = 0; i < 6; i++)
        {
            textures[i] = new Texture2D(size, size);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    // Generate a starry pattern (you can customize this)
                    Color color = Random.value > 0.999f ? Color.white : Color.black;
                    textures[i].SetPixel(x, y, color);
                }
            }
            textures[i].Apply();
        }

        return textures;
    }

    void ApplyTexturesToSkybox(Texture2D[] textures)
    {
        if (skyboxMaterial == null)
        {
            Debug.LogError("Skybox material not assigned.");
            return;
        }

        skyboxMaterial.SetTexture("_FrontTex", textures[0]);
        skyboxMaterial.SetTexture("_BackTex", textures[1]);
        skyboxMaterial.SetTexture("_LeftTex", textures[2]);
        skyboxMaterial.SetTexture("_RightTex", textures[3]);
        skyboxMaterial.SetTexture("_UpTex", textures[4]);
        skyboxMaterial.SetTexture("_DownTex", textures[5]);

        RenderSettings.skybox = skyboxMaterial;
    }
}
