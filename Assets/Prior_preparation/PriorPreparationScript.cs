using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.VR;
using DG.Tweening;

public class PriorPreparationScript : MonoBehaviour {

    //ライブミラー、ライブスフィア、ミラー、スフィア
    public GameObject LiveMirror;
    public GameObject LiveSphere;
    public GameObject Mirror;
    public GameObject Sphere;

    //ウェブカム数値設定
    public int SphereCamNum = 3;
    public int MirrorCamNum = 0;
    private int width = 1920;
    private int height = 1080;
    private int fps = 30;

    //ウェブカムの云々
    private WebCamTexture SphereWebCam;
    private WebCamTexture MirrorWebCam;

    //その他の変数
    public Quaternion direction;
    private bool isHead = false;
    private bool canAlf = true;
    public float SphereAlf = 1f;
    private Material SphereMat;
    private Material MirrorMat;

    // Use this for initialization
    void Start () {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > SphereCamNum)
        {
            SphereWebCam = new WebCamTexture(devices[SphereCamNum].name, width, height, fps);
            SphereMat = LiveSphere.GetComponent<Renderer>().material;
            LiveSphere.GetComponent<Renderer>().material.mainTexture = SphereWebCam;
            SphereWebCam.Play();

            MirrorWebCam = new WebCamTexture(devices[MirrorCamNum].name, width, height, fps);
            MirrorMat = LiveMirror.GetComponent<Renderer>().material;
            LiveMirror.GetComponent<Renderer>().material.mainTexture = MirrorWebCam;
            MirrorWebCam.Play();
        }
        else
        {
            Debug.Log("no camera");
        }

        for (int i = 0; i < devices.Length; i++)
        {
            Debug.Log(devices[i].name);
        }

        //Oculusの映像をうつさないようにする設定(ライブの映像には関係ない)
        VRSettings.showDeviceView = false;
    }
	


	// Update is called once per frame
	void Update () {
        keyCont();

        direction = InputTracking.GetLocalRotation(VRNode.Head);

        //上を向いたタイミングでLiveSphereを消す
        if (0.4f <= direction[0] && direction[0] < 0.50f)
        {
            if (canAlf)
            {
                isHead = true;
            }
            canAlf = false;
        }
        else {
            canAlf = true;
        }

        if (isHead)
        {
            DOTween.To(() => SphereAlf, x => SphereAlf = x, 0, 2f);
            isHead = false;
         }

        SphereMat.SetFloat("_SphereAlf", SphereAlf);
    }



    void keyCont() {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 mirrorPos = LiveMirror.transform.position;
            mirrorPos.x = mirrorPos.x + 0.02f;
            LiveMirror.transform.position = mirrorPos;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 mirrorPos = LiveMirror.transform.position;
            mirrorPos.x = mirrorPos.x - 0.02f;
            LiveMirror.transform.position = mirrorPos;

        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 mirrorPos = LiveMirror.transform.position;
            mirrorPos.y = mirrorPos.y + 0.02f;
            LiveMirror.transform.position = mirrorPos;

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 mirrorPos = LiveMirror.transform.position;
            mirrorPos.y = mirrorPos.y - 0.02f;
            LiveMirror.transform.position = mirrorPos;

        }
        else if (Input.GetKey(KeyCode.R))
        {
            LiveMirror.transform.Rotate(new Vector3(0f, 0f, 1f));

        }
        else if (Input.GetKey(KeyCode.L))
        {
            LiveMirror.transform.Rotate(new Vector3(0f, 0f, -1f));
        }
        else if (Input.GetKey(KeyCode.U))
        {
            LiveMirror.transform.Rotate(new Vector3(1f, 0f, 0f));

        }
        else if (Input.GetKey(KeyCode.D))
        {
            LiveMirror.transform.Rotate(new Vector3(-1f, 0f, 0f));
        }
    }
}
