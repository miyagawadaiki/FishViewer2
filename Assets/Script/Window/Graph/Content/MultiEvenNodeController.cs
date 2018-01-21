using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiEvenNodeController : MonoBehaviour {

	[SerializeField]
	private Image image = null;
	[SerializeField]
	private Text text = null;

	private Transform tra;

	// Use this for initialization
	void Start () {
		this.GetComponent<Button> ().onClick.AddListener (() => tra.SetAsLastSibling ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Set(int colorNum, int fish) {
		image.color = ProjectData.ColorList.colors[colorNum];
		text.text = (fish + 1) + "";
	}

	public void RegisterTransform (Transform t) {
		tra = t;
	}
}
