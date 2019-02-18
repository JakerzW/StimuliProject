using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

	bool fadeOut = false;
	public float fadeSpeed = 1.0f;
	public float fadeTime = 1.0f;
	public SpriteRenderer targetRenderer;

	// Use this for initialization
	void Start() 
	{

	}
	
	// Update is called once per frame
	void Update() 
	{
		if (fadeOut) 
		{
			float fade = Mathf.SmoothDamp(1.0f, 0.0f, ref fadeSpeed, fadeTime);
			targetRenderer.color = new Color (1.0f, 1.0f, 1.0f, fade);

			if (fade == 0.0f) 
			{
				Destroy(this.gameObject);				
			}
		}
	}

	public void Hit(bool hit)
	{
		if (hit) 
		{
			Destroy (this.gameObject);			
		} 
		else 
		{
			//Fade out and then destroy
			fadeOut = true;
		}
	}
}
