using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimUI.Vivid
{
	public class Restart : MonoBehaviour
	{
		void Update()
		{
			if (Input.GetKeyDown("r") && Application.isEditor)
			{
				SceneManager.LoadScene("SlimUI_SciFi_Demo", LoadSceneMode.Single);
			}
		}
	}
}