using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Bardent
{
    public class MoveCamera : MonoBehaviour
    {
        public static MoveCamera Instance;

        private CinemachineVirtualCamera cinemachineVirtualCamera;

        private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

        private float timeMovement;

        private float timeMovementTotal;

        private float initialIntensity;

        private void Awake()
        {
            Instance = this;

            cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>(); ;
            cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void MovementCamera(float intensity, float frequency, float time)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;
            initialIntensity = intensity;
            timeMovement = time;
        }

        private void Update()
        {
            if(timeMovement > 0)
            {
                timeMovement -= Time.deltaTime;
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(initialIntensity, 0, 1 - (timeMovement / timeMovementTotal));
            }
        }
    }
}
