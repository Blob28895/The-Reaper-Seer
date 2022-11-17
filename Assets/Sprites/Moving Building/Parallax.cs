using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public float animationSpeed = 1f;
    private bool moving = true;

    private void Awake()
    {
        moving = true;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void setMoving(bool x)
	{
        moving = x;
	}
    public void moveBuilding()
	{
        meshRenderer.material.mainTextureOffset += new Vector2(0, animationSpeed * Time.deltaTime);
    }
    public IEnumerator moveToStop()
    {
        Debug.Log("moveToStop");
        while (meshRenderer.material.mainTextureOffset.y % 1f < 0.47f || meshRenderer.material.mainTextureOffset.y % 1f > 0.49f)
        {
            yield return null;
        }
        moving = false;
    }

	private void Update()
    {
        //Debug.Log(meshRenderer.material.mainTextureOffset.y % 1f);
        if (moving)
        {
            moveBuilding();
        }
    }

}