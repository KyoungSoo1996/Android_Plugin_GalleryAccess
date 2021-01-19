using System.Collections;
using UnityEngine;
using UnityEngine.Events;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif


public class AndroidGallery : MonoBehaviour
{
    private AndroidJavaObject Kotlin;
    private (Texture2D _tex, Sprite _spr) CallBack = (null, null);


    GameObject dialog = null;
    private void Awake() {
        #if PLATFORM_ANDROID
        if(!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead)){
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            dialog = new GameObject();
        }
        Kotlin = new AndroidJavaObject(className: "com.example.android_plugin_galleryaccess_core.galleryAccess");
        #endif
    }


    public IEnumerator ShowGallery(UnityAction<Texture2D, Sprite> val)
    {
        Kotlin.Call("Open");
        yield return new WaitUntil(() => CallBack._tex != null && CallBack._spr != null);
        val(CallBack._tex, CallBack._spr);
        CallBack = (null, null);
    }

    private void getImage(string _path)
    {
        var www = new WWW($"file:/{_path}");
        var _tex = www.texture;
        var _spr = Sprite.Create(_tex, new Rect(0f, 0f, _tex.width, _tex.height), new Vector2(0.5f, 0.5f), 100);
        CallBack = (_tex, _spr);
    }

}