using UnityEngine;

public class ControllerScene : MonoBehaviour {
	public GameObject Game;
	public GameObject NewScene;

	public void ExibirTela() {
		Game.SetActive(false);
		NewScene.SetActive(true);
	}
}
