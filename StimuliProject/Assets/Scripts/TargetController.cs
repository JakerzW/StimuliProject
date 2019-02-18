using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    
	public float fadeTime = 1.0f;
	public SpriteRenderer targetRenderer;

	// Use this for initialization
	void Start() 
	{

	}
	
	// Update is called once per frame
	void Update() 
	{
		
	}

	public void Hit(bool hit)
	{
		if (hit) 
		{
			Destroy (this.gameObject);			
		}
        else if (!hit)
		{
            //Fade out and then destroy
            StartCoroutine(FadeOut());
        }
	}

    public IEnumerator FadeOut()
    {
        float start = Time.time;
        Debug.Log("Fading...");
        while (Time.time <= start + fadeTime)
        {
            Color newColor = targetRenderer.color;
            newColor.a = 1f - Mathf.Clamp01((Time.time - start) / fadeTime);
            //Debug.Log("Fade: " + newColor.a);
            targetRenderer.color = newColor;
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }
}
