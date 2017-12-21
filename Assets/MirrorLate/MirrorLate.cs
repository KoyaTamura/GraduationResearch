using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.VR;
using DG.Tweening;

public class MirrorLate : MonoBehaviour
{

    //webcam数値
    private WebCamTexture webcamTexture;
    public int camNum = 0;
    public int width = 1280;
    public int height = 720;
    public int fps = 15;

    //代入する云々
    private Texture2D setTexture;
    private Texture2DArray textureArray;
    private bool webcamIsPlaying = true;

    //何秒遅らせるか
    public int lateSec = 3;
    public float lateSpeed = 0.01f;

    //遅延を減らすスイッチ
    private bool realTime = false;
    public bool swichLate = true;
    private int GoBack;
    private float fixTime;

    //ミラーを選択して放り込む
    public GameObject Mirror;


    // Use this for initialization
    void Start()
    {

        WebCamInit();
        Application.targetFrameRate = fps-10; //FPS設定

    }


    // Update is called once per frame
    void Update()
    {
        keyCont();

        //最初の一回で代入先のtextureを初期化
        if (webcamTexture.width > 16 && webcamTexture.height > 16 && webcamIsPlaying)
        {
            SetTextureInit();
            webcamIsPlaying = false;
        }

        //全て初期化されたらここに入る
        if (webcamIsPlaying == false)
        {

            if (Input.GetKeyDown(KeyCode.Return))
            {
                realTime = true;
                GoBack = 1 - GoBack;
            }

            textureArray.SetPixels(webcamTexture.GetPixels(), Time.frameCount % (lateSec * fps));
            StartCoroutine("realTimeMovie", Time.frameCount % (fps * lateSec));

        }

        Debug.Log(fixTime);

    }

    void WebCamInit()
    {
        //遅延した状態で始めるならGoBackは0でfixTimeは0
        //リアルタイムで始める場合はGoBackは1でfixTimeはlateSec
        if (swichLate)
        {
            GoBack = 0;
            fixTime = 0;
        }
        else
        {
            GoBack = 1;
            fixTime = lateSec;
        }

        //webcamをスタートさせる
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > camNum)
        {
            webcamTexture = new WebCamTexture(devices[camNum].name, width, height, 15);
            webcamTexture.Play();
        }
        else
        {
            Debug.Log("no camera");
        }

        //デバイスの名前一覧表示
        for (int i = 0; i < devices.Length; i++)
        {
            Debug.Log(devices[i].name);
        }
    }

    void SetTextureInit()
    {

        //テクスチャー配列、カラー、表示テクスチャーの初期化
        textureArray = new Texture2DArray(webcamTexture.width, webcamTexture.height, lateSec * fps, TextureFormat.RGB565, false);
        setTexture = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGB565, false);
        GetComponent<Renderer>().material.mainTexture = setTexture;
    }

    IEnumerator realTimeMovie(int i)
    {

        if (realTime && GoBack == 1)
        {
            if (lateSec - fixTime > 0)
            {
                fixTime += lateSpeed;
            }
            else
            {
                fixTime = lateSec;
                realTime = false;
            }
        }
        else if (realTime && GoBack == 0)
        {
            if (fixTime > 0)
            {
                fixTime -= lateSpeed;
            }
            else
            {
                fixTime = 0;
                realTime = false;
            }
        }

        yield return new WaitForSeconds((float)lateSec - fixTime);
        setTexture.SetPixels(textureArray.GetPixels(i));
        setTexture.Apply();
    }
    void keyCont()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 mirrorPos = Mirror.transform.position;
            mirrorPos.x = mirrorPos.x + 0.02f;
            Mirror.transform.position = mirrorPos;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 mirrorPos = Mirror.transform.position;
            mirrorPos.x = mirrorPos.x - 0.02f;
            Mirror.transform.position = mirrorPos;

        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 mirrorPos = Mirror.transform.position;
            mirrorPos.y = mirrorPos.y + 0.02f;
            Mirror.transform.position = mirrorPos;

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 mirrorPos = Mirror.transform.position;
            mirrorPos.y = mirrorPos.y - 0.02f;
            Mirror.transform.position = mirrorPos;

        }
        else if (Input.GetKey(KeyCode.R))
        {
            Mirror.transform.Rotate(new Vector3(0f, 0f, 1f));

        }
        else if (Input.GetKey(KeyCode.L))
        {
            Mirror.transform.Rotate(new Vector3(0f, 0f, -1f));
        }
        else if (Input.GetKey(KeyCode.U))
        {
            Mirror.transform.Rotate(new Vector3(1f, 0f, 0f));

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Mirror.transform.Rotate(new Vector3(-1f, 0f, 0f));
        }

    }
}

