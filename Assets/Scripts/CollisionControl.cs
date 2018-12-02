using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionControl : MonoBehaviour {
    public SplatPainter painter;
    public ParticleSystem particles;
    public void OnCollisionEnter(Collision collision) {
        var contactUV = Vector2.zero;
        var contacts = 0;
        RaycastHit hit;

        foreach(var contact in collision.contacts) {
            var ray = new Ray(contact.point - contact.normal, contact.normal);
            if(Physics.Raycast( ray, out hit) ){
                Debug.Log("Raycast" + ray.origin +"," + ray.direction);
                contacts++;
                contactUV += hit.textureCoord;
            }
        }

        Debug.Log("Collision");
        if (contacts > 0) {
            Debug.Log("Contact");
            contactUV /= contacts; // compute average
            painter.DrawHit(contactUV);
        }
    }
}
