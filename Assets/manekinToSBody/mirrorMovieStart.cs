using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.VR;


public class mirrorMovieStart : MonoBehaviour {

	//マネキンのビデオプレイヤー
	public VideoPlayer moviePlayer;
	//HMDトラッキングの格納
	public Quaternion dir;
	//上を一回だけ向いた判定
	private bool flag = true;
	//再生されてるmirrorを探す
	public GameObject mirror;


	// Use this for initialization
	void Start () {
		//過去映像をスタートと同時に再生
		moviePlayer.Play();
	}
	
	// Update is called once per frame
	void Update () {
		//上を向いたら
		dir = InputTracking.GetLocalRotation (VRNode.Head);
		if(0.4f <= dir[0] && dir[0] < 0.50f && flag){
			StartCoroutine ("StopTimer");
			flag = false;
		}

		//space押したら
		else if (Input.GetKeyDown (KeyCode.Space) && flag) {
			StartCoroutine ("StopTimer");
			flag = false;
		}
	}

	IEnumerator StopTimer(){
		yield return new WaitForSeconds (2.0f);
		moviePlayer.Stop ();
		mirror.SetActive (false);
	}
}
