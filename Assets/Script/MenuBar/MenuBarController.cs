using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBarController : MonoBehaviour {

	public bool isActive = false;
	public float duration = 0.1f;

	private RectTransform panelRecTra = null;

	private Vector2 inPosition;
	private Vector2 outPosition;
	private AnimationCurve animCurve = AnimationCurve.Linear (0, 0, 1, 1);
	private RectTransform recTra;
	private float canvasScale;

	// Use this for initialization
	void Start () {
		recTra = this.GetComponent<RectTransform> ();
		canvasScale = GameObject.Find ("Canvas").transform.localScale.x;
		outPosition = new Vector2 (0f, Screen.height / 2 + recTra.rect.height / 2); //this.transform.localPosition;
		inPosition = outPosition - new Vector2 (0f, recTra.rect.height);

		foreach (Button button in this.GetComponentsInChildren<Button>()) {
			button.onClick.AddListener (() => SlideOut ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (flag && Input.mousePosition.y > Screen.height - 5f) {
			SlideIn ();
			flag = false;
			return;
		} else if (!flag && Screen.height - Input.mousePosition.y > rect.height * canvasScale) {
			SlideOut ();
			flag = true;
		}
		*/
	}

	public void SlideIn() {
		StartCoroutine (StartSlidePanel (true));
		isActive = true;
	}

	public void SlideOut() {
		StartCoroutine (StartSlidePanel(false));
		isActive = false;
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
		//if (!isActive)
		//	isActive = true;
	}

	public bool IsMouseInArea() {
		Vector2 vec = (Vector2)Input.mousePosition - inPosition - new Vector2(Screen.width, Screen.height) / 2f;
		if (!recTra.rect.Contains (vec))
			return false;

		if (panelRecTra != null) {
			vec = Input.mousePosition - panelRecTra.position;
			return panelRecTra.rect.Contains (vec);
		} else {
			return true;
		}
	}

	public void OnMouseLeftDown() {
		SlideOut ();
	}
}
