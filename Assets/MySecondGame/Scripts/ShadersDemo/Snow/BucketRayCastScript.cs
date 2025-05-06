using UnityEngine;

public class BucketRayCastScript : MonoBehaviour
{
    [SerializeField] private GameObject _bucketRayCastPoint;
    [SerializeField] private GameObject _bucket;
    private RaycastHit _hitInfo;
    [SerializeField] private LayerMask _snowLayer;

    private GameObject cords;

    [SerializeField] private CustomRenderTexture _snowHeightRenderTexture;
    private Material _renderTextureMaterial;
    private Vector2 _uvCoords;

    private bool _isInTrigger;

    private int _chunksCounter = 0; // test

    internal void OnGameLevelInitialize()
    {
        _snowHeightRenderTexture.Initialize();
        _renderTextureMaterial = _snowHeightRenderTexture.material;
        _renderTextureMaterial.SetVector("_DrawPosition", -Vector4.one); 
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("other " + other.gameObject.name + " layer " + other.gameObject.layer);
        Debug.Log(_snowLayer.value);
        if (1 << other.gameObject.layer != _snowLayer.value)
            return;

        //TEST FOR 4 MODE delete after testing
        if(TestChunksGenerator.Instance.CurrentModeIndex == 3)
        {
            if(_chunksCounter == 4)
                _chunksCounter = 0;
        }
        //TEST FOR 4 MODE

        //test start
        _snowHeightRenderTexture = TestChunksGenerator.Instance.SnowHeightRenderTexturesForChunks[_chunksCounter];
        _renderTextureMaterial = _snowHeightRenderTexture.material;
        _chunksCounter++;
        //test end

        _isInTrigger = Physics.Raycast(_bucketRayCastPoint.transform.position, Vector3.down, out _hitInfo, 10f, _snowLayer);

        cords = other.transform.GetChild(0).gameObject; 

        cords.transform.position = _hitInfo.point;

        _uvCoords = new Vector2(cords.transform.localPosition.x, cords.transform.localPosition.z);

        _uvCoords.x = (_uvCoords.x + 1) / 2;
        _uvCoords.y = (_uvCoords.y + 1) / 2;

        Debug.Log("Передаваемые UV" + _uvCoords);
        _renderTextureMaterial.SetVector("_DrawPosition", _uvCoords);

    }

    private void OnTriggerStay(Collider collision)
    {
        if (1 << collision.gameObject.layer != _snowLayer.value)
            return;

        _isInTrigger = Physics.Raycast(_bucketRayCastPoint.transform.position, Vector3.down, out _hitInfo, 10f, _snowLayer); 

        if (_isInTrigger)
        {
            cords.transform.position = _hitInfo.point;

            _uvCoords = new Vector2(cords.transform.localPosition.x, cords.transform.localPosition.z);

            _uvCoords.x = (_uvCoords.x + 1) / 2;
            _uvCoords.y = (_uvCoords.y + 1) / 2;

          //  Debug.Log("Передаваемые UV" + _uvCoords);
            _renderTextureMaterial.SetVector("_DrawPosition", _uvCoords);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (_isInTrigger)
        {
            Gizmos.DrawRay(_bucketRayCastPoint.transform.position, Vector3.down * _hitInfo.distance);
            Gizmos.DrawWireCube(_bucketRayCastPoint.transform.position + Vector3.down * _hitInfo.distance, _bucket.transform.localScale / 2);
        }
        else
        {
            Gizmos.DrawRay(_bucketRayCastPoint.transform.position, Vector3.down * 10);
            Gizmos.DrawWireCube(_bucketRayCastPoint.transform.position + Vector3.down * 10, _bucket.transform.localScale / 2);
        }
    }
}
