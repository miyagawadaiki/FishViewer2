using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBarController : MonoBehaviour {

	public bool isActive = false;
	public float duration = 0.1f;

	[SerializeField]
	private Text fileNameText = null;
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

	private RectTransform recTra;

	// Use this for initialization
	void Start () {
		recTra = this.GetComponent<RectTransform> ();

		image = this.GetComponent<Image> ();
		image.color = normalColor;

		UpdateButtonImage ();

		DisInteractivate ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateButtonImage () {
		foreach (Button button in this.GetComponentsInChildren<Button>()) {
			if (button.gameObject.GetComponent<ParentButtonController> () != null) {
				button.gameObject.GetComponent<ParentButtonController> ().templete.gameObject.SetActive (true);
			}
		}

		foreach (Text txt in this.GetComponentsInChildren<Text> ()) {
			txt.color = normalTextColor;
		}

		foreach (Button button in this.GetComponentsInChildren<Button>()) {
			ColorBlock cb = button.colors;
			cb.normalColor = normalColor;
			cb.highlightedColor = highlightedColor;
			cb.pressedColor = pressedColor;
			cb.disabledColor = disabledColor;
			button.colors = cb;

			if (button.gameObject.GetComponent<ParentButtonController> () != null) {
				button.gameObject.GetComponent<ParentButtonController> ().templete.gameObject.SetActive (false);
			} else {
				button.onClick.AddListener (() => HidePanel ());
			}
		}

		fileNameText.color = normalTextColor;
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
}
