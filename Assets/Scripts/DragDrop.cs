using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DragDrop : NetworkBehaviour
{
    PlayerManager playerManager;
    Vector2 startingPosition;
    GameObject gameArea;
    GameObject canvas;
    GameObject startParent;
    bool isOverGameArea;
    public bool isDragging;
    bool isDraggable = true;

    private void Start() 
    {
        canvas = GameObject.Find("Main Canvas");  
        gameArea = GameObject.Find("GameArea"); 
        if(!hasAuthority)
        {
            isDraggable = false;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(canvas.transform, true);
        }
    }

    public void StartDragging()
    {
        if(!isDraggable) return;
        startingPosition = transform.position;
        startParent = transform.parent.gameObject;
        isDragging = true;
    }

    public void StopDragging()
    {
        if(!isDraggable) return;
        isDragging = false;
        if(isOverGameArea)
        {
            transform.SetParent(gameArea.transform, false);
            isDraggable = false;
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            playerManager = networkIdentity.GetComponent<PlayerManager>();
            playerManager.PlayCard(gameObject);
        }
        else
        {
            transform.position = startingPosition;
            transform.SetParent(startParent.transform, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag.Equals("GameArea"))
        {
            isOverGameArea = true;
            gameArea = other.gameObject;
        } 
    }

    private void OnCollisionExit2D(Collision2D other) 
    {
        if(other.gameObject.tag.Equals("GameArea"))
        {
            isOverGameArea = false;
        }  
    }
}
