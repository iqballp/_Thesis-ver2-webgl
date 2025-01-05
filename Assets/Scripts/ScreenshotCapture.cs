using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotCapture : MonoBehaviour
{
    private GameLoger gl;
    void Start()
    {
        gl = GameObject.Find("GameLoger").GetComponent<GameLoger>();
    }

    void OnEnable()
    {
        StartCoroutine(CaptureScreenshot());
    }

    public IEnumerator CaptureScreenshot()
    {
        yield return new WaitForSeconds(2.0f);
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;

        // Ambil screenshot sebagai Texture2D
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        // Konversi ke base64
        byte[] bytes = screenshot.EncodeToPNG();
        string base64Image = System.Convert.ToBase64String(bytes);

        // Kirim base64 ke JavaScript untuk di-upload ke Firebase
        Application.ExternalCall("UploadImageToFirebase", base64Image, gl.pemain);

        Destroy(screenshot); // Hapus screenshot dari memori setelah selesai
    }
}
