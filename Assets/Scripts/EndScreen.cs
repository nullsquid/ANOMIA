using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
public class EndScreen : MonoBehaviour {
    public PostProcessingProfile blurProfile;
    public PostProcessingProfile defaultProfile;
    public PostProcessingBehaviour cameraPostProcess;

    public void EnableCameraBlur(bool state) {
        if(cameraPostProcess != null && blurProfile != null && defaultProfile != null) {
            if (state) {
                cameraPostProcess.profile = blurProfile;
            }
            else {
                cameraPostProcess.profile = defaultProfile;
            }
        }
    }

}
