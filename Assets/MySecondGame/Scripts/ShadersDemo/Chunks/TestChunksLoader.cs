using UnityEngine;

public class TestChunksLoader : MonoBehaviour
{
    //private int _chunksCounter = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //if (!other.gameObject.CompareTag("Respawn"))
        //    return;

        //Debug.Log("Trigger enter ");

        //if (_previousChunk == null)
        //{
        //    _previousChunk = other.transform.parent.gameObject;
        //}
        //else
        //{
        //    Destroy(_previousChunk);
        //    _previousChunk = other.transform.parent.gameObject;
        //}

        if (other.gameObject.CompareTag("Respawn"))
        {
            //TestChunksGenerator.Instance.DeleteChunk(other);
            TestChunksGenerator.Instance.LoadNewChunk();
            TestChunksGenerator.Instance.DeleteChunk(other);
        }

    }
}
