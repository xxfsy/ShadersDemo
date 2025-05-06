using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestChunksGenerator : MonoBehaviour
{
    public static TestChunksGenerator Instance { get; private set; }

    [SerializeField] private GameObject[] _variantsOfChunks;
    [SerializeField] private GameObject _endChunk;
    private float _chunkLengthZ;
    private Vector3 _chunkLengthVector;

    private GameObject _previousChunk = null;

    [SerializeField] private TextMeshProUGUI _modeText;

    [SerializeField] private int _startAmountOfChunks = 3;
    [SerializeField] private int _amountOfChunksInTotal = 10; // без учета чанка с финишем 
    private int _countOfSpawnedChunks = 0; // с учетом финиша. Так же является номером последнего активного чанка в 3 моде
    private int _deletedChunksCounter = 0;
    Vector3 spawnPoint;

    [SerializeField] private CustomRenderTexture _snowHeightRenderTexture;
    [SerializeField] private Material _materialForHeightRenderTexture;
    private CustomRenderTexture[] _snowHeightRenderTexturesForChunks;
    public CustomRenderTexture[] SnowHeightRenderTexturesForChunks => _snowHeightRenderTexturesForChunks;
    private Material[] _materialsForHeightRenderTexture;

    private static Modes _mode = Modes.First;
    private static int _currentModeIndex = 0;
    public int CurrentModeIndex => _currentModeIndex;
    private Modes[] _allModes = { Modes.First, Modes.Second, Modes.Third, Modes.Fourth };

    private GameObject[] _spawnedChunks;

    private int _currentChunkIndex = 0; // test for 4 mode

    private enum Modes
    {
        First, // генерация всех сразу
        Second, // генерация 3-х, потом создаются новые
        Third, // генерация всех сразу но включать и удалять постепенно
        Fourth // генерация 3-4 чанков двигать и обновлять старые на место новых
    };

    private void Awake()
    {
        this.Initialize();

        _modeText.SetText("Mode " + (_currentModeIndex + 1));

        GenerateMaterialAndRenderTexturesForChunks();
        GenerateChunks();
    }

    private void Initialize()
    {
        Debug.Log("TestChunksGenerator Initialize");
        if (Instance != null)
        {
            throw new UnityException("One instance only!");
        }

        Instance = this;
        Debug.Log("TestChunksGenerator Initialized");

        _chunkLengthZ = _variantsOfChunks[0].transform.localScale.z * 10f; // длина чанка = его z scale * 10
        _chunkLengthVector = new Vector3(0, 0, _chunkLengthZ);

        _snowHeightRenderTexturesForChunks = new CustomRenderTexture[_amountOfChunksInTotal]; // test
        _materialsForHeightRenderTexture = new Material[_amountOfChunksInTotal]; // test
    }

    private void GenerateChunks()
    {
        //TEST start
        spawnPoint = new Vector3(0, 0.01f, _chunkLengthZ / 2);

        switch (_mode)
        {
            case Modes.First:

                for (int i = 0; i < _amountOfChunksInTotal; i++)
                {
                    GameObject newChunk = Instantiate(_variantsOfChunks[0], spawnPoint, Quaternion.identity, this.transform);
                    newChunk.transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_HeightMap", _snowHeightRenderTexturesForChunks[i]);
                    spawnPoint = spawnPoint + _chunkLengthVector;
                }

                Instantiate(_endChunk, spawnPoint, Quaternion.identity, this.transform);
                break;

            case Modes.Second:

                for (int i = 0; i < _startAmountOfChunks; i++, _countOfSpawnedChunks++)
                {
                    GameObject newChunk = Instantiate(_variantsOfChunks[0], spawnPoint, Quaternion.identity, this.transform);
                    newChunk.transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_HeightMap", _snowHeightRenderTexturesForChunks[i]);
                    spawnPoint = spawnPoint + _chunkLengthVector;
                }
                break;

            case Modes.Third:

                _spawnedChunks = new GameObject[_amountOfChunksInTotal + 1]; // + 1 место для чанка с концом 

                for (int i = 0; i < _amountOfChunksInTotal; i++)
                {
                    GameObject newChunk;
                    if (_countOfSpawnedChunks < _startAmountOfChunks)
                    {
                        newChunk = Instantiate(_variantsOfChunks[0], spawnPoint, Quaternion.identity, this.transform);
                        newChunk.transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_HeightMap", _snowHeightRenderTexturesForChunks[i]);
                        spawnPoint = spawnPoint + _chunkLengthVector;
                        _countOfSpawnedChunks++;
                    }
                    else
                    {
                        newChunk = Instantiate(_variantsOfChunks[0], spawnPoint, Quaternion.identity, this.transform);
                        newChunk.SetActive(false);
                        newChunk.transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_HeightMap", _snowHeightRenderTexturesForChunks[i]);
                        spawnPoint = spawnPoint + _chunkLengthVector;
                    }
                    _spawnedChunks[i] = newChunk;
                }

                _spawnedChunks[_spawnedChunks.Length - 1] = Instantiate(_endChunk, spawnPoint, Quaternion.identity, this.transform);
                _spawnedChunks[_spawnedChunks.Length - 1].SetActive(false);
                break;

            case Modes.Fourth:

                for (int i = 0; i < _startAmountOfChunks + 1; i++, _countOfSpawnedChunks++)
                {
                    GameObject newChunk = Instantiate(_variantsOfChunks[0], spawnPoint, Quaternion.identity, this.transform);
                    newChunk.transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_HeightMap", _snowHeightRenderTexturesForChunks[i]);
                    spawnPoint = spawnPoint + _chunkLengthVector;
                }
                _spawnedChunks = new GameObject[1];
                _spawnedChunks[0] = Instantiate(_endChunk, spawnPoint, Quaternion.identity, this.transform);
                _spawnedChunks[0].SetActive(false);

                break;

            default:
                return;
        }
    }

    private void GenerateMaterialAndRenderTexturesForChunks() // test
    {
        for (int i = 0; i < _amountOfChunksInTotal; i++)
        {
            _snowHeightRenderTexturesForChunks[i] = Instantiate(_snowHeightRenderTexture);
            _materialsForHeightRenderTexture[i] = Instantiate(_materialForHeightRenderTexture);

            _snowHeightRenderTexturesForChunks[i].material = _materialsForHeightRenderTexture[i];
            _snowHeightRenderTexturesForChunks[i].Initialize();
        }
    }

    internal void DeleteChunk(Collider other)
    {
        Debug.Log("DeleteChunk ");

        switch (_mode)
        {
            case Modes.Fourth:

                _previousChunk = other.transform.parent.gameObject;
                break;

            default:

                if (_previousChunk == null)
                {
                    _previousChunk = other.transform.parent.gameObject;
                }
                else
                {
                    Destroy(_previousChunk);
                    _previousChunk = other.transform.parent.gameObject;

                    _snowHeightRenderTexturesForChunks[_deletedChunksCounter].Release();
                    Destroy(_snowHeightRenderTexturesForChunks[_deletedChunksCounter]);
                    Destroy(_materialsForHeightRenderTexture[_deletedChunksCounter]);
                    _deletedChunksCounter++;
                }
                break;
        }
    }

    internal void LoadNewChunk()
    {
        switch (_mode)
        {
            case Modes.Second:

                if (_countOfSpawnedChunks < _amountOfChunksInTotal)
                {
                    GameObject newChunk = Instantiate(_variantsOfChunks[0], spawnPoint, Quaternion.identity, this.transform);
                    newChunk.transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_HeightMap", _snowHeightRenderTexturesForChunks[_countOfSpawnedChunks]);
                    spawnPoint = spawnPoint + _chunkLengthVector;
                    _countOfSpawnedChunks++;
                }
                else if (_countOfSpawnedChunks == _amountOfChunksInTotal)
                {
                    Instantiate(_endChunk, spawnPoint, Quaternion.identity, this.transform);
                    _countOfSpawnedChunks++;
                }
                break;

            case Modes.Third:

                if (_countOfSpawnedChunks <= _amountOfChunksInTotal)
                {
                    _spawnedChunks[_countOfSpawnedChunks].SetActive(true);
                    _countOfSpawnedChunks++;
                }
                break;

            case Modes.Fourth:

                if (_previousChunk != null)
                {
                    if (_countOfSpawnedChunks < _amountOfChunksInTotal)
                    {
                        if (_currentChunkIndex == 4)
                            _currentChunkIndex = 0;

                        //_snowHeightRenderTexturesForChunks[_currentChunkIndex].Release();
                        _snowHeightRenderTexturesForChunks[_currentChunkIndex].Initialize();
                        _previousChunk.transform.position = spawnPoint;
                        spawnPoint = spawnPoint + _chunkLengthVector;
                        _countOfSpawnedChunks++;
                        _currentChunkIndex++;
                    }
                    else if (_countOfSpawnedChunks == _amountOfChunksInTotal)
                    {
                        _spawnedChunks[0].transform.position = spawnPoint;
                        _spawnedChunks[0].SetActive(true);
                        _countOfSpawnedChunks++;
                    }
                }
                break;

            default:
                Debug.Log("DEFAULT");
                return;
        }
    }

    public void MethodForButtonToChangeMode()
    {
        if (_currentModeIndex < _allModes.Length - 1)
        {
            _currentModeIndex++;
        }
        else
        {
            _currentModeIndex = 0;
        }
        _mode = _allModes[_currentModeIndex];
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
