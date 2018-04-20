using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteDroppable : ExplosiveDroppable {
    [SerializeField]
    float explodeInSecondsTime = 3.0f;
    float timer = 0.0f;

    bool isActive = false;

    private void Update()
    {
        if(isActive)
        {
            timer -= Time.deltaTime;
            transform.GetChild(1).GetComponent<TextMesh>().text = ((int)(timer + 1)).ToString();
            if(timer <= 0)
            {
                timer = 0.0f;
                explosion.Explode(transform.position);
                Destroy(gameObject);
            }
        }
    }

    protected override void Explode()
    {
        base.Explode();
        isActive = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        Destroy(gameObject, 2.5f);
        GetComponent<Collider>().enabled = false;
        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Rigidbody>());
    }

    public override void Drop()
    {
        base.Drop();
        isActive = true;
        timer = explodeInSecondsTime;
        transform.GetChild(1).gameObject.SetActive(true);
        transform.rotation = Quaternion.identity;
    }

}
