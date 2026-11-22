using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BotonUI : MonoBehaviour, IInteractable
{
    void Awake()
    {
        var rt = (RectTransform)transform;
        var col = GetComponent<BoxCollider>();
        if (col == null) col = gameObject.AddComponent<BoxCollider>();

        col.isTrigger = true;   // así no estorba al movimiento del jugador
        col.size = new Vector3(rt.rect.width, rt.rect.height, 10f);
        col.center = new Vector3(rt.rect.center.x, rt.rect.center.y, 0f);
    }

    public void Interact()
    {
        GetComponent<Button>().onClick.Invoke();
    }
}