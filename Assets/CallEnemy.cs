using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;

    public Animator elevatorAnimator;
    public GameObject topGear;
    public GameObject bottomGear;
    public GameObject building;
	public Transform spawnPoint;

	private Parallax parallax;
	private bool stopped = false;

	public void Start()
	{
		parallax = building.GetComponent<Parallax>();
	}

    public void stopElevator()
	{
		parallax.setMoving(false);
		topGear.GetComponent<Spin>().SpinSpeed = 0;
		bottomGear.GetComponent<Spin>().SpinSpeed = 0;
	}
	public void startElevator()
	{
		parallax.setMoving(true);
		topGear.GetComponent<Spin>().SpinSpeed = 900;
		bottomGear.GetComponent<Spin>().SpinSpeed = 900;
	}
    
    public IEnumerator SpawnEnemy()
	{
		stopElevator();
		elevatorAnimator.SetTrigger("Open");
		yield return new WaitForSeconds(1);
		GameObject minion = Instantiate(enemyPrefab);
		minion.transform.position = spawnPoint.position;
		yield return new WaitForSeconds(0.5f);
		elevatorAnimator.SetTrigger("Close");
		yield return new WaitForSeconds(1);
		startElevator();

	}

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			StartCoroutine(SpawnEnemy());
		}
	}
}
