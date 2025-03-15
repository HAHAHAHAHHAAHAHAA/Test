using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Player : MonoBehaviour
{
    [SerializeField] Animator animator;
    private string weaponAnimationName;
    private string idleweaponAnimationName;
    private string reloadAnimationName;

    [SerializeField] private GameObject shootButton;
    public Vector3 playerPos;
    [SerializeField] private int health=25;
    [SerializeField] private TextMeshProUGUI health_UI;
    [SerializeField] private Transform lineEnd;
    private bool aiming = false;
    private bool rotate;
    private LineRenderer lineRenderer;
    private bool TakingTimeToNextShot;
    private bool reloading = false;
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private ParticleSystem bloodParticles;
    [SerializeField] private Transform gunholder;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FloatingJoystick joystickMove;
    [SerializeField] private GameObject joystickMove_UIButton;
    [SerializeField] private GameObject joystickAim_UIButton;
    [SerializeField] private FloatingJoystick joystickAim;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private TextMeshProUGUI ammo_UI;
    
    public int money;
    public int metal;
    public int cloth;
 
    private bool isShooting;
    public string gun = "pistol";

    [SerializeField] private GameObject pistol;
    public int pistolAmmo;
    [SerializeField] private int currentPistolAmmo = 8;
    [SerializeField] private int pistolMaxAmmo;
    [SerializeField] private int pistolDamage;
    [SerializeField] private float pistolfireRate;
    [SerializeField] private float pistolReloadSpeed;

    [SerializeField] private GameObject shotgun;
    public int shotgunAmmo;
    [SerializeField] private int currentShotgunAmmo;
    [SerializeField] private int shotgunMaxAmmo;
    [SerializeField] private int shotgunDamage;
    [SerializeField] private float shotgunFireRate;
    [SerializeField] private float shotgunReloadSpeed;

    [SerializeField] private GameObject submachinegun;
    public int submachineAmmo;
    [SerializeField] private int currentSubmachinegunAmmo;
    [SerializeField] private int submachinegunMaxAmmo;
    [SerializeField] private int submachinegunDamage;
    [SerializeField] private float submachinegunFireRate;
    [SerializeField] private float submachinegunReloadSpeed;

    [SerializeField] private GameObject rifle;
    public int rifleAmmo;
    [SerializeField] private int currentRifleAmmo;
    [SerializeField] private int rifleMaxAmmo;
    [SerializeField] private int rifleDamage;
    [SerializeField] private float rifleFireRate;
    [SerializeField] private float rifleReloadSpeed;


    [SerializeField] private RectTransform background;
    [SerializeField] private Joystick joystick;


    [SerializeField] private AudioSource reloadSound;
    [SerializeField] private AudioSource pistolSound;
    [SerializeField] private AudioSource shotgunSound;
    [SerializeField] private AudioSource submachinegunSound;
    [SerializeField] private AudioSource rifleSound;

    private void ResetJoystickInput()
    {
        joystick.SetInputZero();
    }
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
    }

    private void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPointerDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnPointerUp();
        }



        switch (gun)
        {
            case "pistol":
                ammo_UI.text = currentPistolAmmo.ToString() + "/" + pistolAmmo;
                break;
            case "shotgun":
                ammo_UI.text = currentShotgunAmmo.ToString() + "/" + shotgunAmmo;
                break;
            case "submachinegun":
                ammo_UI.text = currentSubmachinegunAmmo.ToString() + "/" + submachineAmmo;
                break;
            case "rifle":
                ammo_UI.text = currentRifleAmmo.ToString() + "/" + rifleAmmo;
                break;
        }
        if (isShooting&&shootButton.active ==true)
        {
            Shoot();
        }
        if (shootButton.active == false)
        {
            isShooting = false;
        }
        playerPos = this.gameObject.transform.position;
        health_UI.text=health.ToString()+"/25";
        // �������� ������� ������ �� ����������
        float horizontalInput = joystickMove.Horizontal;
        float verticalInput = joystickMove.Vertical;
        float horizontalRotationInput = joystickAim.Horizontal;
        float verticalRotationInput = joystickAim.Vertical;

        // ���������� ����������� ��������
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // ���������� ����������� ��������
        Vector3 rotationDirection = new Vector3(horizontalRotationInput, 0f, verticalRotationInput).normalized;

        // Прицел
        if (rotationDirection == Vector3.zero)
        {
            aiming=false;
            animator.SetBool("Aiming", false);
            joystickMove_UIButton.SetActive(true);
            if (movementDirection != Vector3.zero)
            {
                background.gameObject.SetActive(true);
            }
            lineRenderer.positionCount = 0;
            rotate = false;
            shootButton.SetActive(false);
        }
        if (rotationDirection != Vector3.zero)
        {
            aiming = true;
            animator.SetBool("Aiming", true);
            background.gameObject.SetActive(false);
            joystickMove_UIButton.SetActive(false);
            ResetJoystickInput();
            rotate = true;
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // ������ ����� �� gunholder � ����������� �������
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.03f;
            lineRenderer.endWidth = 0.03f;
            lineRenderer.SetPosition(0, gunholder.position);
            lineRenderer.SetPosition(1, lineEnd.position);
            shootButton.SetActive(true);
            animator.SetBool("Run", false);

        }
        // Движение
        if (movementDirection != Vector3.zero && aiming == false)
        {
            animator.SetBool("Run", true);
            if (rotate == false)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            rb.MovePosition(transform.position + movementDirection * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }
    public void SwitchGun(string gunType)
    {
        if (reloading == false)
        {
            gun = gunType;
            switch (gun)
            {
                case "pistol":
                    pistol.SetActive(true);
                    submachinegun.SetActive(false);
                    rifle.SetActive(false);
                    shotgun.SetActive(false);
                    weaponAnimationName = "Pistol";
                    reloadAnimationName = "ReloadPistol";
                    idleweaponAnimationName = "Pistol 0";
                    animator.SetInteger("Weapon", 1);
                    break;
                case "shotgun":
                    shotgun.SetActive(true);
                    pistol.SetActive(false);
                    rifle.SetActive(false);
                    submachinegun.SetActive(false);
                    weaponAnimationName = "Shotgun";
                    reloadAnimationName = "ReloadShotgun";
                    idleweaponAnimationName = "Shotgun 0";
                    animator.SetInteger("Weapon", 2);
                    break;
                case "submachinegun":
                    submachinegun.SetActive(true);
                    pistol.SetActive(false);
                    rifle.SetActive(false);
                    shotgun.SetActive(false);
                    weaponAnimationName = "Submachine";
                    reloadAnimationName = "ReloadSubmachine";
                    idleweaponAnimationName = "Submachine 0";
                    animator.SetInteger("Weapon", 3);
                    break;
                case "rifle":
                    rifle.SetActive(true);
                    pistol.SetActive(false);
                    submachinegun.SetActive(false);
                    shotgun.SetActive(false);
                    weaponAnimationName = "Rifle";
                    reloadAnimationName = "ReloadRifle";
                    idleweaponAnimationName = "Rifle 0";
                    animator.SetInteger("Weapon", 4);
                    break;
            }
        }
    }
    public void Reload()
    {
        StartCoroutine(Reloading(gun));
    }
    public void OnPointerDown()
    {
        isShooting = true; // Начинаем стрельбу
    }
    public void OnPointerUp()
    {
        isShooting = false; // Останавливаем стрельбу
    }
    private void Shoot()
    {
        switch (gun)
        {
            case "pistol":
                RaycastHit hit;
                //ammo_UI.text = currentPistolAmmo.ToString() + "/" + pistolAmmo;
                if (currentPistolAmmo >= 1 && !TakingTimeToNextShot)
                {
                    PlayAnimation(weaponAnimationName);
                    pistolSound.pitch = Random.Range(.9f, 1);
                    pistolSound.Play();
                    // Выполняем Raycast только для получения информации о попадании
                    bool hitDetected = Physics.Raycast(gunholder.position, gunholder.forward, out hit);
                    // Если попали в врага, вызываем ShootCor с параметром true
                    if (hitDetected && hit.collider.CompareTag("Enemy"))
                    {
                        StartCoroutine(ShootCor(hit.point, hit.collider, true, pistolfireRate, pistolDamage)) ;
                    }
                    else if (hitDetected && hit.collider.CompareTag("Object"))
                    {
                        MoveByHit movebyhit = hit.collider.GetComponent<MoveByHit>();

                        movebyhit.PushObject((hit.point - gunholder.position).normalized);
                    }
                    else
                    {
                        // Если не попали в врага, просто вызываем ShootCor с параметром false
                        StartCoroutine(ShootCor(hitDetected ? hit.point : gunholder.position + gunholder.forward * 1000, hit.collider, false, pistolfireRate, pistolDamage));
                    }
                    // Уменьшаем количество патронов
                    currentPistolAmmo--;
                    // Проверяем, нужно ли перезаряжаться
                    
                }
                if (currentPistolAmmo <= 0 && !reloading && pistolAmmo != 0)
                {
                    StartCoroutine(Reloading("pistol"));
                }
                break;
            case "shotgun":
                RaycastHit hit4;
                //ammo_UI.text = currentShotgunAmmo.ToString() + "/" + shotgunAmmo;

                if (currentShotgunAmmo >= 1 && !TakingTimeToNextShot)
                {
                    PlayAnimation(weaponAnimationName);
                    shotgunSound.pitch = Random.Range(.9f, 1);
                    shotgunSound.Play();
                    // Определяем количество лучей и их угол
                    int numberOfRays = 8;
                    float angleStep = 3f; // Угол между лучами (180 / 8 = 22.5, но можно настроить)
                    Vector3 direction = gunholder.forward;

                    for (int i = 0; i < numberOfRays; i++)
                    {
                        // Вычисляем угол для текущего луча
                        float angle = -angleStep * (numberOfRays / 2) + angleStep * i;
                        Quaternion rotation = Quaternion.Euler(0, angle, 0);
                        Vector3 rayDirection = rotation * direction;

                        // Выполняем Raycast
                        bool hitDetected = Physics.Raycast(gunholder.position, rayDirection, out hit4);
                        Color rayColor = hitDetected ? Color.red : Color.green; // Красный, если попали, зеленый, если нет
                        Debug.DrawRay(gunholder.position, rayDirection * 100, rayColor, 2f);
                        // Если попали в врага, вызываем ShootCor с параметром true
                        if (hitDetected && hit4.collider.CompareTag("Enemy"))
                        {
                            StartCoroutine(ShootCor(hit4.point, hit4.collider, true, shotgunFireRate, shotgunDamage));
                        }
                        else if (hitDetected && hit4.collider.CompareTag("Object"))
                        {
                            MoveByHit movebyhit = hit4.collider.GetComponent<MoveByHit>();

                            movebyhit.PushObject((hit4.point - gunholder.position).normalized);
                        }
                        else
                        {
                            // Если не попали в врага, просто вызываем ShootCor с параметром false
                            StartCoroutine(ShootCor(hitDetected ? hit4.point : gunholder.position + rayDirection * 1000, hit4.collider, false, shotgunFireRate, shotgunDamage));

                        }
                    }

                    // Уменьшаем количество патронов
                    currentShotgunAmmo--;

                    // Проверяем, нужно ли перезаряжаться
                    if (currentShotgunAmmo <= 0 && !reloading && shotgunAmmo != 0)
                    {
                        StartCoroutine(Reloading("shotgun"));
                    }
                }

                break;
            case "submachinegun":
                RaycastHit hit2;
                //ammo_UI.text = currentSubmachinegunAmmo.ToString() + "/" + submachineAmmo;
                if (currentSubmachinegunAmmo >= 1 && !TakingTimeToNextShot)
                {
                    PlayAnimation(weaponAnimationName);
                    submachinegunSound.pitch = Random.Range(.9f, 1);
                    submachinegunSound.Play();
                    // Выполняем Raycast только для получения информации о попадании
                    bool hitDetected = Physics.Raycast(gunholder.position, gunholder.forward, out hit2);
                    // Если попали в врага, вызываем ShootCor с параметром true
                    if (hitDetected && hit2.collider.CompareTag("Enemy"))
                    {
                        StartCoroutine(ShootCor(hit2.point, hit2.collider, true, submachinegunFireRate, submachinegunDamage));
                    }
                    else if (hitDetected && hit2.collider.CompareTag("Object"))
                    {
                        MoveByHit movebyhit = hit2.collider.GetComponent<MoveByHit>();

                        movebyhit.PushObject((hit2.point - gunholder.position).normalized);
                    }
                    else
                    {
                        // Если не попали в врага, просто вызываем ShootCor с параметром false
                        StartCoroutine(ShootCor(hitDetected ? hit2.point : gunholder.position + gunholder.forward * 1000, hit2.collider, false, submachinegunFireRate, submachinegunDamage));
                    }
                    // Уменьшаем количество патронов
                    currentSubmachinegunAmmo--;
                    // Проверяем, нужно ли перезаряжаться
                    
                }
                if (currentSubmachinegunAmmo <= 0 && !reloading && submachineAmmo != 0)
                {
                    StartCoroutine(Reloading("submachinegun"));
                }
                break;
            case "rifle":
                RaycastHit hit3;
                //ammo_UI.text = currentRifleAmmo.ToString() + "/" + rifleAmmo;
                if (currentRifleAmmo >= 1 && !TakingTimeToNextShot)
                {
                    PlayAnimation(weaponAnimationName);
                    rifleSound.pitch = Random.Range(.9f, 1);
                    rifleSound.Play();
                    // Выполняем Raycast только для получения информации о попадании
                    bool hitDetected = Physics.Raycast(gunholder.position, gunholder.forward, out hit3);
                    // Если попали в врага, вызываем ShootCor с параметром true
                    if (hitDetected && hit3.collider.CompareTag("Enemy"))
                    {
                        StartCoroutine(ShootCor(hit3.point, hit3.collider, true, rifleFireRate, rifleDamage));
                    }
                    else if (hitDetected && hit3.collider.CompareTag("Object"))
                    {
                        MoveByHit movebyhit = hit3.collider.GetComponent<MoveByHit>();

                        movebyhit.PushObject((hit3.point - gunholder.position).normalized);
                    }
                    else
                    {
                        // Если не попали в врага, просто вызываем ShootCor с параметром false
                        StartCoroutine(ShootCor(hitDetected ? hit3.point : gunholder.position + gunholder.forward * 1000, hit3.collider, false, rifleFireRate, rifleDamage));
                    }
                    // Уменьшаем количество патронов
                    currentRifleAmmo--;
                    // Проверяем, нужно ли перезаряжаться
                    
                }
                if (currentRifleAmmo <= 0 && !reloading && rifleAmmo != 0)
                {
                    StartCoroutine(Reloading("rifle"));
                }
                break;
        }
    }
    IEnumerator Reloading(string gunType)
    {
        reloading = true;
        PlayAnimation(reloadAnimationName);
        reloadSound.Play();
        switch (gunType)
        {
            case "pistol":
                yield return new WaitForSeconds(pistolReloadSpeed);
                pistolAmmo = pistolAmmo + currentPistolAmmo;
                currentPistolAmmo = 0;
                if (pistolAmmo >= pistolMaxAmmo)
                {
                    currentPistolAmmo = pistolMaxAmmo;
                    pistolAmmo -= pistolMaxAmmo;
                    //ammo_UI.text = currentPistolAmmo.ToString() + "/" + pistolAmmo;
                }
                else
                {
                    currentPistolAmmo = pistolAmmo;
                    pistolAmmo = 0;
                    //ammo_UI.text = currentPistolAmmo.ToString() + "/" + pistolAmmo;
                }
                break;
            case "shotgun":
                yield return new WaitForSeconds(shotgunReloadSpeed);
                shotgunAmmo = shotgunAmmo + currentShotgunAmmo;
                currentShotgunAmmo = 0;
                if (shotgunAmmo >= shotgunMaxAmmo)
                {
                    currentShotgunAmmo = shotgunMaxAmmo;
                    shotgunAmmo -= shotgunMaxAmmo;
                    //ammo_UI.text = currentShotgunAmmo.ToString() + "/" + shotgunAmmo;
                }
                else
                {
                    currentShotgunAmmo = shotgunAmmo;
                    shotgunAmmo = 0;
                    //ammo_UI.text = currentShotgunAmmo.ToString() + "/" + shotgunAmmo;
                }
                break;
            case "submachinegun":
                yield return new WaitForSeconds(submachinegunReloadSpeed);
                submachineAmmo = submachineAmmo + currentSubmachinegunAmmo;
                currentSubmachinegunAmmo = 0;
                if (submachineAmmo >= submachinegunMaxAmmo)
                {
                    currentSubmachinegunAmmo = submachinegunMaxAmmo;
                    submachineAmmo -= submachinegunMaxAmmo;
                   // ammo_UI.text = currentSubmachinegunAmmo.ToString() + "/" + submachineAmmo;
                }
                else
                {
                    currentSubmachinegunAmmo = submachineAmmo;
                    submachineAmmo = 0;
                    //ammo_UI.text = currentSubmachinegunAmmo.ToString() + "/" + submachineAmmo;
                }
                break;
            case "rifle":
                yield return new WaitForSeconds(rifleReloadSpeed);
                rifleAmmo = rifleAmmo + currentRifleAmmo;
                currentRifleAmmo = 0;
                if (rifleAmmo >= rifleMaxAmmo)
                {
                    currentRifleAmmo = rifleMaxAmmo;
                    rifleAmmo -= rifleMaxAmmo;
                    //ammo_UI.text = currentRifleAmmo.ToString() + "/" + rifleAmmo;
                }
                else
                {
                    currentRifleAmmo = rifleAmmo;
                    rifleAmmo = 0;
                    //ammo_UI.text = currentRifleAmmo.ToString() + "/" + rifleAmmo;
                }
                break;
        }
        ;
        reloading = false;
    }
    IEnumerator ShootCor(Vector3 hitPoint, Collider hitCollider, bool isEnemyWasHit, float timeToShoot, int damage)
    {
        TakingTimeToNextShot = true;

        if (isEnemyWasHit)
        {
            hitCollider.GetComponent<Enemy>().Damage(damage);

            // Создание копии частиц крови
            ParticleSystem bloodEffect = Instantiate(bloodParticles, hitPoint, Quaternion.identity);
            bloodEffect.Play();

            // Удаление копии частиц через время
            Destroy(bloodEffect.gameObject, bloodEffect.main.duration);
        }
        else
        {
            if (hitCollider.CompareTag("Action"))
            {
                hitCollider.gameObject.GetComponent<ActionScript>().ActionMethod();
            }
            // Создание копии взрывных частиц
            ParticleSystem explosionEffect = Instantiate(explosionParticles, hitPoint, Quaternion.identity);
            explosionEffect.Play();

            // Удаление копии частиц через время
            Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
        }

        yield return new WaitForSeconds(timeToShoot);
        TakingTimeToNextShot = false;
    }



    // ----------УПРАВЛЕНИЕ АНИМАЦИЯМИ----------//НАЧАЛО

    public void PlayAnimation(string animationName)
    {
        animator.speed = 1f;
        animator.Play(animationName, 0, 0.15f);
    }
    private void StopAnimationAtStart(string animationName) // ПОКА НЕ НУЖНО 
    {
        animator.Play(animationName, 0, 0f); // Воспроизводит анимацию с самого начала 
    }

    // ----------УПРАВЛЕНИЕ АНИМАЦИЯМИ----------//КОНЕЦ

    public void Damage(int dmg)
    {
        health -= dmg;
    }
}
