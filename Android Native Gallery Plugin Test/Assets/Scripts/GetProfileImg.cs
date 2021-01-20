using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetProfileImg : MonoBehaviour
{
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
        float width = GetComponent<RectTransform>().sizeDelta.x / 4;
        float height = GetComponent<RectTransform>().sizeDelta.y / 4;
        if (xValue < yValue)
        {
            xMove = (x + width / height);
            profileImg.uvRect = new Rect(Mathf.Clamp(-xMove, 0, 1 - xValue), 0, xValue, yValue);
        }
        else if (xValue > yValue)
        {
            yMove = (y + height) / width;
            profileImg.uvRect = new Rect(0, Mathf.Clamp(-yMove, 0, 1 - yValue), xValue, yValue);
        }
        else
        {
            return null;
        }
        float[] result = { xMove, yMove };
        return result;
    }

    public IEnumerator CoSaveImg(Texture _texture, int x, int y)
    {
        yield return new WaitForEndOfFrame();
        var width = (int)(128 * (xValue >= 1 ? 1 : xValue));
        var height = (int)(128 * (yValue >= 1 ? 1 : yValue));
        Texture2D texture = ScaleTexture((Texture2D)_texture, width, height, x, y);

        Color[] colors = texture.GetPixels(0, 0, width, height);
        texture.Apply();
        profileImg.texture = texture;
        byte[] bytes = texture.EncodeToPNG();
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
        PickImage(128);
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