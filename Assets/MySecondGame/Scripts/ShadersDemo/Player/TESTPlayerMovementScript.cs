using UnityEngine;
using UnityEngine.SceneManagement;

public class TESTPlayerMovementScript : MonoBehaviour// TESTING FILE
{
    [SerializeField] private float _speed = 0.2f;// TESTING FILE 

    [SerializeField] private Joystick _test;// TESTING FILE 

    [SerializeField] private float _positiveXBorderOfRoad; // сделать потом вычисляемую границу

    // TESTING FILE

    private Rigidbody _rigidbody; // TESTING FILE

    // TESTING FILE

    private void Start() // TESTING FILE
    {
        this.GetComponent<BucketRayCastScript>().OnGameLevelInitialize();// TESTING FILE
        // TESTING FILE
        _rigidbody = this.gameObject.GetComponent<Rigidbody>(); // TESTING FILE
        // TESTING FILE
        // TESTING FILE
    }

    private void FixedUpdate() // TESTING FILE
    {
        // TESTING FILE
        if (transform.position.x < _positiveXBorderOfRoad && transform.position.x > -_positiveXBorderOfRoad)
        {
            Movement(true); // TESTING FILE
        }
        else
        {
            Movement(false);
        }
        // TESTING FILE
        //if (this.transform.position.z >= 100)// TESTING FILE
        //    this.transform.position = new Vector3(0,0.45f,0);// TESTING FILE

    }

    private void Movement(bool inBounce) // TESTING FILE
    {
        // TESTING FILE
        if (inBounce) // TESTING FILE
        {
            _rigidbody.MovePosition(_rigidbody.position + ((Vector3.forward + new Vector3(_test.Horizontal, 0, 0)).normalized * _speed)); // /.normalized чтобы по диоганали скорость быстрее не получалась
        }
        else if((_test.Horizontal > 0 && transform.position.x < 0) || (_test.Horizontal < 0 && transform.position.x > 0))
        {
            _rigidbody.MovePosition(_rigidbody.position + ((Vector3.forward + new Vector3(_test.Horizontal, 0, 0)).normalized * _speed));
        }
        else
        {
            _rigidbody.MovePosition(_rigidbody.position + (Vector3.forward * _speed));
        }
    }

    public void ResetPosition()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
