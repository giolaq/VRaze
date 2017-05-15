using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour 
{
    //Create a reference to the KeyPoofPrefab and Door
	public GameObject keyPoofPrefab;
	private bool isCollected = false;

	void Update()
	{
		//Bonus: Key Animation
	}

	public void OnKeyClicked()
	{
		isCollected = true;

		// Make sure the poof animates vertically
		Quaternion rotation = Quaternion.Euler(270,0,0);

		// Instatiate the KeyPoof Prefab where this key is located
		Instantiate (keyPoofPrefab, this.transform.position, rotation);

		// Destroy this key.
		StartCoroutine(PlayAudio());


    }


	IEnumerator PlayAudio() {
		GetComponent<Renderer>().enabled = false;
		AudioSource audio = keyPoofPrefab.GetComponent<AudioSource>();
		audio.Play();
		yield return new WaitForSeconds(audio.clip.length);

		GameObject doorGameObj = GameObject.Find("Door");
		if (doorGameObj != null)
		{
			Door door = doorGameObj.GetComponent<Door>();
			// Call the Unlock() method on the Door
			door.Unlock(isCollected);
		}

		// Destroy the key. Check the Unity documentation on how to use Destroy
		Destroy (gameObject);
	}

}
