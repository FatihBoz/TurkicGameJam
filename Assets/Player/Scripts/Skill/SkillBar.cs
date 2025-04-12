using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image closerImage;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private float cooldown;
    bool isOnCooldown = false;
    float elapsedTime = 0f;
    public void Cooldown(float cd)
    {
        cooldown = cd;
        closerImage.gameObject.SetActive(true);
        closerImage.fillAmount = 1;
        isOnCooldown = true;
    }

    private void Update()
    {
        if (isOnCooldown)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= cooldown)
            {
                elapsedTime = 0f;
                isOnCooldown = false;
                closerImage.gameObject.SetActive(false);
                cooldownText.text = "";
            }
            else
            {

                closerImage.fillAmount = 1 - (elapsedTime / cooldown);
                cooldownText.text = Mathf.CeilToInt((cooldown - elapsedTime)).ToString();
            }
        }
    }


}
