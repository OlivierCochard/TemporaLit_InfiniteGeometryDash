using UnityEngine;

public class HorizontalScroll : MonoBehaviour
{
    float speed;
    Material mat;
    float offset;

    public bool hasStarterSpeed;
    public float starterSpeed;

    void Awake(){
        mat = GetComponent<SpriteRenderer>().material;
        mat.SetFloat("_Offset", offset);

        if (hasStarterSpeed) SetSpeed(starterSpeed);
    }

    void Update(){
        offset += Time.deltaTime * speed;
        mat.SetFloat("_Offset", offset);
    }

    public void SetSpeed(float newSpeed){
        speed = newSpeed;
    }
}