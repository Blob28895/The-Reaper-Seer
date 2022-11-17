using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public float animationSpeed = 1f;
    private bool moving = true;
    private float offset;

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
        meshRenderer.material.mainTextureOffset += new Vector2(0, animationSpeed * Time.fixedDeltaTime);
        offset = meshRenderer.material.mainTextureOffset.y;
    }
    public IEnumerator moveToStop()
    {
        Debug.Log("moveToStop");
        while (offset % 1f <= 0.47f)
        {
            yield return null;
        }
        int currentOffset = (int)offset;
        meshRenderer.material.mainTextureOffset = new Vector2(0, currentOffset + 0.48f);
        moving = false;
    }

	private void FixedUpdate()
    {
        //Debug.Log(meshRenderer.material.mainTextureOffset.y % 1f);
        if (moving)
        {
            moveBuilding();
        }
    }

}