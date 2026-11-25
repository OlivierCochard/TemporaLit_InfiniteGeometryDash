using UnityEngine;

public class ObstacleDistanceSize : MonoBehaviour
{
    int modifSize;
    float sizeMultiplicator;
    Transform playerTransform;

    public void Initialize(Transform playerTransform, float sizeMultiplicator, int modifSize){
        this.playerTransform = playerTransform;
        this.sizeMultiplicator = sizeMultiplicator;
        this.modifSize = modifSize;
    }

    void Update(){
        //Obstacle reached
        if (playerTransform.position.x >= transform.position.x){
            transform.localScale = Vector3.one;
            Destroy(this);
            return;
        }

        float distanceX = Mathf.Abs(transform.position.x - playerTransform.position.x);
        distanceX = Mathf.Max(0, distanceX - 1f);

        float factor = 1f + distanceX * sizeMultiplicator;

        if (modifSize == -1)
            factor = 1f / factor;

        Vector3 finalSize = Vector3.one * factor;
        finalSize.z = 1;
        transform.localScale = finalSize;
    }
}