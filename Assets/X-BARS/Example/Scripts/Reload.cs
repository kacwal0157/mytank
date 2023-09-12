using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour {

	public void ReloadLevel()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
