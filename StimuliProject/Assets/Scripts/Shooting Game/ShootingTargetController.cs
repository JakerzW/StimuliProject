using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTargetController : MonoBehaviour
{
    //Target variables defined
	public float fadeTime = 1.0f;
	public SpriteRenderer targetRenderer;
    public int position;

    //If the target is hit, destroy or fade out
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

    //Fade out function
    public IEnumerator FadeOut()
    {
        float start = Time.time;
        while (Time.time <= start + fadeTime)
        {
            Color newColor = targetRenderer.color;
            newColor.a = 1f - Mathf.Clamp01((Time.time - start) / fadeTime);
            targetRenderer.color = newColor;
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }

    //Update the targets position
    public void UpdatePosition(int newPos)
    {
        position = newPos;
    }
}
