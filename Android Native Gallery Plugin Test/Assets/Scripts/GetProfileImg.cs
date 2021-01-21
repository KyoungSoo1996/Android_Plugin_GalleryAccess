using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetProfileImg : MonoBehaviour
{
    public int _RESOLUTION = 256;
    public RawImage profileImg;
    [SerializeField] private Image bakcgroundImg;

    private void Awake()
    {
        SwitchImage(false);
    }
    private float xValue, yValue;

    public float[] MoveImage(float x, float y)
    {
        float xMove = 0.0f, yMove = 0.0f;
        float[] result;
        xMove = -x / 30;
        yMove = -y / 30;
        if (xValue < yValue)
        {
            xMove = Mathf.Clamp(xMove, 0, 1 - xValue);
            profileImg.uvRect = new Rect(xMove, 0, xValue, yValue);
            yMove = 0;
            result = new float[] { xMove, 0 };
            return result;
        }
        else if (xValue > yValue)
        {
            yMove = Mathf.Clamp(yMove, 0, 1 - yValue);
            xMove = 0;
            profileImg.uvRect = new Rect(0, yMove, xValue, yValue);
            result = new float[] { 0, yMove };
            return result;
        }
        else
        {
            return null;
        }
    }

    public IEnumerator CoSaveImg(Texture _texture, float x, float y)
    {
        
        Debug.Log(x);
        Debug.Log(y);
        yield return new WaitForEndOfFrame();
        var width = (int)(_RESOLUTION * (xValue >= 1 ? 1 : 1 / xValue));
        var height = (int)(_RESOLUTION * (yValue >= 1 ? 1 : 1 / yValue));
        Texture2D texture = ScaleTexture((Texture2D)_texture, width, height, 0, 0);

        Color[] colors = texture.GetPixels((int)(x * width), (int)(y * width), _RESOLUTION, _RESOLUTION);
        texture.Apply();

        Texture2D finTexture = new Texture2D(_RESOLUTION, _RESOLUTION, texture.format, false, false);
        finTexture.SetPixels(0, 0, _RESOLUTION, _RESOLUTION, colors);
        finTexture.Apply();
        profileImg.texture = finTexture;
        profileImg.uvRect = new Rect(0, 0, 1, 1);
        xValue = 1;
        yValue = 1;
        byte[] bytes = finTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + "test.png", bytes);
        Debug.Log(Application.dataPath + "/" + "test.png");
    }

    private void SwitchImage(bool sw)
    {
        int value = sw ? 1 : 0;
        bakcgroundImg.GetComponent<CanvasGroup>().alpha = 1 - value;
        bakcgroundImg.GetComponent<CanvasGroup>().interactable = !sw;
        profileImg.GetComponent<CanvasGroup>().alpha = value;
    }

    private void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    return;
                }
                profileImg.texture = texture;
                xValue = texture.texelSize.x;
                yValue = texture.texelSize.y;
                if (xValue > yValue) { yValue = yValue / xValue; xValue = 1; }
                else if (xValue < yValue) { xValue = xValue / yValue; yValue = 1; }
                else { xValue = 1; yValue = 1; }
                profileImg.uvRect = new Rect(0, 0, xValue, yValue);
                SwitchImage(true);
            }
        });
    }

    public void OnClickProfileSwtich()
    {
        PickImage(_RESOLUTION);
    }


    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight, int posX, int posY)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(posX, posY, targetWidth, targetHeight, rpixels, 0);
        result.Apply();
        return result;
    }

}