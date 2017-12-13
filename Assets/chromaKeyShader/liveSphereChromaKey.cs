using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class liveSphereChromaKey : MonoBehaviour {

	public int camNum = 0;
	public int width = 1920;
	public int height = 1080;
	public int fps = 30;

	private WebCamTexture webcamTexture;

	// Use this for initialization
	void Start () {
		WebCamDevice[] devices = WebCamTexture.devices;
		if (devices.Length > camNum)
		{
			webcamTexture = new WebCamTexture(devices[camNum].name, width, height, fps);
			GetComponent<Renderer>().material.mainTexture = webcamTexture;
			webcamTexture.Play();
		}
		else
		{
			Debug.Log("no camera");
		}

		for(int i=0;i<devices.Length;i++){
			Debug.Log (devices[i].name);
		}

		//Oculusの映像をうつさないようにする設定(ライブの映像には関係ない)
		UnityEngine.VR.VRSettings.showDeviceView = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
