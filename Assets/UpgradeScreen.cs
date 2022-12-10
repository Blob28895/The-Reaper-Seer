using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScreen : MonoBehaviour
{
    private ElevatorButton button;
    private bool upgradeSelected;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<ElevatorButton>();
        upgradeSelected = false;
    }

    public void SwiftStrike()
	{
        if (!upgradeSelected)
        {
            upgradeSelected = true;
            staticVariables.dashDamage = true;
            StartCoroutine(button.TransitionScene());
        }
	}
    public void BlazingSpeed()
	{
        if (!upgradeSelected)
        {
            upgradeSelected = true;
            staticVariables.movementMultiplier = 1.35f;
            staticVariables.moveSpeedEnabled = true;
            StartCoroutine(button.TransitionScene());
        }
    }
    public void StrongerMoves()
	{
        if (!upgradeSelected)
        {
            upgradeSelected = true;
            staticVariables.heavyAtkEnabled = true;
            StartCoroutine(button.TransitionScene());
        }
    }
    public void SharpBlade()
	{
        if (!upgradeSelected)
        {
            upgradeSelected = true;
            staticVariables.damageMultiplier = 1.3f;
            StartCoroutine(button.TransitionScene());
        }
    }
    public void GoodDefense()
	{
        if (!upgradeSelected)
        {
            upgradeSelected = true;
            Player.maxHealth += 3;
            StartCoroutine(button.TransitionScene());
        }
    }
    public void BurstsOfSpeed()
	{
        if (!upgradeSelected)
        {
            upgradeSelected = true;
            staticVariables.dashDistanceMult = 1.4f;
            StartCoroutine(button.TransitionScene());
        }
    }
    public void HealPlayer()
	{
        staticVariables.currHealth = Player.maxHealth;
	}

}
