using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour 
{
    // Create a boolean value called "locked" that can be checked in Update() 
	private bool locked = true;
	private bool doorOpening = false;

	public AudioClip[] soundFiles;
	public AudioSource soundSource;


    void Update() {
        // If the door is unlocked and it is not fully raised
        // Animate the door raising up
		if (doorOpening == true) {
			if (transform.position.y < 25.5) {
				transform.Translate (0, 2 * Time.deltaTime, 0, Space.World);
			} else {
				StopOpeningAudio ();
				doorOpening = false;
			}
		}
    }

	public void Unlock(bool isKeyCollected)
    {
		if (isKeyCollected == true) {
			// You'll need to set "locked" to false here
			locked = false;
			doorOpening = true;
			PlayUnlockedDoorAudio ();
		}
      
    }

	public void OpenDoor() {
		doorOpening = true;
		PlayOpeningAudio();
	}

	public void OnDoorClicked() {

		if (locked == false) {
			OpenDoor ();
		} else {
			PlayLockedDoorAudio ();
		}
	}

	private void PlayOpeningAudio() {
		soundSource.clip = soundFiles [2];
		soundSource.Play ();
	}

	private void StopOpeningAudio() {
		soundSource.Stop();
	}

	private void PlayLockedDoorAudio() {
		soundSource.clip = soundFiles [0];
		soundSource.Play ();	
	}

	private void PlayUnlockedDoorAudio() {
		soundSource.clip = soundFiles [1];
		soundSource.Play ();	
	}

}
