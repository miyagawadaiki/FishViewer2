using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour {

	public GraphContent gc;
	public GraphContentType type;
	public GameObject coverPanelObj;
	public List<RectTransform> elements = new List<RectTransform>();

	/*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	*/

	public virtual void CoverElement(int idx) {
		if (idx >= elements.Count)
			idx = elements.Count - 1;

		Instantiate (coverPanelObj, elements [idx]);
	}

	public virtual string GetData() {
		return "";
	}
}
