using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.VR;

public class VideoPlayScript : MonoBehaviour {

	//　VideoPlayerコンポーネント
	private VideoPlayer mPlayer;

	// Use this for initialization
	void Start () {
		mPlayer = GetComponent <VideoPlayer> ();
		mPlayer.Stop ();
	}
		

	void Update () {
		Quaternion direction = InputTracking.GetLocalRotation (VRNode.Head);

		if(0.4f <= direction[0] && direction[0] < 0.50f){
			mPlayer.Play ();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			mPlayer.Play ();
		}
	}
}