using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeGameObject : MonoBehaviour
{
    public float lifeTime;
    private float elapsedTime = 0.0f;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        float alpha = (lifeTime - elapsedTime) / lifeTime;
        spriteRenderer.color = new Color(1f, 1f, 1f, alpha);

        if (elapsedTime >= lifeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
