using UnityEngine;
using System.Collections;

public class WebLoadingBillboard : MonoBehaviour {
    public void Operate() {
        Managers.Images.GetWebImage(OnWebImage); // Вызов метода в сценарии ImagesManager. 
    }

    private void OnWebImage(Texture2D image) {
        GetComponent<Renderer>().material.mainTexture = image;
    }
}