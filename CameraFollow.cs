using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;

	public float dampTime = 0.15f;
	Vector3 velocity = Vector3.zero;
    Camera camera;
    float gameHeight;
    float gameWidth;

    void Start(){
        camera = GetComponent<Camera>();
        gameHeight = camera.orthographicSize * 2;
        gameWidth = gameHeight * camera.aspect;
    }

	// Update is called once per frame
	void Update () 
	{
        
        // mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (target)
		{
                Vector3 point = camera.WorldToViewportPoint(target.position);
                Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
                Vector3 destination = transform.position + delta;
                // if(newPos){
                //     transform.position += delta.normalized*0.1f;
                //     newPos = false;
                // }
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
                
                if(transform.position.x>gameWidth*0.25){ 
                    transform.position = new Vector3(gameWidth*0.25f, transform.position.y, transform.position.z);
                }
                if(transform.position.x<-gameWidth*0.25){ 
                    transform.position = new Vector3(-gameWidth*0.25f, transform.position.y, transform.position.z);
                }
                if(transform.position.y>gameHeight*0.25){ 
                    transform.position = new Vector3(transform.position.x, gameHeight*0.25f, transform.position.z);
                }
                if(transform.position.y<-gameHeight*0.25){ 
                    transform.position = new Vector3(transform.position.x, -gameHeight*0.25f, transform.position.z);
                }
                

            
        }   
    }
}
