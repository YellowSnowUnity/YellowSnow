using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionControl : MonoBehaviour {
    public SplatPainter painter;
    public ParticleSystem particles;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    public void OnParticleCollision(GameObject other) {
        RaycastHit hit;
        int numCollisions = particles.GetCollisionEvents(other, collisionEvents);
        Vector2 point = Vector2.zero;
        int count = 0;

        for(var i = 0; i < numCollisions; i++) {
            var ce = collisionEvents[i];
            var ray = new Ray(origin: ce.intersection, direction: -ce.normal);
            if(Physics.Raycast( ray, out hit) ) {
                count ++;
                point += hit.textureCoord;
            }
        }

        if (count > 0) {
            painter.DrawHit(point / count);
        }
    }
}
