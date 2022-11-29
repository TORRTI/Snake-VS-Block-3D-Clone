using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    public Transform Basis;
    public float CircleDiameter;
    public float CollisionInterval = 0.2f;
    public Text HPText;
    public Player Player;
    public ParticleSystem DieEffect;
    public ParticleSystem DestroyEffect;
    public AudioPlayer AudioPlayer;
    public int HP { get; private set; }

    private List<Transform> snakeCircles = new List<Transform>();
    private List<Vector3> positions = new List<Vector3>();
    private float collisionTimer = 0;

    void Start()
    {
        positions.Add(Basis.position);
        //HP = 1;
        int startHP = Progress.SnakeLength != -1 ? Progress.SnakeLength : Progress.InitialSnakeLength;
        for (int i = 0; i < startHP; i++)
        {
            AddCircle();
        }
    }

    void FixedUpdate()
    {
        collisionTimer -= Time.deltaTime;

        float distance = (Basis.position - positions[0]).magnitude;

        if (distance > CircleDiameter)
        {
            Vector3 direction = (Basis.position - positions[0]).normalized;

            positions.Insert(0, positions[0] + direction * CircleDiameter);
            positions.RemoveAt(positions.Count - 1);

            distance -= CircleDiameter;
        }

        for (int i = 0; i < snakeCircles.Count; i++)
        {
            snakeCircles[i].position = Vector3.Lerp(positions[i + 1], positions[i], distance / CircleDiameter);
        }
    }

    public void AddCircle()
    {
        HP++;
        HPText.text = (HP + 1).ToString();
        Transform circle = Instantiate(Basis, positions[positions.Count - 1], Quaternion.identity, transform);
        snakeCircles.Add(circle);
        positions.Add(circle.position);
    }

    public void RemoveCircle()
    {
        int lastIndex = snakeCircles.Count - 1;
        
        if (snakeCircles.Count == 0)
        {
            
            Destroy(gameObject);
            Player.Die();
        }
        else
        {
            HP--;
            HPText.text = (HP + 1).ToString();
            Destroy(snakeCircles[lastIndex].gameObject);
            snakeCircles.RemoveAt(lastIndex);
            positions.RemoveAt(lastIndex+1);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Food food))
        {
            for (int i = 0; i < food.Amount; i++)
            {
                AddCircle();
            }

            Progress.SnakeLength = HP;
            Destroy(other.gameObject);
            AudioPlayer.TakeFoodAudio();
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (collisionTimer <= 0 && other.gameObject.TryGetComponent(out Let block))
        {
            DieEffect.Play();
            block.ApplyDamage();
            RemoveCircle();
            collisionTimer = CollisionInterval;
            AudioPlayer.PlayAudio();

            Progress.SnakeLength = HP;
            if (block.Hitpoints == 0)
                DestroyEffect.Play();
        }
    }
}
