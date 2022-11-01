using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public Transform target;
    private bool show = false;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(target.position.x);
        // Checks to see if the Reaper is near the end of the level
        if (!show && !GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().enabled)
        {
            show = true;
            Debug.Log("Win Triggered");
            StartCoroutine(WinScreen());
        }
    }
    IEnumerator WinScreen()
    {
        yield return new WaitForSeconds(3f);
        // Moves the win screen to the Reaper's location
        //transform.position = target.position;
        // Enables the Sprite renderer to have the win screen appear
        GetComponent<SpriteRenderer>().enabled = true;
        // Freezes the Reaper in place
        // FindObjectOfType<Player>().GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
