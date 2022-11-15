using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTracker : MonoBehaviour
{
    List<GameObject> listOfEnemys = new List<GameObject>();
    public GameObject Keycard;
    public SoulBar souls;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        listOfEnemys.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        souls.SetMax(staticVariables.maxSouls);
        souls.SetCurrent(staticVariables.currSouls);
        player = GetComponent<Player>();
    }
    public void KilledEnemy(GameObject Enemy)
    {
        if (listOfEnemys.Contains(Enemy))
        {
            if (staticVariables.currSouls < staticVariables.maxSouls)
            {
                staticVariables.currSouls += 1;
            }
            if(staticVariables.currSouls >= staticVariables.maxSouls){
                staticVariables.currSouls -= staticVariables.maxSouls;
                player.Heal(1);
                
			}
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
}
