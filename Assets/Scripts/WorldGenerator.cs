using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Chunks Generation")]
    public int modifAmountPercentage;    
    public Transform obstaclesParent;
    public int chunkAmountMaxInRow;
    public float obstacleDistance;
    public float heightGeneration;
    public float chunkDistance;
    float currentChunkDistance;
    int currentObstacleIndex;
    int chunkLeftAmount;

    [Header("Chunks Memory")]
    public int chunkListSize = 3;
    GameObject[] chunkList;

    [Header("Generic Chunks")]
    public GameObject[] obstacles;
    public GameObject chunkTrigger;
    public Transform chunksParent;
    GameObject[] genericChunks;
    int obstacleAmount;

    [Header("Speed/Ground Setup")]
    public HorizontalScroll shaderFloor;
    public float normalSpeed_ground;
    public float worldUnitsPerUV;

    [Header("Speed SFX")]
    public float speedMultiplicator;
    public HorizontalScroll shaderBg;
    public float normalSpeed_bg;
    public HorizontalScroll shaderMg;
    public float normalSpeed_mg;

    [Header("Size SFX")]
    public Transform playerTransform;
    public float modifSizePercentage;
    public float sizeMultiplicatorSfx;


    public void Awake(){
        //INITIALIZE SPEED
        shaderBg.SetSpeed(normalSpeed_bg);
        shaderMg.SetSpeed(normalSpeed_mg);
        shaderFloor.SetSpeed(normalSpeed_ground);

        //APPLY FLOOR SPEED
        Rigidbody2D rb = obstaclesParent.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector3(-normalSpeed_ground * worldUnitsPerUV, 0);
    }

    public void Start(){
        chunkList = new GameObject[chunkListSize];
        obstacleAmount = obstacles.Length;

        //GENERIC CHUNKS
        genericChunks = new GameObject[obstacleAmount];
        for (int i = 0; i < obstacleAmount; i++){
            GameObject parent = new GameObject("GENERIC CHUNK " + (i+1));
            parent.transform.SetParent(chunksParent);

            for (int x = -1; x < 2; x++){
                GameObject obj = Instantiate(obstacles[i]);
                obj.transform.SetParent(parent.transform);
                obj.transform.localPosition = new Vector3(x * obstacleDistance, obstacles[i].transform.position.y);
                
                //GENERATE TRIGGER
                if (x != 1) continue;
                GameObject chunkTriggerObj = Instantiate(chunkTrigger);
                chunkTriggerObj.transform.SetParent(obj.transform);
                chunkTriggerObj.transform.localPosition = new Vector3(0, 0);

                //ADD PARENT TRIGGERS
                chunkTriggerObj.transform.SetParent(parent.transform);
            }

            genericChunks[i] = parent;
            parent.SetActive(false);
        }

        //GENERATE 2 CHUNKS
        currentObstacleIndex = -1;
        NewChunkIndex();
        GenerateChunk();
        GenerateChunk();

        //RESET BG/MG SPEED
        shaderBg.SetSpeed(normalSpeed_bg);
        shaderMg.SetSpeed(normalSpeed_mg);
    }

    void NewChunkIndex(){
        chunkLeftAmount = chunkAmountMaxInRow;

        //A SECURITY TO HAVE A DIFFERENT GENERIC CHUNK NEXT TO THIS ONE
        int rdm = Random.Range(0, obstacleAmount);
        while (rdm == currentObstacleIndex)
            rdm = Random.Range(0, obstacleAmount);

        currentObstacleIndex = rdm;
    }

    public void GenerateChunk(){
        if (chunkLeftAmount <= 0){
            NewChunkIndex();
        }

        //GENERATE RANDOM GENERIC CHUNK
        GameObject obj = Instantiate(genericChunks[currentObstacleIndex]);
        obj.transform.SetParent(obstaclesParent);
        obj.name = "CHUNK " + (currentObstacleIndex);
        obj.SetActive(true);

        //CHOSE RANDOM MODIF SIZE = -1 / 0 / 1
        int modifSize = 0;
        if (Random.Range(1, 101) <= modifSizePercentage){
            modifSize = Random.Range(0, 2);
            if (modifSize == 0) modifSize = -1;
        }

        //CHOSE RANDOM MODIF SPEED
        shaderBg.SetSpeed(normalSpeed_bg + Random.Range(-normalSpeed_bg*speedMultiplicator, normalSpeed_bg*speedMultiplicator));
        shaderMg.SetSpeed(normalSpeed_mg + Random.Range(-normalSpeed_mg*speedMultiplicator, normalSpeed_mg*speedMultiplicator)); 

        //CHECK ALL OBSTACLES IN THE CHUNK (modif : obstacle object and size)
        for (int i = 0; i < 3; i++){
            GameObject child = obj.transform.GetChild(i).gameObject;

            if (Random.Range(1, 101) <= modifAmountPercentage){
                Vector3 childPos = child.transform.position;
                Destroy(child);

                int randomObstacleIndex = GetRandomIndexButNot(currentObstacleIndex);
                GameObject newChild = Instantiate(obstacles[randomObstacleIndex]);
                newChild.transform.SetParent(obj.transform);
                newChild.transform.localPosition = new Vector3(child.transform.position.x, obstacles[randomObstacleIndex].transform.position.y);

                child = newChild;
            }

            if (modifSize != 0){
                ObstacleDistanceSize script = child.AddComponent<ObstacleDistanceSize>();
                script.Initialize(playerTransform, sizeMultiplicatorSfx, modifSize);
            }
        }

        //SETUP THE CHUNK
        currentChunkDistance += chunkDistance;
        obj.transform.localPosition = new Vector3(currentChunkDistance, heightGeneration);
        chunkLeftAmount--;

        //DESTROY OLD CHUNK AND ADD NEW ONE
        if (chunkList[chunkList.Length-1] != null){
            Destroy(chunkList[chunkList.Length-1]);
            chunkList[chunkList.Length-1] = null;
        }
        for (int i = chunkList.Length-2; i >= 0; i--){
            chunkList[i+1] = chunkList[i];
        }
        chunkList[0] = obj;
    }

    int GetRandomIndexButNot(int index){
        int res = -1;
        while (res == -1 || res == index){
            res = Random.Range(0, obstacleAmount);
        }
        return res;
    }
}
