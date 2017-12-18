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

	//スフィアに貼り付ける映像のあれ
	public VideoPlayer SphereMovie;
	public VideoPlayer MirrorMovie;

    //その他の変数
    public Quaternion direction;
    private bool isUpHead = false;
	private bool isAlf = false;
	private bool isChange = false;

    public float SphereAlf = 1f;
    private Material SphereMat;
    private Material MirrorMat;

    // Use this for initialization
    void Start () {
		webCamInit ();
		movieInit ();
		VRSettings.showDeviceView = false; //Oculusの映像をうつさないようにする設定(ライブの映像には関係ない)
    }
	

	// Update is called once per frame
	void Update () {
        keyCont(); //key操作の関数
		ControlHMD(); //HMDが上を向いた時とかの関数
    }

	void movieInit(){
		SphereMovie.Play ();
		MirrorMovie.Play ();
		SphereMovie.Pause ();
		MirrorMovie.Pause ();
	}

	void webCamInit(){
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
	}

	void ControlHMD(){
		//上を向いたタイミングでLiveSphereを消す
		direction = InputTracking.GetLocalRotation(VRNode.Head);
		if (0.4f <= direction [0] && direction [0] < 0.50f) {
			isUpHead = true;
			if(isAlf == false && isChange){
				DOTween.To(() => SphereAlf, x => SphereAlf = x, 0, 2f);
				LiveMirror.SetActive (false);
				SphereMovie.Play ();
				MirrorMovie.Play ();
				isAlf = true;
			}else if(isAlf == true && isChange){
				DOTween.To(() => SphereAlf, x => SphereAlf = x, 1, 2f);
				LiveMirror.SetActive (true);
				SphereMovie.Pause ();
				MirrorMovie.Pause ();
				isAlf = false;
			}
			isChange = false;
		} else {
			if (isUpHead) {
				StartCoroutine ("StopAlfChange");
			}
			isUpHead = false;
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
        else if (Input.GetKeyDown(KeyCode.D))
        {
            LiveMirror.transform.Rotate(new Vector3(-1f, 0f, 0f));
		}
			
    }

	//アルファチェンジの微調整
	IEnumerator StopAlfChange(){
		yield return new WaitForSeconds (2f);
		isChange = true;
	}
}
