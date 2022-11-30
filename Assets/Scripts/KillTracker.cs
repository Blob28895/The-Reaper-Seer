using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillTracker : MonoBehaviour
{
    List<GameObject> listOfEnemys = new List<GameObject>();
    public GameObject Keycard;
    public SoulBar souls;
    private Player player;
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index = 0;
    public float typingSpeed;
    // Start is called before the first frame update
    void Start()
    {
        //listOfEnemys.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        listOfEnemys.AddRange(GameObject.FindGameObjectsWithTag("Boss"));
        souls.SetMax(staticVariables.maxSouls);
        souls.SetCurrent(staticVariables.currSouls);
        player = GetComponent<Player>();
        //StartCoroutine(Type());
    }

    // Retrieves the list of enemies for use in other scripts
    public List<GameObject> GetListOfEnemies()
    {
        return listOfEnemys;
    }

    public void KilledEnemy(GameObject Enemy)
    {
        if (listOfEnemys.Contains(Enemy))
        {
            //Debug.Log("Enemy died");
            staticVariables.totalKills++;
            if (staticVariables.totalKills == 1)
            {
                NextSentence();
            }
            /*if (staticVariables.totalKills >= 2)
            {
                NextSentence();
            }*/
            if (staticVariables.currSouls < staticVariables.maxSouls)
            {
                staticVariables.currSouls += 1;
            }
            if (staticVariables.currSouls >= staticVariables.maxSouls) {
                staticVariables.currSouls -= staticVariables.maxSouls;
                player.Heal(1);

            }
            //Debug.Log(staticVariables.currSouls);
            souls.SetCurrent(staticVariables.currSouls);
            listOfEnemys.Remove(Enemy);
            int chance = Random.Range(0, listOfEnemys.Count);
            if (chance == 0)
            {
                Keycard.SetActive(true);
            }
        }
        AreOpponentsDead();
        //print(listOfEnemys.Count);
    }
    public bool AreOpponentsDead()
    {
        if (listOfEnemys.Count <= 0)
        {
            // They are dead!
            Keycard.SetActive(true);
            return true;
        }
        else
        {
            // They're still alive dangit
            return false;
        }
    }

    public void AddNewEnemy(GameObject newEnemy)
    {
        // Make sure that enemy is not already in list
        foreach (GameObject enemy in listOfEnemys)
        {
            // If enemy is already present, do not add them to the list
            if (enemy.Equals(newEnemy))
            {
                return;
            }
        }
        // Function will continue here if the enemy is not in the list yet
        listOfEnemys.Add(newEnemy);
    }

    IEnumerator Type()
    {
        //Debug.Log(sentences[index]);
        foreach(char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(3f);
        NextSentence();
        
    }
    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            textDisplay.text = "";
            StartCoroutine(Type());
            index++;
        }
    }
}
