using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowListPanelController : MonoBehaviour {

	[SerializeField]
	private MyWindowManager mwm = null;
	[SerializeField]
	private RectTransform content = null;
	[SerializeField]
	private GameObject nodeObj = null;

	public float duration = 0.1f;

	private Vector2 inPosition;
	private Vector2 outPosition;
	private AnimationCurve animCurve = AnimationCurve.Linear (0, 0, 1, 1);
	private RectTransform recTra;

	public bool isActive = false;

	void Awake () {
		recTra = this.GetComponent<RectTransform> ();
	}

	// Use this for initialization
	void Start () {
		outPosition = recTra.localPosition;
		inPosition = outPosition + new Vector2 (0f, recTra.rect.height);
	}
	
	// Update is called once per frame
	void Update () {
		
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
		foreach (Transform t in content)
			Destroy (t.gameObject);

		foreach (MyWindowController mwc in mwm.windowList) {
			mwc.TranslateIntoWindowManager ();
			GameObject obj = Instantiate (nodeObj, content) as GameObject;
			obj.GetComponent<Button> ().onClick.AddListener (() => mwc.transform.SetAsLastSibling ());
			obj.GetComponentInChildren<Text> ().text = mwc.content.contentName;
		}

		inPosition = (Vector2)recTra.localPosition - new Vector2 (recTra.rect.width, 0f);
		StartCoroutine (StartSlidePanel (true));
	}

	public void SlideOut() {
		outPosition = (Vector2)recTra.localPosition + new Vector2 (recTra.rect.width, 0f);
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

	public bool IsMouseInArea() {
		Vector2 vec = (Vector2)Input.mousePosition - (Vector2)recTra.localPosition - new Vector2(Screen.width, Screen.height) / 2f;
		return recTra.rect.Contains (vec);
	}
}
