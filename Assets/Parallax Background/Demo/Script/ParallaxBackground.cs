using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public bool Parallax_Enabled = false;

    public float[] Layer_Speed = new float[7];

    public GameObject[] Layer_Objects = new GameObject[7];

    private float[] _initialXPositions;
    private float _spriteWidth;
    private float _scaleX;
    private float _totalOffset;

    public float BackgroundAutoScrollSpeed = 0.5f;

    private void Start()
    {
        _initialXPositions = new float[Layer_Objects.Length];
        _scaleX = Layer_Objects[0].transform.localScale.x;
        _spriteWidth = Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        for (int i = 0; i < Layer_Objects.Length; i++)
        {
            if (Layer_Objects[i] != null)
                _initialXPositions[i] = Layer_Objects[i].transform.position.x;
        }
    }

    private void Update()
    {
        if (!Parallax_Enabled) return;

        for (int i = 0; i < Layer_Objects.Length; i++)
        {
            if (Layer_Objects[i] == null) continue;

            float newX = Layer_Objects[i].transform.position.x -
                         Layer_Speed[i] * Time.deltaTime * BackgroundAutoScrollSpeed;
            Layer_Objects[i].transform.position = new Vector2(newX, Layer_Objects[i].transform.position.y);

            if (newX < -(_spriteWidth * _scaleX))
            {
                newX += _spriteWidth * _scaleX * 2; // Телепортируем вправо
                Layer_Objects[i].transform.position = new Vector2(newX, Layer_Objects[i].transform.position.y);
            }
            else if (newX > _spriteWidth * _scaleX)
            {
                newX -= _spriteWidth * _scaleX * 2; // Телепортируем влево
                Layer_Objects[i].transform.position = new Vector2(newX, Layer_Objects[i].transform.position.y);
            }
        }
    }

    public void Stop()
    {
        Parallax_Enabled = false;
    }
    
    public void Play()
    {
        Parallax_Enabled = true;
    }
}