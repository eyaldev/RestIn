using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCardBehaviour : MonoBehaviour
{
    private Vector3 m_LastDrag;
    public Vector3 m_SnapTo;
    private bool m_ShouldKillCard;
    private float m_SnapSpeed=20;
    private GameControllerBehaviourScript m_GameController;
    public bool m_WaitingForInteraction;

    // Start is called before the first frame update
    void Start()
    {
        SetPosition(this.transform.position);
        m_GameController = GameObject.FindObjectOfType<GameControllerBehaviourScript>();
        ReviveCard();
    }

    public void ReviveCard()
    {
        m_WaitingForInteraction = true;
        m_ShouldKillCard = false;
    }

    public void SetPosition(Vector3 newPosition)
    {
        m_SnapTo = newPosition;
        this.transform.position = m_SnapTo;
    }

    private void OnMouseDrag()
    {
        Vector3 currentMouse = GetMouseWorldCoordinates(); Debug.Log("drag" + currentMouse);
        var delta = currentMouse - m_LastDrag;
        this.transform.position += new Vector3(delta.x, delta.y, 0);
        m_LastDrag = currentMouse;
    }

    private static Vector3 GetMouseWorldCoordinates()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    internal void RemoveAndLive()
    {
        if (m_WaitingForInteraction)
        {
            this.m_SnapTo = this.transform.position + new Vector3(0, 10, 0);
            m_WaitingForInteraction = false;
        }

    }

    private void OnMouseDown()
    {
        m_LastDrag = GetMouseWorldCoordinates();
    }
    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            var ratio = Screen.width / Screen.height;
            if (!m_ShouldKillCard)
            {
                if (Mathf.Abs(this.transform.position.y) > 0.8 * Camera.main.orthographicSize)
                {
                    m_ShouldKillCard = true;
                }
                if (Mathf.Abs(this.transform.position.x) > 0.8 * Camera.main.orthographicSize * ratio)
                {
                    m_ShouldKillCard = true;
                }
                if (m_ShouldKillCard)
                {
                    KillCard();
                }
            }
            var delta = m_SnapTo-this.transform.position ;
            var speed = m_SnapSpeed*Time.deltaTime;
            if (delta.magnitude > speed)
            {
                delta = delta.normalized * speed;
            }
            this.transform.position += delta;
        }
    }

    private void KillCard()
    {

        if (m_WaitingForInteraction)
        {
            //send card off screen
            m_SnapTo = (this.transform.position - m_SnapTo).normalized * 100;
            m_SnapSpeed = 40;
            m_GameController.CardWasKilled(this);
            m_WaitingForInteraction = false;
        }

    }
}
