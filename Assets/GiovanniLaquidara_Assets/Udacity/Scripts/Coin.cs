using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour 
{
    //Create a reference to the CoinPoofPrefab
	public GameObject coinPoofPrefab;


    public void OnCoinClicked() {

		// Make sure the poof animates vertically
		Quaternion rotation = Quaternion.Euler(270,0,0);

        // Instantiate the CoinPoof Prefab where this coin is located
	    Instantiate (coinPoofPrefab, this.transform.position, rotation);


        // Destroy this coin.
		StartCoroutine(PlayAudio());
    }

	IEnumerator PlayAudio() {
		GetComponent<Renderer>().enabled = false;
		AudioSource audio = coinPoofPrefab.GetComponent<AudioSource>();
		audio.Play();
		yield return new WaitForSeconds(audio.clip.length);
		Destroy (gameObject);
	}

}
