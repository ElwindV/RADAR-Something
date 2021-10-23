using Audio;
using Items;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class Player : MonoBehaviour
    {
        [Range(1, 20)] public float speed = 5f;
        [Range(1, 20)] public float turnSpeed = 5f;

        [SerializeField]
        public float maxHitPoints;
        public float hitPoints;
        private float _shownHitPoints;
        public Image healthBar;
        public GameObject[] bulletPrefabs;

        // private Quaternion _currentRotation;
        // private Quaternion _targetRotation;
        private Rigidbody _rigidbody;

        private GameManager _gameManager;
        private Camera _camera;
        private Body _body;
    
        public void Start()
        {
            _camera = Camera.main;
            _shownHitPoints = hitPoints = maxHitPoints;
            _rigidbody = GetComponent<Rigidbody>();
            _body = GetComponent<Body>();
        
            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        public void Update ()
        {
            _shownHitPoints = Mathf.Lerp(_shownHitPoints, hitPoints, 10f * Time.deltaTime);
            if(healthBar != null) healthBar.fillAmount = Mathf.Clamp(_shownHitPoints, 0, maxHitPoints) / maxHitPoints;
        }

        public void FixedUpdate()
        {
            if (Input.GetKey("escape"))
                GameManager.ExitGame();

            _rigidbody.velocity = new Vector3(Input.GetAxisRaw("LeftJoyX"), 0, Input.GetAxisRaw("LeftJoyY")) * (speed * 50f * Time.fixedDeltaTime);

            if (Input.GetButton("Fire1") || Input.GetMouseButton(0))
            {
                _body.LeftArmAction();
            }

            if (Input.GetButton("Fire2") || Input.GetMouseButton(1))
            {
                _body.RightArmAction();
            }

            HandleRotation(new Vector2(Input.GetAxisRaw("RightJoyX"), Input.GetAxisRaw("RightJoyY")));
        }

        private void HandleRotation(Vector2 input)
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity)) return;
        
            var targetDirection = hit.point;
        
            var position = transform.position;
            targetDirection.y = position.y;
            targetDirection -= position;

            var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * turnSpeed, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        
        public void TakeDamage(float damage)
        {
            hitPoints -= damage;
            if (hitPoints <= 0 && _gameManager != null) {
                _gameManager.EndGame();
            }
        }

        public void RestoreHealth()
        {
            hitPoints = maxHitPoints;
        }
    }
}
