using UnityEngine;
using System.Collections;

public interface IEnemy : IAction {
	void initialise();
	void ActionLoop ();
}