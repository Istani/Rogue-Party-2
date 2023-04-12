using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int PlayerNum = 5;
    public int enemyType;
    public GameObject Player1;
    public GameObject Player2;
    public int goalDamage = 10;
    private bool isMove;
    public Vector3 startPoint = new Vector3(0, 1.25f, 0);
    private CharacterController characterController;
    Vector3 directionVec = Vector3.zero;
    float ballspeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        transform.position = startPoint;
        isMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMove ==true)
        {
        characterController.Move(Movement() * Time.deltaTime);
        }
        else if(isMove ==false)
        {
            transform.position = startPoint;
            
        }
        
    }

    Vector3 Movement()
    {
        Vector3 moveVector = Vector3.zero;
        moveVector = directionVec * ballspeed;
        return moveVector;
    }
    public void TakeDamage(int damage, Vector3 direction, int PlayerNumber)
    {
        if(enemyType == 0)
        {
            pongDamage(damage, direction);
        }

        if(enemyType == 1)
        {
            llDamage(damage, direction);
            PlayerNum = PlayerNumber;
        }

        
    }

    private void pongDamage(int damage, Vector3 direction) 
    {
        isMove = true;
        ballspeed = damage;
        directionVec = direction + AddNoiseOnAngle(0, 15); //achse?
    }

    private void llDamage(int damage, Vector3 direction)
    {
        isMove = true;
        ballspeed = ballspeed + (0.01f * damage);
        directionVec = new Vector3(direction.x, AddNoiseOnAngle(0, 30).x, 0);
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        string collideto =collisionInfo.collider.tag;
        Debug.Log("hit" + collideto );
        
        if (collideto == "PongVerWall") 
        {
            directionVec = new Vector3(directionVec[0] * (-1), 0, directionVec[2]);
            if(ballspeed > 1)
            {
                ballspeed -= 1;
            }
            
        }
        if (collideto == "PongHorWall")
        {
            directionVec = new Vector3(directionVec[0], 0, directionVec[2] * (-1));
            if (ballspeed > 1)
            {
                ballspeed -= 1;
            }
        }
        if (collideto == "GoalP1")
        {
            isMove = false;
            Debug.Log("TOOOOOOR");
            Player1.GetComponent<CharMove>().GetDamage(goalDamage);
        }
        if (collideto == "GoalP2")
        {
            isMove = false;
            Debug.Log("TOOOOOOR");
            Player2.GetComponent<CharMove>().GetDamage(goalDamage);
        }

        if (collideto == "LLWallVer")
        {
            directionVec = new Vector3(directionVec[0] * (-1), directionVec[1] + AddNoiseOnAngle(0, 15).x, 0);

        }
        if(collideto == "LLWallHor")
        {
            directionVec = new Vector3(directionVec[0] + AddNoiseOnAngle(0, 15).x, directionVec[1] * (-1), 0);

        }

        if(enemyType == 1 && collideto == "Player" && collisionInfo.collider.GetComponent<CharMove>().PlayerID != PlayerNum && collisionInfo.collider.GetComponent<CharMove>().isDead == false)
        {
            isMove = false;
            collisionInfo.collider.GetComponent<CharMove>().GetDamage((int)ballspeed);
            ballspeed = 0;
        }

        //directionVec = directionVec * (-1);
    }
    Vector3 AddNoiseOnAngle(float min, float max)
    {
        // Find random angle between min & max inclusive
        float xNoise = Random.Range(min, max);
        float zNoise = Random.Range(min, max);

        // Convert Angle to Vector3
        Vector3 noise = new Vector3(
          Mathf.Sin(2 * Mathf.PI * xNoise / 360),
          0,
          Mathf.Sin(2 * Mathf.PI * zNoise / 360)
        );
        return noise;
    }
}
