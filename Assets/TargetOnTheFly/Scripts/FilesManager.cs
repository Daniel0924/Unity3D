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
        void Awake()
        {
            ui = FindObjectOfType<TargetOnTheFly>();
            MarksDirectory = Application.persistentDataPath;
            Debug.Log("MarkPath:" + Application.persistentDataPath);
        }

        public void StartTakePhoto()
        {
            
            if (!Directory.Exists(MarksDirectory))
                Directory.CreateDirectory(MarksDirectory);
            if (!isWriting)
                StartCoroutine(ImageCreate());
            
            
        }
        
        public IEnumerator getTexture()
        {
            
            print("调用了getTexture方法");
            
            yield return new WaitForEndOfFrame();
            Texture2D t = new Texture2D(400, 300);
            t.ReadPixels(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 50, 360, 300), 0, 0, false);
            //距X左的距离        距Y屏上的距离
            // t.ReadPixels(new Rect(220, 180, 200, 180), 0, 0, false);
            t.Apply();
            byte[] byt = t.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/Photoes/" + Time.time + ".jpg", byt);

            encodePhoto = Convert.ToBase64String(byt);
            tex.Play();
            
            print("================begin to post photoes===================");
            
            Dictionary<string, string> post = new Dictionary<string, string>();
           
            post.Add("encodePhoto",encodePhoto);
            string data = GetPost(post);
            string url = "http://10.112.32.109:8088/api/drawBorder";
            string result = postWebRequest(url, data);
            
            JsonData jsonData = JsonMapper.ToObject(result);
            
            DynamicImageTagetBehaviour ditb = new DynamicImageTagetBehaviour();
            
            ditb.drawTest(jsonData);
            
        }

        IEnumerator ImageCreate()
        {
            Debug.Log("调用了imageCreate 方法");
            isWriting = true;
            yield return new WaitForEndOfFrame();

            //Texture2D photo = new Texture2D(Screen.width / 2, Screen.height / 2, TextureFormat.RGB24, false);
            //photo.ReadPixels(new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2), 0, 0, false);

            Texture2D photo = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            photo.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            photo.Apply();

            byte[] data = photo.EncodeToJPG(80);
            
            
            encodePhoto = Convert.ToBase64String(data);
            
            
            Debug.Log("================begin to post photoes===================");
            
            Dictionary<string, string> post = new Dictionary<string, string>();
            
            post.Add("encodePhoto",encodePhoto);
            string postData = GetPost(post);
            string url = "http://10.112.32.109:8088/api/drawBorder";
            string result = postWebRequest(url, postData);
        
            JsonData jsonData = JsonMapper.ToObject(result);
            
            Debug.Log(jsonData);
            
            
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
