using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{
	public bool alive = false;
	public bool nextAlive;
	bool prevAlive;
	public int x = -1;
	public int y = -1;
    public float fallspeed = 2f;
    public float init_h=3.3f;

	Renderer renderer;

	// Start is called before the first frame update
	void Start()
    {
		prevAlive = alive;
	}

    // Update is called once per frame
    void Update()
    {
		if (prevAlive != alive) {
			updateColor();
            updateHeight();
		}
        if(gameObject.transform.localScale.y > init_h)
        {
            gameObject.transform.localScale -= new Vector3(0, Time.deltaTime * fallspeed, 0);
        }

		prevAlive = alive;
	}

	public void updateColor()
	{
		if (renderer == null) {
			renderer = gameObject.GetComponent<Renderer>();
		}

		if (this.alive) {
			renderer.material.color = Color.magenta;
		} else {
			renderer.material.color = Color.yellow;
		}
	}
    public void updateHeight()
    {
  
        if (this.alive)
        {
            gameObject.transform.localScale += new Vector3(0,10f,0);
        }
    }

    private void OnMouseDown()
	{
		alive = !alive;
	}
}
