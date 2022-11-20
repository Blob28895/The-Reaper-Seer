using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEnemy : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    public Animator elevatorAnimator;
    public GameObject topGear;
    public GameObject bottomGear;
    public GameObject building;
	public Transform spawnPoint;

	private Parallax parallax;

	public void Start()
	{
		parallax = building.GetComponent<Parallax>();
	}

    public void stopElevator()
	{
		//parallax.setMoving(false);
		StartCoroutine(parallax.moveToStop());
		topGear.GetComponent<Spin>().SpinSpeed = 0;
		bottomGear.GetComponent<Spin>().SpinSpeed = 0;
	}
	public void startElevator()
	{
		parallax.setMoving(true);
		topGear.GetComponent<Spin>().SpinSpeed = 900;
		bottomGear.GetComponent<Spin>().SpinSpeed = 900;
	}
    
	public void Spawn()
    {
		StartCoroutine(SpawnEnemy());
    }

    public IEnumerator SpawnEnemy()
	{
		float chance = Random.Range(1f, 100f);
		Debug.Log(chance);
		int enemyType = 0;
		for (int i = 1; i < enemyPrefabs.Length; i++)
        {
			if (chance <= 100 / Mathf.Pow(5, i))
            {
				enemyType = i;
            }
        }
		stopElevator();
		elevatorAnimator.SetTrigger("Open");
		yield return new WaitForSeconds(1);
		GameObject minion = Instantiate(enemyPrefabs[enemyType]);
		minion.transform.position = spawnPoint.position;
		yield return new WaitForSeconds(0.5f);
		elevatorAnimator.SetTrigger("Close");
		yield return new WaitForSeconds(1);
		startElevator();

	}

	/* public void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			StartCoroutine(SpawnEnemy());
		}
	}*/
}
