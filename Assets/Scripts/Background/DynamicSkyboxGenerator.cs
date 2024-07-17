using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DynamicSkyboxGenerator : MonoBehaviour
{
    public Material skyboxMaterial;
    //public Texture2D[] textures;
    //public Texture2D[] textures2;
    public string folderPath = "";
    


    // Start is called before the first frame update
    void Start()
    {

        string resourcePath = Path.Combine(Application.dataPath, folderPath).Replace("\\", "/");
        string[] subdirectories;

        if (Directory.Exists(resourcePath))
        {
            subdirectories = Directory.GetDirectories(resourcePath,"*", SearchOption.AllDirectories);

            List<string> folderNames = new List<string>();
            foreach (string folder in subdirectories)
            {
                string relativePath = folder.Replace(Application.dataPath, "").Replace("\\", "/").Replace("/Resources/","");

                Texture2D[] testOfTextures = Resources.LoadAll<Texture2D>(relativePath);
                if (testOfTextures.Length < 6) continue;

                folderNames.Add(relativePath);
            }

            if (folderNames.Count < 0)
            {
                Debug.LogError("Do not find sub Folder");
            }
            else
            {
                Debug.Log(folderNames);
            }



            string selectFolder = folderNames[Random.Range(0, folderNames.Count)];

            Texture2D[] textures = Resources.LoadAll<Texture2D>(selectFolder);



            ShuffleTextures(textures);
            ApplyTextures(textures);

            RenderSettings.skybox = skyboxMaterial;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShuffleTextures(Texture2D[] textures)
    {
        for (int i = 0; i < textures.Length; i++)
        {
            Texture2D tex = textures[i];
            int randomIndex = Random.Range(0, textures.Length);
            textures[i] = textures[randomIndex];
            textures[randomIndex] = tex;
        }
    }

    void ApplyTextures(Texture2D[] textures)
    {
        skyboxMaterial.SetTexture("_FrontTex", textures[0]);
        skyboxMaterial.SetTexture("_BackTex", textures[1]);
        skyboxMaterial.SetTexture("_LeftTex", textures[2]);
        skyboxMaterial.SetTexture("_RightTex", textures[3]);
        skyboxMaterial.SetTexture("_UpTex", textures[4]);
        skyboxMaterial.SetTexture("_DownTex", textures[5]);
    }
}
