using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation {

	public static bool isEnabled = false;
	public static int step = 0;

	private static List<GraphManager> graphList;
	private static int memo = 0;

	public static void Init() {
		step = 0;
		memo = -1;

		DataBase.SetDataBase ();

		graphList = new List<GraphManager> ();
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Graph")) {
			GraphManager gm = obj.GetComponent<GraphManager>();
			gm.Init ();
			graphList.Add (gm);
		}

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
		if (step == memo)
			return;
		foreach (GraphManager gm in graphList) {
			gm.Plot (step);
		}
		memo = step;
	}

	public static void Execute(int s) {
		if (s == memo)
			return;
		foreach (GraphManager gm in graphList) {
			gm.Plot (s);
		}
		step = s;
		memo = s;
	}
}
