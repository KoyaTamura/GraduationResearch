using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using DG.Tweening;

public class manekinMirror : MonoBehaviour
{

    public int cameraNumber = 0;
    private WebCamTexture webcamTexture;
    public Color32[] color32; //webcamのテクスチャーをそのまま保存
    private bool flag = false;
    public int AlfCount = 255;
    
    public GameObject TestSphere100;
    private Texture2D texture;
    public Quaternion direction;

    public GameObject WhiteOutSphere;
    private Material whiteOutMat;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > cameraNumber)
        {
            webcamTexture = new WebCamTexture(devices[cameraNumber].name, 1920, 1080, 30);
            GetComponent<Renderer>().material.mainTexture = webcamTexture;
            webcamTexture.Play();
        }
        else
        {
            Debug.Log("no camera");
        }
        texture = new Texture2D(webcamTexture.width, webcamTexture.height);
        TestSphere100.GetComponent<Renderer>().material.mainTexture = texture;
        Debug.Log(devices[cameraNumber].name);
        

    }

    void Update()
    {
        if (webcamTexture.isPlaying)
        {
            color32 = webcamTexture.GetPixels32();
            direction = InputTracking.GetLocalRotation(VRNode.Head);

            //上を向いたタイミングでInSphereを消す
            if (0.4f <= direction[0] && direction[0] < 0.50f && flag == false)
            {
                Debug.Log("hoge");
                //StartCoroutine(WhiteOut());
                flag = true;
            }


            if (flag)
            {
                //if (AlfCount > 0)
                //{
                //    AlfCount -= 15;
                //}
                //for (int i = 0; i < color32.Length; i++)
                //{
                //    color32[i].a = AlfCount;
                //}

                DOTween.To(() => AlfCount , x => AlfCount = x , 0 , 2f);

                for (int i = 0; i < color32.Length; i++)
                {
                    color32[i].a = (byte)AlfCount;
                }

            }

            //Debug.Log(direction[0]);
            Debug.Log(AlfCount);
            texture.SetPixels32(color32);
            texture.Apply();

        }

        if (AlfCount <= 10)
        {
            webcamTexture.Stop();
            TestSphere100.SetActive(false);
        }
    }



}