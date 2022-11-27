using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScreen : MonoBehaviour
{
    private ElevatorButton button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<ElevatorButton>();
    }

    public void SwiftStrike()
	{
        staticVariables.dashDamage = true;
        StartCoroutine(button.TransitionScene());
	}
    public void BlazingSpeed()
	{
        staticVariables.movementMultiplier = 1.35f;
        staticVariables.moveSpeedEnabled = true;
        StartCoroutine(button.TransitionScene());
    }
    public void StrongerMoves()
	{
        staticVariables.heavyAtkEnabled = true;
        StartCoroutine(button.TransitionScene());
    }

}
