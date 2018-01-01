using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation {

	public static bool isEnabled = false;
	public static int step = 0;

	private static List<GraphManager> graphList;

	public static void Init() {
		step = 0;

		graphList = new List<GraphManager> ();
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Graph")) {
			graphList.Add(obj.GetComponent<GraphManager>());
		}

		DataBase.SetDataBase ();
		if (!isEnabled)
			GameObject.Find ("MyAppManager").GetComponent<MyAppManager>().ActivateSimuPanel();
		
		isEnabled = true;
	}

	public static void Reset() {
		step = 0;
	}

	public static bool IsEnd() {
		return step >= DataBase.step;
	}

	public static void Execute() {
		foreach (GraphManager gm in graphList) {
			gm.Plot (step);
		}
	}

	public static void Execute(int s) {
		foreach (GraphManager gm in graphList) {
			gm.Plot (s);
		}
	}
}
