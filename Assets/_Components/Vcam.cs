using Unity.Cinemachine;
using UnityEngine;

public class Vcam : MonoBehaviour
{
    private float maxTime = .1f;
    private CinemachineCamera cam;
    private Transform target;
    float elapsedTime = 0f;

    private void Awake()
    {
        cam = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        target = PlayerChange.Instance.GetPlayer().transform;
    }


    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= maxTime)
        {
            
            target = PlayerChange.Instance.GetPlayer().transform;
            print(target.name);
            elapsedTime = 0f;
            cam.Follow = target;
            cam.LookAt = target;
        }
    }

}
