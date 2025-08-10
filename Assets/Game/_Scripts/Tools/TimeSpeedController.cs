using UnityEngine;
using VContainer.Unity;

public class TimeSpeedController : ITickable
{
    public void Tick()
    {
        if (Input.GetMouseButton(3))
        {
            Time.timeScale = 5;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else if (!Mathf.Approximately(Time.timeScale, 1))
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }
    }
}