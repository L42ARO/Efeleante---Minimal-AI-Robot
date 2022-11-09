using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LightSensorController : MonoBehaviour
{
    public Camera cameraSensor;
    RenderTexture LightSensorTexture;
    [System.NonSerialized] public float LightLevel;
    float maxLigthLevel =  14875630;//8140000 - New:    1.487563E+07
    string newAssetPath;
    bool destroyAssets = false;
    void Start()
    {
        EditorApplication.quitting+=OnQuit;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
        LightSensorTexture = CopyTextureAsset();
        cameraSensor.targetTexture=LightSensorTexture;
    }
    void Update()
    {
        if(!destroyAssets){
            //Switch the render texture to temporary one
            RenderTexture tmpTexture = RenderTexture.GetTemporary(LightSensorTexture.width, LightSensorTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(LightSensorTexture, tmpTexture);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = tmpTexture;
            //Get the pixels of the render texture
            Texture2D temp2DTexture = new Texture2D(LightSensorTexture.width, LightSensorTexture.height);
            temp2DTexture.ReadPixels(new Rect(0, 0, LightSensorTexture.width, LightSensorTexture.height), 0, 0);
            temp2DTexture.Apply();
            //Relase the temporary texture
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(tmpTexture);
            //Evaluate light levels:
            Color32[] colors = temp2DTexture.GetPixels32();
            LightLevel = 0;
            for (int i = 0; i < colors.Length; i++)
            {
                // LightLevel += (0.2126f * colors[i].r) + (0.7152f * colors[i].g) + (0.0722f * colors[i].b);
                LightLevel += (0.299f * colors[i].r) + (0.587f * colors[i].g) + (0.114f * colors[i].b);
                //NOTE: Check https://stackoverflow.com/questions/596216/formula-to-determine-perceived-brightness-of-rgb-color
            }
            var valToPrint = LightLevel;
            Debug.Log("True Light Level: " + valToPrint);
        }

    }
    RenderTexture CopyTextureAsset()
    {
        var assetPath = "Assets/Objects/LS_Texture_Prime.renderTexture";
        var guid = System.Guid.NewGuid().ToString();
        var newAssetName = "LS_Texture-" + guid + ".renderTexture";
        newAssetPath = $"Assets/Objects/RuntimeAssets/{newAssetName}";
        if (!AssetDatabase.CopyAsset(assetPath, newAssetPath)){
            Debug.LogWarning($"Failed to copy {assetPath}");
        }
        RenderTexture newTexture = (RenderTexture)AssetDatabase.LoadAssetAtPath(newAssetPath, typeof(RenderTexture));
        return newTexture;  
    }
    public float GetLightIntensity()
    {
        return LightLevel / maxLigthLevel;
    }
    void OnQuit() //This will be in charge of deleting the temporary files if editor crashes
    {
        if (newAssetPath != null)
        {
            destroyAssets = true;
            // AssetDatabase.DeleteAsset(newAssetPath);//NOTE: Causes crash in HDPR we don't know why
        }
    }
    void OnPlayModeChanged(PlayModeStateChange state) // Tjhis will be in charge of deleting the temporary files if the editor stops the game
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            OnQuit();
        }
    }
}
