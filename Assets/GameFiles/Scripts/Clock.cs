using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour {

	public float second = 0;
	public int minute = 0, hour = 0;
    private int sunTick = 0;
    public GameObject sun, mainCamera;
    Camera cam;
    Color dayC = new Color(0.19f, 0.3f, 0.47f, 1f), nightC = new Color(0.02f, 0.02f, 0.02f, 1f), dayFogC = new Color(0.36f, 0.36f, 0.36f, 1f);

	void Start () {
		cam = mainCamera.GetComponent<Camera>();
	}

	void Update () {
		second += Time.deltaTime;
		minute = (int) second;
		if(minute > 59){
			hour ++;
			minute = 0;
			second = 0;
			if(hour == 24)hour = 0;
		}
        sunTick ++;
        if(sunTick > 5){
            sun.transform.localEulerAngles = new Vector3(sun.transform.localEulerAngles.x, sun.transform.localEulerAngles.y, second / 4f + hour * 15f - 90f);
            if(hour > 4 && hour < 8){
                cam.backgroundColor = Color.Lerp(nightC, dayC, (second + (hour % 5) * 60f) / 180f);
                RenderSettings.fogColor = Color.Lerp(nightC, dayFogC, (second + (hour % 5) * 60f) / 180f);
            }
            else if(hour > 16 && hour < 20){
                cam.backgroundColor = Color.Lerp(dayC, nightC, (second + (hour % 17) * 60f) / 180f);
                RenderSettings.fogColor = Color.Lerp(dayFogC, nightC, (second + (hour % 17) * 60f) / 180f);
            }
            sunTick = 0;
        }
	}

}
