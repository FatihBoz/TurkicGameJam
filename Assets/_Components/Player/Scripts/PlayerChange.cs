using System;
using UnityEngine;

public class PlayerChange : MonoBehaviour
{
    public static PlayerChange Instance { get; private set; }
    [SerializeField] private Transform initialTransform;
    [SerializeField] private GameObject changeEffect;
    [SerializeField] private GameObject archer;
    [SerializeField] private GameObject warrior;
    bool isArcherActive = true;
    private GameObject player;
    public static Action OnPlayerChange;

    private void Awake()
    {
        Instance = this;
        player = archer;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnPlayerChange?.Invoke();
            if (isArcherActive)
            {
                player = warrior;
                if (archer.TryGetComponent<DashController>(out DashController dashController))
                {
                    dashController.StopAllCoroutines();
                }
                archer.SetActive(false);
                warrior.SetActive(true);

                //Destroy(Instantiate(changeEffect, archer.transform.position, Quaternion.identity), 1f);
                warrior.transform.localPosition = archer.transform.localPosition;

                isArcherActive = false;
            }
            else
            {
                player = archer;
                if (warrior.TryGetComponent<DashController>(out DashController dashController))
                {
                    dashController.StopAllCoroutines();
                }
                warrior.SetActive(false);
                archer.SetActive(true);

                //Destroy(Instantiate(changeEffect, warrior.transform.position, Quaternion.identity), 1f);

                Archer.Instance.RestrainPlayer(false);
                archer.transform.position = warrior.transform.position;
             
                isArcherActive = true;
            }
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath()
    {
        archer.transform.position = initialTransform.position;
        warrior.transform.position = initialTransform.position;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= OnEnemyDeath;
    }
}
