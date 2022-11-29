using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody player;
    public Vector3 previousMousePosition;
    public float speed;
    public float sensitivity;
    public GameState GameState;

    private CanvasController canvasController;

    private void Awake()
    {
        canvasController = FindObjectOfType(typeof(CanvasController)) as CanvasController;
    }

    private void FixedUpdate()
    {
        if (canvasController.IsGameActive)
        {
            Moving(player);

            if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - previousMousePosition;
                player.velocity = new Vector3(speed, 0.0f, -delta.x* sensitivity);
            }

            previousMousePosition = Input.mousePosition;
        }
    }

    public void Moving(Rigidbody rb)
    {
        rb.velocity = Vector3.right * speed;
    }

    public void Die()
    {
        canvasController.LoseGame();
        GameState.OnPlayerDied();
        player.velocity = Vector3.zero;
    }
}