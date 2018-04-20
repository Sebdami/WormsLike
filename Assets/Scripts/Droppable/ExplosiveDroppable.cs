using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDroppable : Droppable {

    public Explosion explosion;

    protected virtual void Explode()
    {
        explosion.Explode(transform.position);
    }
}
