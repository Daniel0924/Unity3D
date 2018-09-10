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
using System.Collections.Generic;
using System;
using System.Net;
using System.Text;
using EasyAR;
using LitJson;


namespace Sample
{
    
    
    
    public class FilesManager : MonoBehaviour
    {
        private string MarksDirectory;
        private bool isWriting;
        private TargetOnTheFly ui;
        private WebCamTexture tex;
        public string deviceName;
        private string encodePhoto;
        private string userName;

        public static string photoReturnResult;
        
        void Awake()
        {
            ui = FindObjectOfType<TargetOnTheFly>();
            MarksDirectory = Application.persistentDataPath;
            Debug.Log("MarkPath:" + Application.persistentDataPath);
            UnityMessageManager.Instance.OnRNMessage += onMessage;
            //+=注册事件，-=解绑事件
        }

        void onDestroy()//这个方法在最后离开场景时调用，在进入时重新绑定
        {
            UnityMessageManager.Instance.OnRNMessage -= onMessage;
        }

        void onMessage(MessageHandler message)
        {
            var data = message.getData<string>();
            Debug.Log("onMessage:" + data);
            userName = data;
            message.send(new { CallbackTest = "I am Unity callback" });
        }
        
        public void StartTakePhoto()
        {
            
            if (!Directory.Exists(MarksDirectory))
                Directory.CreateDirectory(MarksDirectory);
            if (!isWriting)
                StartCoroutine(ImageCreate());
            
            
        }
        
        IEnumerator ImageCreate()
        {
            Debug.Log("调用了imageCreate 方法");
            isWriting = true;
            yield return new WaitForEndOfFrame();

           
            Texture2D photo = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            photo.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            photo.Apply();

            byte[] data = photo.EncodeToJPG(80);
            
            
            encodePhoto = Convert.ToBase64String(data);
            
            
            Debug.Log("================begin to post photoes===================");
            
            Dictionary<string, string> post = new Dictionary<string, string>();
            
            post.Add("encodePhoto",encodePhoto);
            //post.Add("userName",userName);
            
            string postData = GetPost(post);
           //string url = "http://10.112.24.79:8080/testplatform/api/drawBorder";//服务器url
           string url = "http://boeing.vicp.net:80/testplatform/api/drawBorder";//服务器映射url
          // string url = "http://10.112.32.109:8088/api/drawBorder";
           // string url = "http://10.112.248.148:8088/api/drawBorder";
            photoReturnResult = postWebRequest(url, postData);

            Debug.Log(photoReturnResult);
            
            
            DestroyImmediate(photo);
            photo = null;

            string photoPath = Path.Combine(MarksDirectory, "photo" + DateTime.Now.Ticks + UnityEngine.Random.Range(-1f, 1f) + ".jpg");

            FileStream file = File.Open(photoPath, FileMode.Create);
            file.BeginWrite(data, 0, data.Length, new AsyncCallback(endWriter), file);
          
        }

        void endWriter(IAsyncResult end)
        {
            using (FileStream file = (FileStream)end.AsyncState)
            {
                file.EndWrite(end);
                isWriting = false;
                ui.StartShowMessage = true;
            }
        }

        public Dictionary<string, string> GetDirectoryName_FileDic()
        {
            if (!Directory.Exists(MarksDirectory))
                return new Dictionary<string, string>();
            return GetAllImagesFiles(MarksDirectory);
        }

        private Dictionary<string, string> GetAllImagesFiles(string path)
        {
            Dictionary<string, string> imgefilesDic = new Dictionary<string, string>();
            foreach (var file in Directory.GetFiles(path))
            {
                if (Path.GetExtension(file) == ".jpg" || Path.GetExtension(file) == ".bmp" || Path.GetExtension(file) == ".png")
                    imgefilesDic.Add(Path.GetFileNameWithoutExtension(file), file);
            }
            return imgefilesDic;
        }

        public void ClearTexture()
        {
            Dictionary<string, string> imageFileDic = GetAllImagesFiles(MarksDirectory);
            foreach (var path in imageFileDic)
                File.Delete(path.Value);
        }
        
        public string GetPost(Dictionary<string, string> post)
        {
            string str = "";
            foreach (var item in post)
            {
                str += item.Key + "=" + item.Value + "&";
            }

            str = str.Remove(str.Length - 1);
            return str;
        }

        //发送post请求，并接受服务器返回的数据
        public string postWebRequest(string postUrl, string data)
        {
        
            byte[] bs = Encoding.Default.GetBytes(data);
            HttpWebRequest req = (HttpWebRequest) HttpWebRequest.Create(new Uri(postUrl));
        
            //Dictionary<string, string> header = new Dictionary<string, string>();
            //header.Add("Content-Type", "application/x-www-form-urlencoded");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            var response = (HttpWebResponse) req.GetResponse();
            using (Stream responseStm = response.GetResponseStream())
            {
                StreamReader redStm = new StreamReader(responseStm, Encoding.UTF8);
                string result = redStm.ReadToEnd();
                Debug.Log(result);
                redStm.Close();
                responseStm.Close();
                return result;
            }
        }

    }
}
