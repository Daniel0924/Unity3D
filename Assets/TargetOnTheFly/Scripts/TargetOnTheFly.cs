//=============================================================================================================================
//
// Copyright (c) 2015-2018 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
// EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
// and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//=============================================================================================================================

using UnityEngine;
using System.Collections;
using System.IO;
using EasyAR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using LitJson;


namespace Sample
{
    public class TargetOnTheFly : MonoBehaviour
    {
        private const string title = "Please enter KEY first!";
        private const string boxtitle = "===PLEASE ENTER YOUR KEY HERE===";
        private const string keyMessage = ""
            + "Steps to create the key for this sample:\n"
            + "  1. login www.easyar.com\n"
            + "  2. create app with\n"
            + "      Name: TargetOnTheFly (Unity)\n"
            + "      Bundle ID: cn.easyar.samples.unity.targetonthefly\n"
            + "  3. find the created item in the list and show key\n"
            + "  4. replace all text in TextArea with your key";

        [HideInInspector]
        public bool StartShowMessage = false;
        private bool isShowing = false;
        private bool isClear = false;
        private ImageTargetManager imageManager;
        private FilesManager imageCreater;

        public GUISkin skin;


        private string message = "Photo Saved";
        private void Awake()
        {
            if (FindObjectOfType<EasyARBehaviour>().Key.Contains(boxtitle))
            {
#if UNITY_EDITOR
                UnityEditor.EditorUtility.DisplayDialog(title, keyMessage, "OK");
#endif
                Debug.LogError(title + " " + keyMessage);
            }
            imageManager = FindObjectOfType<ImageTargetManager>();
            imageCreater = FindObjectOfType<FilesManager>();
        }
        
        

     
        void OnGUI()
        {
            if (StartShowMessage)
            {
                if (!isShowing)
                    StartCoroutine(showMessage());
                StartShowMessage = false;
            }
        if(GUI.Button(new Rect(Screen.width / 2 - 80, Screen.height - 85, 160, 80), "Clear Targets", skin.GetStyle("Button")))
            {
                message = "Photo Clearedddd";
                imageCreater.ClearTexture();
                imageManager.ClearAllTarget();
                isClear = true;
            }


                
               
            if (Input.anyKeyDown && !isClear)
            {
                //Debug.Log("Input.touchCount1:" + Input.touchCount);
                message = "Photo Saved";
                imageCreater.StartTakePhoto();
                
            }
            isClear = false;

            if (isShowing)
                GUI.Box(new Rect(Screen.width / 2 - 65, Screen.height / 2, 135, 60), message, skin.GetStyle("Box"));

        }

        private WebCamTexture tex;
        public string deviceName;

        /// <summary>
        /// 获取截图
        /// </summary>
        /// <returns>The texture.</returns>
        ///
        ///
        private string encodePhoto; 
        

        IEnumerator showMessage()
        {
            isShowing = true;
            yield return new WaitForSeconds(2f);
            isShowing = false;
        }
        
        
     
        
        
        
    }
}