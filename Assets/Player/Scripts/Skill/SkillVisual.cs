using UnityEngine;

public class SkillVisual : MonoBehaviour
{
    [SerializeField] private float endScale = 1f;
    private float lifeTime;
    private float elapsedTime = 0f;


    public void Initialize(float lifeTime)
    {
        this.lifeTime = lifeTime;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= lifeTime)
        {
            Destroy(gameObject);
        }
        else
        {
            float t = elapsedTime / lifeTime;
            float scale = Mathf.Lerp(0f, endScale, t);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

}
