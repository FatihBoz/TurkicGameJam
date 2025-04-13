using UnityEngine;

public class ObjectDestroy : MonoBehaviour
{
    public GameObject objectDestroy;


    public void DestroyObject()
    {
        GameObject instantiatedObject = Instantiate(objectDestroy, transform.position, Quaternion.identity);
        Destroy(instantiatedObject, 5f); // Nesne 5 saniye sonra yok edilir
        Destroy(gameObject);
    }
}
