using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotController : BaseMonoBehaviour
{
    private int count = 0;

    private void Start()
    {
        count = PlayerPrefs.GetInt("ScreenshotCount");
    }

    public override void UpdateNormal()
    {
        if (Input.GetKeyDown(KeyCode.F12))
            StartCoroutine(TakeScreenshot());
    }

    IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height),0, 0);
        texture.Apply();

        yield return 0;

        byte[] bytes = texture.EncodeToPNG();

        File.WriteAllBytes(Application.dataPath + "/../Screenshot-" + count + ".png", bytes);
        count++;
        PlayerPrefs.SetInt("ScreenshotCount", count++);

        DestroyObject(texture);
        Debug.Log("SCREENSHOT TAKEN");
    }

}
