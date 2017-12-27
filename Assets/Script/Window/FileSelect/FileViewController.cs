using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileViewController : MonoBehaviour {

	[SerializeField]
	private GameObject content = null;
	[SerializeField]
	private GameObject nodeObj = null;
	[SerializeField]
	private FileInputFieldManager fifMan = null;
	[SerializeField]
	private InputField nameIf = null;

	private string fileName;
	private bool flag = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (flag) {
			UpdateView ();
			flag = false;
		}
	}

	void UpdateView() {
		foreach (Transform transform in content.transform)
			Destroy (transform.gameObject);

		char[] separator = { '/', '\\' };

		string[] folders = Directory.GetDirectories (fifMan.GetPath ());
		foreach (string name in folders) {
			string[] tmp = name.Split (separator);
			string s = tmp [tmp.Length - 1];
			GameObject obj = Instantiate (nodeObj, content.transform);
			FileNode fn = obj.GetComponent<FileNode> ();
			fn.Set (true, s);
			fn.fifMan = this.fifMan;
			fn.fvc = this;
			fn.nameIf = nameIf;
		}

		string[] files;
		if(fifMan.key == ProjectData.FileKey.Read)
			files = Directory.GetFiles (fifMan.GetPath (), "*-fv.csv");
		else
			files = Directory.GetFiles (fifMan.GetPath ());
		foreach (string name in files) {
			string[] tmp = name.Split (separator);
			string s = tmp [tmp.Length - 1];
			GameObject obj = Instantiate (nodeObj, content.transform);
			FileNode fn = obj.GetComponent<FileNode> ();
			fn.Set (false, s);
			fn.fifMan = this.fifMan;
			fn.fvc = this;
			fn.nameIf = nameIf;
		}
	}

	public void CallForUpdateView() {
		flag = true;
	}
}
