using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatPainter : MonoBehaviour {

    public Material DrawMaterial;
    public RenderTexture SplatMap;
    public bool Debug;
    private Vector2? Coordinate = null;

    public float SlowResetRate = 0.05f;
    public float SlowResetTime = 1f;
    private float OrigRate = 0;
    void Start() {
        OrigRate = DrawMaterial.GetFloat("_RegenAmount");
        Reset();
    }

    public void Reset() {
        var active = RenderTexture.active;
        RenderTexture.active = SplatMap;
        GL.Clear(false, true, Color.black);
        RenderTexture.active = active;
    }

    public void SlowReset() {
        StartCoroutine(SlowResetWorker());
    }

    public IEnumerator SlowResetWorker() {
        // if we do it here then pressing it twice will break it
        //var OrigRate = DrawMaterial.GetFloat("_RegenAmount");
        DrawMaterial.SetFloat("_RegenAmount",  SlowResetRate);
        yield return new WaitForSeconds(SlowResetTime);
        DrawMaterial.SetFloat("_RegenAmount",  OrigRate);
    }

	public void DrawHit (Vector2 hit_uv) {
        Coordinate = hit_uv;
	}

    private void Blit() {
        RenderTexture temp;
        temp = RenderTexture.GetTemporary(SplatMap.width, SplatMap.height, SplatMap.depth, SplatMap.format);
        Graphics.Blit(SplatMap, temp);
        Graphics.Blit(temp, SplatMap, DrawMaterial);
        RenderTexture.ReleaseTemporary(temp);
    }

    public void Update() {
        if (Coordinate == null) {
            DrawMaterial.SetInt("_Drawing", 0);
        } else {
            DrawMaterial.SetInt("_Drawing", 1);
            DrawMaterial.SetVector("_Coordinate", new Vector4(Coordinate.Value.x, Coordinate.Value.y));
            Coordinate = null;
        }
        Blit();

    }

    void OnGUI() {
        if (!Debug)
            return;

        GUI.DrawTexture(new Rect(0,0,256,256), SplatMap, ScaleMode.ScaleToFit, false, 1);
    }
}
