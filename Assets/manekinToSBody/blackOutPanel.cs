using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.VR;
using DG.Tweening;

public class blackOutPanel : MonoBehaviour {

	public CanvasGroup fadeout;
	private bool flag = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion direction = InputTracking.GetLocalRotation (VRNode.Head);

		if(0.4f <= direction[0] && direction[0] < 0.50f && flag){
			StartCoroutine ("blackOut");
			flag = false;
		}

		else if (Input.GetKeyDown(KeyCode.Space) && flag) {
			Debug.Log (fadeout.alpha);
			StartCoroutine ("blackOut");
			flag = false;
		}

		Debug.Log (fadeout.alpha);

	}

	private IEnumerator blackOut(){
		DOTween.To (() => fadeout.alpha , (x) =>fadeout.alpha = x ,1.0f,2.0f).SetEase(Ease.InCubic);
		yield return new WaitForSeconds (2.0f);
		DOTween.To (() => fadeout.alpha , (x) =>fadeout.alpha = x ,0.0f,2.0f).SetEase(Ease.InCubic);
	}

}
