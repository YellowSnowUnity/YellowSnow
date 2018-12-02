using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseControl : MonoBehaviour {

    // you could use this to paint directly on canvas instead of using the particles if you wanted
    private SplatPainter painter;

    public Camera Camera;
    public ParticleSystem Stream;
	void Update () {
        RaycastHit hit;
        bool overUI = EventSystem.current.currentSelectedGameObject != null;

        if (Input.GetKey(KeyCode.Mouse0) && !overUI) {
            Stream.enableEmission = true;
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out hit)) {
                Stream.gameObject.transform.LookAt(hit.point);
                if (painter != null)
                    painter.DrawHit(hit.textureCoord);
            }
        } else {
            Stream.enableEmission = false;
        }

	}
}
