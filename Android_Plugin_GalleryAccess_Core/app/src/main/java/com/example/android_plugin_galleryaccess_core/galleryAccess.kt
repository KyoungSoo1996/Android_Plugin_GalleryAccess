package com.example.android_plugin_galleryaccess_core

import android.app.Activity
import android.content.Intent
import android.database.Cursor
import android.net.Uri
import android.os.Bundle
import android.provider.MediaStore
import android.util.Log
import com.unity3d.player.UnityPlayer
import com.unity3d.player.UnityPlayerActivity

class galleryAccess : UnityPlayerActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        Log.d("UnityLogTest", "onCreate")
    }

    fun Open(){
        val intent = Intent(Intent.ACTION_PICK, MediaStore.Images.Media.INTERNAL_CONTENT_URI)
         UnityPlayer.currentActivity.startActivityForResult(intent, 0)
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)

        if(requestCode == 0 && resultCode == Activity.RESULT_OK){
            var UPath: Uri? = data!!.data;
            var path: String? = abs_path(UPath!!)
            //유니티에 값을 전달
            UnityPlayer.UnitySendMessage("AndroidPlugin","getImage",path)
        }
    }
    //절대 경로로 변환
    fun abs_path(uri:Uri): String?{
        val m_data = arrayOf(MediaStore.Images.Media.DATA)
        val cursor: Cursor = managedQuery(uri, m_data,null,null,null)
        startManagingCursor(cursor)
        val columIndex:Int = cursor.getColumnIndexOrThrow(MediaStore.Images.Media.DATA)
        cursor.moveToFirst()
        return cursor.getString(columIndex)
    }
}