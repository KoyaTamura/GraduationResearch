using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.VR;

public class liveStart : MonoBehaviour {

	public int cameraNumber = 0;
	private WebCamTexture webcamTexture;
	public Quaternion direction;
	private bool playFlag = true;

	// Use this for initialization
	void Start () {
		WebCamDevice[] devices = WebCamTexture.devices;
		if (devices.Length > cameraNumber)
		{
			webcamTexture = new WebCamTexture(devices[cameraNumber].name, 1920, 1080, 30);
			GetComponent<Renderer>().material.mainTexture = webcamTexture;
			//webcamTexture.Play();
		}
		else
		{
			Debug.Log("no camera");
		}
	}
	
	// Update is called once per frame
	void Update () {
		direction = InputTracking.GetLocalRotation(VRNode.Head);

		//上を向いたタイミングでInSphereを消す
		if (0.4f <= direction[0] && direction[0] < 0.50f && playFlag)
		{
			webcamTexture.Play ();
			playFlag = false;
			//Debug.Log("hoge");
		}

		else if(Input.GetKeyDown(KeyCode.Space) && playFlag){
			webcamTexture.Play ();
			playFlag = false;
		}
	}
}
