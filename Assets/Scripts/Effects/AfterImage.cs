using UnityEngine;

public class AfterImage : MonoBehaviour
{
    [SerializeField] private float lifeTime = .3f;
    [SerializeField] private float fadeSpeed = 8f;

    private SpriteRenderer sr;
    private Color color;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
    }

    public void Init(Sprite sprite, Vector3 position, Vector3 scale, Color startColor)
    {
        sr.sprite = sprite;
        transform.position = position;
        transform.localScale = scale;
        sr.color = startColor;
    }

    private void Update()
    {
        color.a -= fadeSpeed * Time.deltaTime;
        sr.color = color;

        if (color.a < 0f) Destroy(gameObject);
    }
}
