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
    private int index;
    public float typingSpeed;
    // Start is called before the first frame update
    void Start()
    {
        listOfEnemys.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        listOfEnemys.AddRange(GameObject.FindGameObjectsWithTag("Boss"));
        souls.SetMax(staticVariables.maxSouls);
        souls.SetCurrent(staticVariables.currSouls);
        player = GetComponent<Player>();
        StartCoroutine(Type());
    }

    public void KilledEnemy(GameObject Enemy)
    {
        if (listOfEnemys.Contains(Enemy))
        {
            Debug.Log("Enemy died");
            staticVariables.kills += 1;
            if (staticVariables.kills == 1)
            {
                NextSentence();
            }
            if (staticVariables.kills != 1)
            {
                textDisplay.text = "";
            }
            if (staticVariables.currSouls < staticVariables.maxSouls)
            {
                staticVariables.currSouls += 1;
            }
            if(staticVariables.currSouls >= staticVariables.maxSouls){
                staticVariables.currSouls -= staticVariables.maxSouls;
                player.Heal(1);
                
			}
            Debug.Log(staticVariables.currSouls);
            souls.SetCurrent(staticVariables.currSouls);
            listOfEnemys.Remove(Enemy);
            int chance = Random.Range(0, listOfEnemys.Count);
            if(chance == 0)
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

    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
