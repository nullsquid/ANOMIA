using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour {

    public Color solidColor = Color.white;
    public Color clearColor = new Color(1f, 1f, 1f, 0f);

    public float delay = 0.5f;
    public float timeToFade = 1f;
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

    MaskableGraphic _graphic;

    private void Awake() {
        _graphic = GetComponent<MaskableGraphic>();
    }

    void UpdateColor(Color newColor) {
        _graphic.color = newColor;
    }

    public void FadeOff() {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", solidColor,
            "to", clearColor,
            "time", timeToFade,
            "delay", delay,
            "easetype", easeType,
            "onupdatetarget", gameObject,
            "onupdate", "UpdateColor"
        ));
    }
    public void FadeOn() {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", clearColor,
            "to", solidColor,
            "time", timeToFade,
            "delay", delay,
            "easetype", easeType,
            "onupdatetarget", gameObject,
            "onupdate", "UpdateColor"
        ));
    }
}
