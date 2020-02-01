using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerBehaviourScript : MonoBehaviour
{
    public GameObject m_CreditsCard;
    public GameObject m_PlayerCard;
    public List<GameObject> m_TutorialCards;
    public List<GameObject> m_CardsInDeck;
    public List<AudioClip> m_MaleBeg;
    public List<AudioClip> m_FeMaleBeg;
    // Start is called before the first frame update
    void Start()
    {
        //SelectNextCards();
    }

    private void SelectNextCards()
    {
        int xOffset = (m_CardsInDeck.Count > 1) ? 6 : 0;
        if (m_PlayerCard == null && m_CreditsCard !=null && m_CardsInDeck.Count > 0)
        {
            //"you" card is being shown:
            StartCoroutine(ExecuteAfterTime(1.5f, () =>
            {
                this.GetComponent<AudioSource>().PlayOneShot(m_LastWords);
            }));
        }
        if (m_PlayerCard == null && m_CreditsCard == null )
        {
            //"credits" card is being shown:
            StartCoroutine(ExecuteAfterTime(3f, () =>
            {
                this.GetComponent<AudioSource>().PlayOneShot(m_CreditSound);
            }));
        }
        for (var x = 0; x <= 1; x++)
        {
            var card = m_CardsInDeck[x];
            var cardController = card.GetComponent<MoveCardBehaviour>();
            cardController.ReviveCard();
            card.SetActive(true);
            cardController.m_SnapTo = new Vector3(((x * 2) - 1) * xOffset, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    internal void CardWasKilled(MoveCardBehaviour cardBehavior)
    {
        HandleTutorialCards(cardBehavior);
        HandleCharacterCards(cardBehavior);
    }

    private void HandleTutorialCards(MoveCardBehaviour cardBehavior)
    {
        if (m_TutorialCards.Contains(cardBehavior.gameObject))
        {
            m_TutorialCards.Remove(cardBehavior.gameObject);
            if (m_TutorialCards.Count > 0)
            {
                m_TutorialCards[0].gameObject.SetActive(true);

            }
            else
            {
                //Start Game
                StartCoroutine(ExecuteAfterTime(.5f, () =>
                {
                    SelectNextCards();
                }));
            }
        }
    }

    private void HandleCharacterCards(MoveCardBehaviour cardBehavior)
    {

        if (m_PlayerCard == null)
        {
            //"you" card is being shown:
            this.GetComponent<AudioSource>().PlayOneShot(m_EvilLaugh);
        }

        if (m_CardsInDeck.Contains(cardBehavior.gameObject))
        {
            m_Kills++;
            m_CardsInDeck.Remove(cardBehavior.gameObject);
            //move remaining card off screen;
            if (m_CardsInDeck.Count == 0 && m_PlayerCard != null)
            {
                m_CardsInDeck.Add(m_PlayerCard);
                m_PlayerCard = null;
            }
            if (m_CardsInDeck.Count == 0 && m_PlayerCard == null && m_CreditsCard!=null)
            {
                m_CardsInDeck.Add(m_CreditsCard);
                m_CreditsCard = null;
            }
            var winningCard = m_CardsInDeck[0];
            //winningCard.GetComponent<MoveCardBehaviour>().RemoveAndLive();
            //m_CardsInDeck.Remove(winningCard);
            //insert the winning card in the end of the list:
            //m_CardsInDeck.Add(winningCard);

            StartCoroutine(ExecuteAfterTime(1f, () =>
            {
                SelectNextCards();
            }));
        }

    }

    int m_Kills = 0;
    public System.Random m_Random = new System.Random();
    public AudioClip m_LastWords;
    public AudioClip m_EvilLaugh;
    public AudioClip m_CreditSound;

    public void Beg(bool isMaleCharacter)
    {
        if (m_Kills < 2)
        {
            //do not play anything at first:
            return;
        }
        if (m_PlayerCard == null)
        {
            //this is the player character
        }
        else
        {
            //this is an npc
            var numCardsLeft = m_CardsInDeck.Count;
            //male or female?
            var beggingSounds = isMaleCharacter ? m_MaleBeg : m_FeMaleBeg;
            if (beggingSounds.Count > 0)
            {
                //choose random begging sounds:
                var randomSound = beggingSounds[m_Random.Next(0, beggingSounds.Count)];
                var audioSource = this.GetComponent<AudioSource>();
                audioSource.PlayOneShot(randomSound);
                //do not repeat anything:
                beggingSounds.Remove(randomSound);
            }
        }

    }

    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }

}
