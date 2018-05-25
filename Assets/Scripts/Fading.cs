using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour {

	private CanvasGroup fade;
	private Player player;

	// Use this for initialization
	void Start () {
		fade = GetComponent<CanvasGroup>();
        player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update ()
    {		
		if (Input.GetMouseButtonDown(1) && player.CanSwitch() && player.currentEnergy > player.switchCost)
        {
            StartCoroutine(FadePanel(fade, fade.alpha, 1));
        }		
	}


	public IEnumerator FadePanel(CanvasGroup cg, float start, float end, float lerpTime = 1.0f)
    {
		float _timeSartedLerping = Time.time;
		float timeSinceStarted = Time.time - _timeSartedLerping;
		float percentangeComplete = timeSinceStarted/lerpTime;

		while(true){
			timeSinceStarted = Time.time - _timeSartedLerping;
			percentangeComplete = timeSinceStarted/lerpTime;

			float currentValue = Mathf.Lerp(start,end,percentangeComplete);

			cg.alpha = currentValue;


			if(percentangeComplete >= 1 && end == 1){
				player.ChangeWorld();
				StartCoroutine(FadePanel(fade, fade.alpha, 0));
				break;
			}


			yield return new WaitForEndOfFrame();
		}

	}
}
