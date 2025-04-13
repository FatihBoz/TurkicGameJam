using UnityEngine;

public class PlayerChange : MonoBehaviour
{
    [SerializeField] private GameObject changeEffect;
    [SerializeField] private GameObject archer;
    [SerializeField] private GameObject warrior;
    bool isArcherActive = true;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isArcherActive)
            {
                if (archer.TryGetComponent<DashController>(out DashController dashController))
                {
                    dashController.StopAllCoroutines();
                }
                archer.SetActive(false);
                warrior.SetActive(true);
                Destroy(Instantiate(changeEffect, archer.transform.position, Quaternion.identity), 1f);

                warrior.transform.position = archer.transform.position;

                isArcherActive = false;
            }
            else
            {
                if (warrior.TryGetComponent<DashController>(out DashController dashController))
                {
                    dashController.StopAllCoroutines();
                }
                warrior.SetActive(false);
                archer.SetActive(true);

                Destroy(Instantiate(changeEffect, warrior.transform.position, Quaternion.identity), 1f);

                Archer.Instance.RestrainPlayer(false);
                archer.transform.position = warrior.transform.position;
             
                isArcherActive = true;
            }
        }
    }
}
