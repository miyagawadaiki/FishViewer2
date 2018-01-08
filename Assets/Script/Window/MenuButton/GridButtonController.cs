using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridButtonController : MonoBehaviour {

	[SerializeField]
	private Sprite axisImage = null;
	[SerializeField]
	private Sprite gridImage = null;
	[SerializeField]
	private Sprite hideImage = null;

	public GraphContent gc;

	private Image image;
	private bool callFlag = false;

	void Awake() {
		image = this.GetComponent<Image> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (callFlag) {
			ChangeImage ();
			callFlag = false;
		}
	}

	public void ChangeImage() {
		switch (gc.viewMode) {
		case ViewMode.ShowAxis:
			image.sprite = axisImage;
			return;
		case ViewMode.ShowGrid:
			image.sprite = gridImage;
			return;
		case ViewMode.Hide:
			image.sprite = hideImage;
			return;
		default:
			break;
		}
	}

	public void CallChangeImage() {
		callFlag = true;
	}
}
