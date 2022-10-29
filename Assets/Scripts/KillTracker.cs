using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTracker : MonoBehaviour
{
    List<GameObject> listOfEnemys = new List<GameObject>();
    public GameObject Keycard;
    // Start is called before the first frame update
    void Start()
    {
        listOfEnemys.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }
    public void KilledEnemy(GameObject Enemy)
    {
        if (listOfEnemys.Contains(Enemy))
        {
            listOfEnemys.Remove(Enemy);
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
