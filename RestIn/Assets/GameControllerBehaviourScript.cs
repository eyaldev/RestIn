using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerBehaviourScript : MonoBehaviour
{

    public List<GameObject> m_CardsInDeck;
    // Start is called before the first frame update
    void Start()
    {
        SelectNextCards();
    }

    private void SelectNextCards()
    {
        for (var x = 0; x <= 1; x++)
        {
            var card = m_CardsInDeck[x];
            var cardController = card.GetComponent<MoveCardBehaviour>();
            cardController.SetPosition(new Vector3(-6 + x * 12, 0, 0));
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

    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }

}
