using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation {

	public static bool isEnabled = false;
	public static int step = 0;

	private static List<GraphManager> graphList;
	private static int memo = 0;

	public static void Init() {
		step = -1;
		memo = -2;

		DataBase.SetDataBase ();

		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("GraphContent")) {
			obj.GetComponent<GraphContent> ().Init ();
		}

		graphList = new List<GraphManager> ();
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Graph")) {
			GraphManager gm = obj.GetComponent<GraphManager>();
			//gm.Init ();
			graphList.Add (gm);
		}

		if (!isEnabled)
			GameObject.Find ("MyAppManager").GetComponent<MyAppManager> ().ActivateSimuPanel ();
		
		GameObject.Find ("SimuPanel").GetComponent<SimulationController> ().Init ();
		
		isEnabled = true;
	}

	public static void Register(GraphManager gm) {
		graphList.Add (gm);
	}

	public static void Register(GraphManager[] gmArray) {
		foreach (GraphManager gm in gmArray)
			graphList.Add (gm);
	}

	public static void Remove(GraphManager gm) {
		graphList.Remove (gm);
	}

	public static void Reset() {
		step = -1;
	}

	public static bool IsEnd() {
		return step >= DataBase.step - 1;
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
