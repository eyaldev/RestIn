using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerBehaviourScript : MonoBehaviour
{

    public GameObject m_PlayerCard;
    public List<GameObject> m_CardsInDeck;
    public List<AudioClip> m_BegSound;
    // Start is called before the first frame update
    void Start()
    {
        SelectNextCards();
    }

    private void SelectNextCards()
    {
        int xOffset = (m_CardsInDeck.Count>1) ?6:0;
        
        for (var x = 0; x <= 1; x++)
        {
            var card = m_CardsInDeck[x];
            var cardController = card.GetComponent<MoveCardBehaviour>();
            cardController.SetPosition(new Vector3( ( (x*2)-1) * xOffset, 0, 0));
            cardController.ReviveCard();
            card.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
                
    }

    internal void CardWasKilled(MoveCardBehaviour cardBehavior)
    {
        m_CardsInDeck.Remove(cardBehavior.gameObject);
        //move remaining card off screen;
        if(m_CardsInDeck.Count==0 && m_PlayerCard != null)
        {
            m_CardsInDeck.Add(m_PlayerCard);
            m_PlayerCard = null;
        }
        var winningCard = m_CardsInDeck[0];
        winningCard.GetComponent<MoveCardBehaviour>().RemoveAndLive();
        m_CardsInDeck.Remove(winningCard);
        //insert the winning card in the end of the list:
        m_CardsInDeck.Add(winningCard);

        StartCoroutine(ExecuteAfterTime(1f, () =>
        {
            SelectNextCards();
        }));
        

    }
    System.Random m_Random = new System.Random();
    public void Beg()
    {
        var numCardsLeft = m_CardsInDeck.Count;
        if (m_Random.Next(0,2)==1)
        {
            var randomSound = m_BegSound[m_Random.Next(0, m_BegSound.Count)];
            var audioSource = this.GetComponent<AudioSource>();
            audioSource.PlayOneShot(randomSound);
        }
        
    }

    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }

}
