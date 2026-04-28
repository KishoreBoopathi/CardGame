using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] GameObject cardOne;
    [SerializeField] GameObject cardTwo;
    GameObject playerArea;
    GameObject opponentArea;
    GameObject gameArea;
     List<GameObject> cards = new List<GameObject>();   

     [SyncVar]
     int cardsPlayed = 0;

    public override void OnStartClient()
    {
        base.OnStartClient();

        playerArea = GameObject.Find("PlayerArea");
        opponentArea = GameObject.Find("OpponentArea");
        gameArea = GameObject.Find("GameArea");

    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();

        cards.Add(cardOne);
        cards.Add(cardTwo);
    }

    [Command]
    public void CmdDealCards()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject card = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0,0,0), Quaternion.identity);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
        }   
    }

    public void PlayCard(GameObject playedCard)
    {
        CmdPlayCard(playedCard);
        cardsPlayed++;
    }

    [Command]
    void CmdPlayCard(GameObject playedCard)
    {
        RpcShowCard(playedCard, "Played");
    }

    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        if(type == "Dealt")
        {
           if(hasAuthority)
           {
               card.transform.SetParent(playerArea.transform, false);
           } 
           else
           {
               card.transform.SetParent(opponentArea.transform, false);
               card.GetComponent<CardFlipper>().FlipCard();
           }
        }
        else if(type == "Played")
        {
            card.transform.SetParent(gameArea.transform, false);
            if(hasAuthority)
            {
                card.GetComponent<CardFlipper>().FlipCard();
            }
        }
    }

    
}
