using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetProfileImg : MonoBehaviour
{
    [SerializeField]
    private RawImage profileImg;

    private void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    return;
                }
                
                profileImg.texture = texture;
            }
        });
    }

    public void OnClickProfileSwtich()
    {
        PickImage(1028);
    }

}
