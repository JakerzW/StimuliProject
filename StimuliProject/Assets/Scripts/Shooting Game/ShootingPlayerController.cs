using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPlayerController : MonoBehaviour
{
    public Canvas gameHUD;
	
	// Update is called once per frame
	void Update () 
	{
        //Check if input collides with target
		if (Input.GetMouseButtonDown(0)) 
		{
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 100);

            if (hit.collider != null)
            {
				gameHUD.GetComponent<ShootingGameController>().TargetHit(hit.collider.gameObject, hit.collider.GetComponent<ShootingTargetController>().position);                
            }      
		}
	}
}
