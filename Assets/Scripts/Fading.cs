using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour {

	private CanvasGroup fade;
	public Player player;

	// Use this for initialization
	void Start () {
		fade = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown(1) && (player.transform.position.x < 39f || player.transform.position.x > 52f) && player.currentEnergy > 0)
            {
               StartCoroutine(FadePanel(fade, fade.alpha, 1));
            }		
	}


	public IEnumerator FadePanel(CanvasGroup cg, float start, float end, float lerpTime = 1.0f){
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
