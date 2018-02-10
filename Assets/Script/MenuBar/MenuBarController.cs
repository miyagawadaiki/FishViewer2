using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBarController : MonoBehaviour {

	public bool isActive = false;
	public float duration = 0.1f;

	[SerializeField]
	private Color normalColor = Color.white;
	[SerializeField]
	private Color highlightedColor = Color.white;
	[SerializeField]
	private Color pressedColor = Color.white;
	[SerializeField]
	private Color disabledColor = Color.white;
	[SerializeField]
	private Color normalTextColor = Color.black;
	[SerializeField]
	private Color disabledTextColor = Color.black;

	private RectTransform panelRecTra = null;
	private Image image = null;

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

		image = this.GetComponent<Image> ();
		image.color = normalColor;

		foreach (Button button in this.GetComponentsInChildren<Button>()) {
			if (button.gameObject.GetComponent<ParentButtonController> () != null) {
				button.gameObject.GetComponent<ParentButtonController> ().templete.gameObject.SetActive (true);
			}
		}

		foreach (Button button in this.GetComponentsInChildren<Button>()) {
			ColorBlock cb = button.colors;
			cb.normalColor = normalColor;
			cb.highlightedColor = highlightedColor;
			cb.pressedColor = pressedColor;
			cb.disabledColor = disabledColor;
			button.colors = cb;
			button.gameObject.GetComponentInChildren<Text> ().color = normalTextColor;

			if (button.gameObject.GetComponent<ParentButtonController> () != null) {
				button.gameObject.GetComponent<ParentButtonController> ().templete.gameObject.SetActive (false);
			} else {
				button.onClick.AddListener (() => HidePanel ());
			}
		}

		DisInteractivate ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SlideIn() {
		StartCoroutine (StartSlidePanel (true));
		isActive = true;
	}

	public void SlideOut() {
		if (panelRecTra != null) {
			panelRecTra.gameObject.SetActive (false);
			panelRecTra = null;
		}
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
		Vector2 vec = (Vector2)Input.mousePosition - (Vector2)recTra.localPosition - new Vector2(Screen.width, Screen.height) / 2f;


		if (recTra.rect.Contains (vec))
			return true;

		if (panelRecTra == null)
			return false;

		Vector2 vecp = Input.mousePosition - panelRecTra.position;
		return panelRecTra.rect.Contains (vecp);
			
	}

	public void RegisterPanel (RectTransform panel) {
		if (panelRecTra != null) {
			panelRecTra.gameObject.SetActive (false);
		}
		
		panelRecTra = panel;
	}

	public void HidePanel () {
		if (panelRecTra != null) {
			panelRecTra.gameObject.SetActive (false);
			panelRecTra = null;
		}
	}

	public void Interactivate () {
		foreach (Button button in this.GetComponentsInChildren<Button>()) {
			if (button.gameObject.CompareTag ("UseDataButton")) {
				button.interactable = true;
				button.gameObject.GetComponentInChildren<Text> ().color = normalTextColor;
			}
		}
	}

	public void DisInteractivate () {
		foreach (Button button in this.GetComponentsInChildren<Button>()) {
			if (button.gameObject.CompareTag ("UseDataButton")) {
				button.interactable = false;
				button.gameObject.GetComponentInChildren<Text> ().color = disabledTextColor;
			}
		}
	}

	public void OnMouseLeftDown() {
		SlideOut ();
	}
}
