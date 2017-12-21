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
    private bool webcamIsPlaying = false;

    private float lateTime;

    //何秒遅らせるか
    public int lateSec = 3;
    public float lateSpeed = 15.0f;

    //遅延を減らすスイッチ
    public bool swichLate = true;
    private bool isLate = false;

    //ミラーを選択して放り込む
    public GameObject Mirror;


    // Use this for initialization
    void Start()
    {

        WebCamInit();
        //Application.targetFrameRate = fps - 10; //FPS設定

    }


    // Update is called once per frame
    void Update()
    {
        keyCont();

        //最初の一回で代入先のtextureを初期化
        if (webcamTexture.width > 16 && webcamTexture.height > 16 && !webcamIsPlaying)
        {
            SetTextureInit();
            webcamIsPlaying = true;
        }

        if (webcamIsPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                isLate = !isLate;
                if (isLate)
                {
                    DOTween.To(() => lateTime, x => lateTime = x, 0.0f, lateSpeed);
                }
                else
                {
                    DOTween.To(() => lateTime, x => lateTime = x, lateSec, lateSpeed);
                }
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                DOTween.KillAll();
            }
            Debug.Log(lateTime);
            StartCoroutine(MovieLateCoroutine(lateTime));
        }


    }

    void WebCamInit()
    {
        //遅延した状態で始めるならGoBackはfalseでfixTimeは0
        //リアルタイムで始める場合はGoBackはtrueでfixTimeはlateSec
        if (swichLate)
        {
            isLate = false;
        }
        else
        {
            isLate = true;
        }

        //webcamをスタートさせる
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > camNum)
        {
            webcamTexture = new WebCamTexture(devices[camNum].name, width, height, fps);
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
        setTexture = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGB565, false);
        GetComponent<Renderer>().material.mainTexture = setTexture;
    }


    IEnumerator MovieLateCoroutine(float time)
    {
        Color[] bufferPixels = new Color[webcamTexture.width * webcamTexture.height];
        bufferPixels = webcamTexture.GetPixels();

        yield return new WaitForSeconds(time);

        setTexture.SetPixels(bufferPixels);
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

