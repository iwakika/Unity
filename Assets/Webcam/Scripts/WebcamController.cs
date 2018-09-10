using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WebcamController : MonoBehaviour {

	[SerializeField] private RawImage display;
	[SerializeField] private Text startStopText;
	
	private int currentCamIndex = 0;
	private WebCamTexture texture;
	private Texture2D snap;
	private const string path = "Assets/Resources/Snapshots/";
	
	public void SwitchCam_Clicked () {
		if (WebCamTexture.devices.Length > 0) {
			currentCamIndex += 1;
			currentCamIndex %= WebCamTexture.devices.Length;

			if (texture != null) {
				StopWebcam();
				StartStopCam_Clicked();
			}
		}
	}

	public void StartStopCam_Clicked () {
		if (texture != null) {
			StopWebcam();
			startStopText.text = "Start Camera";
		} else {
			WebCamDevice device = WebCamTexture.devices[currentCamIndex];
			texture = new WebCamTexture(device.name);
			display.texture = texture;
			
			texture.Play();	
			startStopText.text = "Stop Camera";
			snap = new Texture2D(texture.width, texture.height);
		}
	}
	
	private void StopWebcam () {
		display.texture = null;
		texture.Stop();
		texture = null;
	}

	// OS METODOS ABAIXO NAO SAO USADOS NA CENA DE DEMONSTRACAO
	// MAS PODEM SER USADOS PARA TIRAR UMA FOTO E SALVA-LA EM JPG
	// USANDO O METODO CreateJPG

	public void CreateJPG() {
		byte[] myImage = TakeSnapshot();

		SaveSnapshot(myImage, path, "image_unity.jpg");
	}

	private byte[] TakeSnapshot() {
		snap.SetPixels(texture.GetPixels());
		snap.Apply();
		return snap.EncodeToJPG();
	}
	
	private void SaveSnapshot(byte[] image, string path, string fileName) {
		File.WriteAllBytes(path + fileName, image);
	}
}
