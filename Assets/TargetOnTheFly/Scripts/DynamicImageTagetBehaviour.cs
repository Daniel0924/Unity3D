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

            JsonData jd = JsonMapper.ToObject(FilesManager.photoReturnResult);
            JsonData jdItems = jd["list"];
            Debug.Log("4441:" + jdItems);

            for (int i = 0; i < jdItems.Count; i++)
            {
                Debug.Log("444:" + jdItems[i]["score"]);
                GameObject subGameObject = Instantiate(Resources.Load("kuang", typeof(GameObject))) as GameObject;
                subGameObject.transform.parent = transform;

                Cirs c = subGameObject.GetComponent<Cirs>();
                c.transform.parent = transform;
                c.cenx = float.Parse((string) jdItems[i]["cenx"]);
                c.ceny = float.Parse((string) jdItems[i]["ceny"]);
                c.width = float.Parse((string) jdItems[i]["width"]);
                c.height = float.Parse((string) jdItems[i]["height"]);


                string imgClass = (string) jdItems[i]["class"];
                if (imgClass == "Crack")
                    c.mat = (Material) Resources.Load("red");
                if (imgClass == "perforation")
                    c.mat = (Material) Resources.Load("yellow");
                if (imgClass == "Injury around rivets")
                    c.mat = (Material) Resources.Load("green");
                if (imgClass == "Scratch")
                    c.mat = (Material) Resources.Load("black");
                if (imgClass == "Flake off")
                    c.mat = (Material) Resources.Load("blue");

                GameObject txtObject = Instantiate(Resources.Load("kuangtxt", typeof(GameObject))) as GameObject;
                txtObject.transform.parent = transform;
                txtObject.GetComponent<TextMesh>().text = (string) jdItems[i]["class"] + "/" +(string) jdItems[i]["score"];
                Vector3 v = new Vector3(c.cenx, 0, c.ceny - c.height / 2 - 0.01f);
                txtObject.transform.position = v;

                if (imgClass == "Crack")
                    txtObject.GetComponent<TextMesh>().color = Color.red;
                if (imgClass == "perforation")
                    txtObject.GetComponent<TextMesh>().color = Color.yellow;
                ;
                if (imgClass == "Injury around rivets")
                    txtObject.GetComponent<TextMesh>().color = Color.green;
                ;
                if (imgClass == "Scratch")
                    txtObject.GetComponent<TextMesh>().color = Color.black;
                
                if (imgClass == "Flake off")
                    txtObject.GetComponent<TextMesh>().color = Color.blue;

            }


        }



    }
}
