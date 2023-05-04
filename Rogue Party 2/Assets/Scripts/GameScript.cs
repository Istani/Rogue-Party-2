using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameScript : MonoBehaviour
{
    public Text Playerwins;
    public bool p1Dead = false;
    public bool p2Dead = false;

    public GameObject Player1;
    public GameObject Player2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        p1Dead = Player1.GetComponent<CharMove>().isDead;
        p2Dead = Player2.GetComponent<CharMove>().isDead;

        if(p1Dead)
        {
            Playerwins.text = "Player 2 Wins";
            StartCoroutine(sceneChange());
        }
        if(p2Dead)
        {
            Playerwins.text = "Player 1 Wins";
            StartCoroutine(sceneChange());
        }
    }
    IEnumerator sceneChange()
    {
        yield return new WaitForSeconds(5);
        int randomscene = Random.Range(0, 2);
        Debug.Log(randomscene);
        if (randomscene == 0)
        {
            
            SceneManager.LoadScene("Pong");
        }
        if (randomscene == 1)
        {
            SceneManager.LoadScene("LL");
        }
        
    }
}
