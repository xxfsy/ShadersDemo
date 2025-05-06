using UnityEngine;
using UnityEngine.SceneManagement;

public class TESTPlayerMovementScript : MonoBehaviour
{
    [SerializeField] private float _speed = 0.2f;

    [SerializeField] private Joystick _test;

    [SerializeField] private float _positiveXBorderOfRoad; // сделать потом вычисляемую границу

    private Rigidbody _rigidbody;

    private void Start()
    {
        this.GetComponent<BucketRayCastScript>().OnGameLevelInitialize();

        _rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        if (transform.position.x < _positiveXBorderOfRoad && transform.position.x > -_positiveXBorderOfRoad)
        {
            Movement(true);
        }
        else
        {
            Movement(false);
        }
    }

    private void Movement(bool inBounce)
    {
        if (inBounce)
        {
            _rigidbody.MovePosition(_rigidbody.position + ((Vector3.forward + new Vector3(_test.Horizontal, 0, 0)).normalized * _speed));
        }
        else if ((_test.Horizontal > 0 && transform.position.x < 0) || (_test.Horizontal < 0 && transform.position.x > 0))
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
