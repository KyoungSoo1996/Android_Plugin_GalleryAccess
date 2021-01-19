using UnityEngine;
using UnityEngine.UI;

public class useAndroidGallery : MonoBehaviour
{
    public Image image;
    public AndroidGallery AG;


    public void ButtonDown(){
        StartCoroutine(AG.ShowGallery(
            (_tex, _spr) =>{
                image.sprite = _spr;
            }
        ));
    }
}
