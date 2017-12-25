using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileNode : MonoBehaviour {

	[SerializeField]
	private GameObject file = null;
	[SerializeField]
	private GameObject folder = null;
	[SerializeField]
	private Text text = null;

	[System.NonSerialized]
	public bool isFolder = false;
	[System.NonSerialized]
	public FileInputFieldManager fifMan;
	[System.NonSerialized]
	public FileViewController fvc;
	[System.NonSerialized]
	public InputField nameIf;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Set(bool b, string name) {
		isFolder = b;
		text.text = name;

		if (isFolder) {
			file.SetActive (false);
			folder.SetActive (true);
		} else {
			file.SetActive (true);
			folder.SetActive (false);
		}
	}

	public string GetName() {
		return text.text;
	}

	public void Excute() {
		if (isFolder) {
			fifMan.GoNext (GetName ());
			fvc.CallForUpdateView ();
		} else {
			nameIf.text = GetName ();
		}
	}
}
