using UnityEngine;

public class OcclusionObject : MonoBehaviour
{
    Renderer render;
    public float displayTime;

    void Start()
    {
        render = gameObject.GetComponent<Renderer>();
        displayTime = -1;
    }

    void Update()
    {
        if(displayTime > 0)
        {
            displayTime -= Time.deltaTime;
            render.enabled = true;
        }
        else
        {
            render.enabled = false;
        }
    }

    internal void HitOcclude(float time)
    {
        displayTime = time;
        render.enabled = true;
    }
}
