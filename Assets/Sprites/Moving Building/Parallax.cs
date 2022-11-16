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
    /*public void moveToStop()
    {
        Debug.Log("moveToStop");
        while (meshRenderer.material.mainTextureOffset.x % 1f != 0.48f)
        {
            meshRenderer.material.mainTextureOffset += new Vector2(0, animationSpeed * Time.deltaTime);

        }
        moving = false;
    }*/

	private void Update()
    {
        if (moving)
        {
            moveBuilding();
        }
    }

}