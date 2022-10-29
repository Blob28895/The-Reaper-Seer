using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasController : MonoBehaviour
{
    List<GameObject> listOfVents = new List<GameObject>();
    List<GasScript> listOfScripts = new List<GasScript>();
    // Start is called before the first frame update
    void Start()
    {
        listOfVents.AddRange(GameObject.FindGameObjectsWithTag("Vent"));
        foreach(GameObject vent in listOfVents)
		{
            listOfScripts.Add(vent.GetComponent<GasScript>());
		}
    }

    public void FireAllVents()
	{
        Debug.Log("Firing All Vents");
        foreach(GasScript script in listOfScripts)
		{
            script.FireGas();
		}
	}
    public void FireRandomVents(int number)
	{
        Debug.Log("Firing random vents");
        List<int> chosen = new List<int>();
        int curr, stopper = 0;
        while (stopper < number) {
            curr = Random.Range(0, listOfScripts.Count);
            if (!chosen.Contains(curr))
            {
                chosen.Add(curr);
                stopper += 1;
            }
        }
		foreach(int i in chosen)
		{
            listOfScripts[i].FireGas();
		}
	}

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown("y"))
		{
            FireRandomVents(3);
		}
    }
}
