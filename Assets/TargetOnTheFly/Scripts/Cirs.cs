using UnityEngine;
using System.Collections;



public class Cirs : MonoBehaviour
{

    public Material mat;

    //public float bigB = 0.21f;
    //public float smallB = 0.19f;
    public float cenx = 0.0f;
    public float ceny = 0.0f;
    public float width = 0.3f;
    public float height = 0.2f;
    private float thick = 0.002f;
    public string info = "hello world";
    

    // Use this for initialization  
    void Start()
    {
        DrawFrame();
    }

    #region 画方框  
    void DrawFrame()
    {
        //gameObject.AddComponent<MeshFilter>();
        //gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = mat;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();


        mesh.vertices = new Vector3[] { new Vector3(-(width/2+thick)+cenx, 0, height/2+thick+ceny), new Vector3(width/2+thick+cenx, 0, height/2+thick+ceny),
                                        new Vector3(width/2+thick+cenx, 0, -(height/2+thick)+ceny), new Vector3(-(width/2+thick)+cenx, 0, -(height/2+thick)+ceny),
                                        new Vector3(-width/2+cenx, 0, height/2+ceny), new Vector3(width/2+cenx, 0, height/2+ceny),
                                        new Vector3(width/2+cenx, 0, -height/2+ceny), new Vector3(-width/2+cenx, 0, -height/2+ceny),


        /*
              mesh.vertices = new Vector3[] { new Vector3(-bigB+cenx, 0, bigB+ceny), new Vector3(bigB+cenx, 0, bigB+ceny),
                                              new Vector3(bigB+cenx, 0, -bigB+ceny), new Vector3(-bigB+cenx, 0, -bigB+ceny),
                                              new Vector3(-smallB+cenx, 0, smallB+ceny), new Vector3(smallB+cenx, 0, smallB+ceny),
                                              new Vector3(smallB+cenx, 0, -smallB+ceny), new Vector3(-smallB+cenx, 0, -smallB+ceny)
                                              */
    };
        mesh.triangles = new int[]
        { 0, 1, 5,
          0, 5, 4,
          1,2,6,
          1,6,5,
          2,7,6,
          2,3,7,
          3,4,7,
          0,4,3

        };


    }
    #endregion



}
