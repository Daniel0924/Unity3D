//=============================================================================================================================
//
// Copyright (c) 2015-2017 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
// EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
// and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//=============================================================================================================================

using UnityEngine;
using EasyAR;
using System.Collections;
using LitJson;

public class ModelInfo
{
    public string info;
    public float cenx;
    public float ceny;
    public float width;
    public float height;

}

public class ModelList
{
    public ModelInfo[] list;
}

namespace Sample
{
    public class DynamicImageTagetBehaviour : ImageTargetBehaviour
    {
      
        protected override void Awake()
        {
            Debug.Log("use awake()ddd");
            //JsonData jd = JsonMapper.ToObject(JsonInitial);
               
            //在这里拿到返回值，并进行解析，画出框
            base.Awake();

            //string url = "http://10.112.150.255:8081/a.txt";
            //string url = "file://10.112.32.109/Users/kiko/a.txt";
            string url = "http://10.112.32.109:8088/api/getAxis";
            WWW www = new WWW(url);
            while (!www.isDone) { }

            Debug.Log("info333333:" + www.text);
            JsonData jd = JsonMapper.ToObject(www.text);
           JsonData jdItems = jd["list"];
            Debug.Log("4441:" + jdItems.Count);
            for (int i = 0; i < jdItems.Count; i++)
            {
                Debug.Log("444:" + jdItems[i]["info"]);
                GameObject subGameObject = Instantiate(Resources.Load("kuang", typeof(GameObject))) as GameObject;
                subGameObject.transform.parent = transform;

                Cirs c = subGameObject.GetComponent<Cirs>();
                c.transform.parent = transform;
                c.cenx = float.Parse((string)jdItems[i]["cenx"]);
                c.ceny = float.Parse((string)jdItems[i]["ceny"]);
                c.width = float.Parse((string)jdItems[i]["width"]);
                c.height = float.Parse((string)jdItems[i]["height"]);

                string imgClass = (string)jdItems[i]["class"];
                if(imgClass == "1")
                    c.mat = (Material)Resources.Load("red");
                if (imgClass == "2")
                    c.mat = (Material)Resources.Load("yellow");
                if (imgClass == "3")
                    c.mat = (Material)Resources.Load("green");

                GameObject txtObject = Instantiate(Resources.Load("kuangtxt", typeof(GameObject))) as GameObject;
                txtObject.transform.parent = transform;
                txtObject.GetComponent<TextMesh>().text = (string)jdItems[i]["info"];
                Vector3 v = new Vector3(c.cenx, 0, c.ceny - c.height / 2 - 0.01f);
                txtObject.transform.position = v;

                if (imgClass == "1")
                    txtObject.GetComponent<TextMesh>().color = Color.red;
                if (imgClass == "2")
                    txtObject.GetComponent<TextMesh>().color = Color.yellow; ;
                if (imgClass == "3")
                    txtObject.GetComponent<TextMesh>().color = Color.green; ;

            }


            /*
            GameObject subGameObject2 = Instantiate(Resources.Load("kuang", typeof(GameObject))) as GameObject;
            subGameObject2.transform.parent = transform;

            Cirs c2 = subGameObject2.GetComponent<Cirs>();
            c2.transform.parent = transform;
            c2.cenx = obj.cenx +0.2f;
            c2.ceny = obj.ceny;
            c2.width = obj.width;
            c2.height = obj.height;
            */
            
        }

        private void Update()
        {
            
        }
        
        
        public void drawTest(JsonData jd)
        {
            JsonData jdItems = jd["list"];

            
            Debug.Log("4441:" + jdItems.Count);
            for (int i = 0; i < jdItems.Count; i++)
            {
                Debug.Log("444:" + jdItems[i]["info"]);
                GameObject subGameObject = Instantiate(Resources.Load("kuang", typeof(GameObject))) as GameObject;
                subGameObject.transform.parent = transform;

                Cirs c = subGameObject.GetComponent<Cirs>();
                c.transform.parent = transform;
                c.cenx = float.Parse((string)jdItems[i]["cenx"]);
                c.ceny = float.Parse((string)jdItems[i]["ceny"]);
                c.width = float.Parse((string)jdItems[i]["width"]);
                c.height = float.Parse((string)jdItems[i]["height"]);

                string imgClass = (string)jdItems[i]["class"];
                if(imgClass == "1")
                    c.mat = (Material)Resources.Load("red");
                if (imgClass == "2")
                    c.mat = (Material)Resources.Load("yellow");
                if (imgClass == "3")
                    c.mat = (Material)Resources.Load("green");


                GameObject txtObject = Instantiate(Resources.Load("kuangtxt", typeof(GameObject))) as GameObject;
                txtObject.transform.parent = transform;
                txtObject.GetComponent<TextMesh>().text = (string)jdItems[i]["info"];
                Vector3 v = new Vector3(c.cenx, 0, c.ceny - c.height / 2 - 0.01f);
                txtObject.transform.position = v;
                //txtObject.GetComponent<TextMesh>().color = Color.green;

                if (imgClass == "1")
                    txtObject.GetComponent<TextMesh>().color = Color.red;
                if (imgClass == "2")
                    txtObject.GetComponent<TextMesh>().color = Color.yellow; ;
                if (imgClass == "3")
                    txtObject.GetComponent<TextMesh>().color = Color.green; ;


            }
        }

        /*
        IEnumerator GetMyText()
        {
            string url = "http://192.168.1.104/a.txt";
            WWW www = new WWW(url);
            Debug.Log("info333333:" + www.text);
            c.info = www.text;

        }
        */


    }
}

