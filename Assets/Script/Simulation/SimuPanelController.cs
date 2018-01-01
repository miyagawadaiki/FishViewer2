using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimuPanelController : MonoBehaviour {

	public float duration = 0.1f;

	private Vector2 inPosition;
	private Vector2 outPosition;
	private AnimationCurve animCurve = AnimationCurve.Linear (0, 0, 1, 1);
	private Rect rect;
	//private float canvasScale;

	public bool isActive = false;

	// Use this for initialization
	void Start () {
		rect = this.GetComponent<RectTransform> ().rect;
		//canvasScale = GameObject.Find ("Canvas").transform.localScale.x;
		outPosition = new Vector2 (0f, Screen.height / 2 + rect.height / 2) * -1f; //this.transform.localPosition;
		inPosition = outPosition + new Vector2 (0f, rect.height);

		Slide ();
	}

	// Update is called once per frame
	void Update () {
		/*
		if (flag && Input.GetKeyDown(KeyCode.Space)) {
			SlideIn ();
			flag = false;
			return;
		} else if (!flag && Input.GetKeyDown(KeyCode.Space)) {
			SlideOut ();
			flag = true;
		}
		*/
	}

	public void Slide() {
		if (!isActive) {
			SlideIn ();
			isActive = true;
		} else {
			SlideOut ();
			isActive = false;
		}
	}

	public void SlideIn() {
		StartCoroutine (StartSlidePanel (true));
	}

	public void SlideOut() {
		StartCoroutine (StartSlidePanel(false));
	}

	private IEnumerator StartSlidePanel(bool isSlideIn) {
		float startTime = Time.time;
		Vector2 startPos = (Vector2)this.transform.localPosition;
		Vector2 moveDistance;

		if (isSlideIn)
			moveDistance = (inPosition - startPos);
		else
			moveDistance = (outPosition - startPos);

		while ((Time.time - startTime) < duration) {
			transform.localPosition = (Vector3)(startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration));
			yield return 0;
		}
		transform.localPosition = (Vector3)(startPos + moveDistance);
	}
}
