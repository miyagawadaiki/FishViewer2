using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiVariousLabelNode : MonoBehaviour {

	[SerializeField]
	private Image pointerImage = null;
	[SerializeField]
	private Text text = null;

	private MultiVariousGraphContent mvgc;
	private Button button;
	private Image buttonImage;
	private int index;

	void Awake () {
		mvgc = this.GetComponentInParent<MultiVariousGraphContent> ();
		button = this.GetComponent<Button> ();
		button.onClick.AddListener (() => mvgc.Select (index));
		buttonImage = this.GetComponent<Image> ();

		UnSelect ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Set (int index, int color, string label) {
		this.index = index;
		pointerImage.color = ProjectData.ColorList.colors [color];
		text.text = label;
	}

	public void Select () {
		buttonImage.color = button.colors.pressedColor;
	}

	public void UnSelect () {
		buttonImage.color = Color.white;
	}
}
