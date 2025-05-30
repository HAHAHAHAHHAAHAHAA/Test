using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class Player : MonoBehaviour
{
    [Header("Vignette")]
    public Volume globalVolume;
    private Vignette vignette;

    [SerializeField] Animator animator;
    private string weaponAnimationName;
    private string idleweaponAnimationName;
    private string reloadAnimationName;
    
    [SerializeField] private GameObject shootButton;
    public Vector3 playerPos;
    [SerializeField] private int maxHealth=25;
    [SerializeField] private int health=25;
    [SerializeField] private Image healthbar;
    [SerializeField] private Image healthbarPast;
    [SerializeField] bool dead = false;
    [SerializeField] private TextMeshProUGUI health_UI;
    [SerializeField] private Transform lineEnd;
    private bool aiming = false;
    private bool rotate;
    private LineRenderer lineRenderer;
    private bool TakingTimeToNextShot;
    private bool reloading = false;
    private bool running=false;
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private ParticleSystem bloodParticles;
    [SerializeField] private Transform gunholder;
    [SerializeField] private Transform gunholderP, gunholderS, gunholderSM, gunholderR;
    [SerializeField] private Transform gunholderRaycast;
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

    [SerializeField] private GameObject shootTracer;
    private bool isShooting;
    public string gun = "pistol";
    [SerializeField] private Image gunsMenuImage;
    [SerializeField] private GameObject gunsMenu;
    [SerializeField] private GameObject gunsMenuButton;

    [SerializeField] private GameObject pistol;
    public int pistolAmmo;
    [SerializeField] private int currentPistolAmmo = 8;
    [SerializeField] private int pistolMaxAmmo;
    [SerializeField] private int pistolDamage;
    [SerializeField] private float pistolfireRate;
    [SerializeField] private float pistolReloadSpeed;
    [SerializeField] private ParticleSystem pistolParticle;
    [SerializeField] private Image pistolImage;
    [SerializeField] private Restartpanel _showDiePanel;

    [SerializeField] private GameObject shotgun;
    public int shotgunAmmo;
    [SerializeField] private int currentShotgunAmmo;
    [SerializeField] private int shotgunMaxAmmo;
    [SerializeField] private int shotgunDamage;
    [SerializeField] private float shotgunFireRate;
    [SerializeField] private float shotgunReloadSpeed;
    [SerializeField] private ParticleSystem shotgunParticle;
    [SerializeField] private Image shotgunImage;

    [SerializeField] private GameObject submachinegun;
    public int submachineAmmo;
    [SerializeField] private int currentSubmachinegunAmmo;
    [SerializeField] private int submachinegunMaxAmmo;
    [SerializeField] private int submachinegunDamage;
    [SerializeField] private float submachinegunFireRate;
    [SerializeField] private float submachinegunReloadSpeed;
    [SerializeField] private ParticleSystem submachinegunParticle;
    [SerializeField] private Image submachinegunImage;

    [SerializeField] private GameObject rifle;
    public int rifleAmmo;
    [SerializeField] private int currentRifleAmmo;
    [SerializeField] private int rifleMaxAmmo;
    [SerializeField] private int rifleDamage;
    [SerializeField] private float rifleFireRate;
    [SerializeField] private float rifleReloadSpeed;
    [SerializeField] private ParticleSystem rifleParticle;
    [SerializeField] private Image rifleImage;


    [SerializeField] private RectTransform background;
    [SerializeField] private Joystick joystick;


    [SerializeField] private AudioSource reloadSound;
    [SerializeField] private AudioSource pistolSound;
    [SerializeField] private AudioSource shotgunSound;
    [SerializeField] private AudioSource submachinegunSound;
    [SerializeField] private AudioSource rifleSound;
    [SerializeField] private AudioSource WalkSound;
    private AudioSource lowHpSource;
    [SerializeField] float runsoundCD;
    [SerializeField] private GameObject groundChecker;
    [SerializeField] private AudioClip lowHealthSound;
    [SerializeField] AudioClip[] pistolClips;
    [SerializeField] AudioClip[] walkClips;
    [SerializeField] AudioClip[] walkClips2;
    [SerializeField] AudioClip[] walkClips3;
    private AudioClip[] shuffledWalkClips;
    private AudioClip[] shuffledWalkClips2;
    private AudioClip[] shuffledWalkClips3;
    private AudioClip[] shuffledPistolClips;
    private int currentWalkClipIndex = 0;
    private int currentPistolClipIndex = 0;
    private CharacterController characterController;
    private float runsCD;
    public float gravity = 5f;
    [SerializeField] private float _agroRadius;
    [SerializeField] private float _agroTime;
    [SerializeField] private AudioSource weaponSwap;
    private void ResetJoystickInput()
    {
        joystick.SetInputZero();
    }
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        SwitchGun("pistol");
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        _showDiePanel = FindFirstObjectByType<Restartpanel>();
        lowHpSource = gameObject.AddComponent<AudioSource>();
        lowHpSource.playOnAwake = false;
        lowHpSource.loop = true;
        if (globalVolume != null && globalVolume.profile.TryGet(out Vignette volVignette))
        {
            vignette = volVignette;
        }
    }

    private void FixedUpdate()
    {
        characterController.Move(Vector3.up * gravity * Time.fixedDeltaTime * -1);
        if (dead) return;
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
        healthbar.fillAmount=health/25f;
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
            running = false;
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
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.03f;

                switch (gun)
                {
                    case "pistol":
                        lineRenderer.SetPosition(0, gunholderP.position);
                        break;
                    case "shotgun":
                        lineRenderer.SetPosition(0, gunholderS.position);
                        break;
                    case "submachinegun":
                        lineRenderer.SetPosition(0, gunholderSM.position);
                        break;
                    case "rifle":
                        lineRenderer.SetPosition(0, gunholderR.position);
                        break;
                }
           

            // Создаем LayerMask, которая *НЕ* включает слой TransparentFX
            int layerMask = 1 << LayerMask.NameToLayer("Default"); // Создаем маску слоя TransparentFX
            //layerMask = ~layerMask; // Инвертируем маску, чтобы ИГНОРИРОВАТЬ этот слой

            // Используем Raycast с настроенной LayerMask
            RaycastHit hit;
            if (Physics.Raycast(gunholderRaycast.position, gunholder.forward, out hit, Mathf.Infinity, layerMask))
            {
               
                   lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                // Если ничего не найдено, то рисуем луч на какое-то расстояние
               
                    lineRenderer.SetPosition(1, gunholder.position + gunholder.forward * 100f);
            }
            shootButton.SetActive(true);
            animator.SetBool("Run", false);
        }
        // Движение
        if (movementDirection != Vector3.zero && !aiming && !reloading)
        {
            running = true;
            animator.SetBool("Run", true);

            if (rotate == false)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Используем CharacterController.Move()
            characterController.Move(movementDirection * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            running = false;
            animator.SetBool("Run", false);
        }
        if (health <= 0)
        {
            dead = true;
            animator.Play("Dying1");
            _showDiePanel.ShowDiePanel();
        }
        CheckLowHealth();
    }
    public void Steps()
    {
        if (Physics.Raycast(groundChecker.transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            string surfaceTag = hit.collider.tag;

            // Выбираем массив звуков в зависимости от тега
            AudioClip[] clipsToUse = surfaceTag switch
            {
                "GroundType1" => walkClips,
                "GroundType2" => walkClips2,
                "GroundType3" => walkClips3,
                _ => walkClips
            };
            AudioClip[] shuffleUse = surfaceTag switch
            {
                "GroundType1" => shuffledWalkClips,
                "GroundType2" => shuffledWalkClips2,
                "GroundType3" => shuffledWalkClips3,
                _ => shuffledWalkClips
            };

            if (shuffleUse == null || currentWalkClipIndex >= shuffleUse.Length)
            {
                shuffleUse = (AudioClip[])clipsToUse.Clone();

                // Алгоритм Фишера-Йетса для перемешивания
                for (int i = 0; i < shuffleUse.Length; i++)
                {
                    int rnd = Random.Range(i, shuffleUse.Length);
                    AudioClip temp = shuffleUse[i];
                    shuffleUse[i] = shuffleUse[rnd];
                    shuffleUse[rnd] = temp;
                    currentWalkClipIndex = 0;
                }
            }

            WalkSound.pitch = Random.Range(0.95f, 1f);
            WalkSound.clip = shuffleUse[currentWalkClipIndex++];
            WalkSound.Play();
        }
        // Если дошли до конца массива или еще не инициализировали - перемешиваем
        
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
                    gunsMenu.SetActive(false);
                    gunsMenuImage.sprite = pistolImage.sprite;
                    gunsMenuButton.SetActive(true);
                    weaponSwap.Play();
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
                    gunsMenu.SetActive(false);
                    gunsMenuImage.sprite = shotgunImage.sprite;
                    gunsMenuButton.SetActive(true);
                    weaponSwap.Play();
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
                    gunsMenu.SetActive(false);
                    gunsMenuImage.sprite = submachinegunImage.sprite;
                    gunsMenuButton.SetActive(true);
                    weaponSwap.Play();
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
                    gunsMenu.SetActive(false);
                    gunsMenuImage.sprite=rifleImage.sprite;
                    gunsMenuButton.SetActive(true);
                    weaponSwap.Play();
                    break;
            }
        }
    }
    public void GunMenuButtonActivate()
    {
        gunsMenuButton.SetActive(false);
        gunsMenu.SetActive(true);
    }
    public void Reload()
    {
        switch (gun)
        {
            case "pistol":
                if (!running && !reloading&&(currentPistolAmmo!=pistolMaxAmmo&pistolAmmo!=0))
                {
                    StartCoroutine(Reloading(gun));
                }
                break;
            case "shotgun":
                if (!running && !reloading && (currentShotgunAmmo != shotgunMaxAmmo & shotgunAmmo != 0))
                {
                    StartCoroutine(Reloading(gun));
                }
                break;
            case "submachinegun":
                if (!running && !reloading && (currentSubmachinegunAmmo != submachinegunMaxAmmo & submachineAmmo != 0))
                {
                    StartCoroutine(Reloading(gun));
                }
                break;
            case "rifle":
                if (!running && !reloading && (currentRifleAmmo != rifleMaxAmmo & rifleAmmo != 0))
                {
                    StartCoroutine(Reloading(gun));
                }
                break;
        }
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
        int layerMask = 1 << LayerMask.NameToLayer("Default");
        switch (gun)
        {
            case "pistol":
                RaycastHit hit;
                //ammo_UI.text = currentPistolAmmo.ToString() + "/" + pistolAmmo;
                if (currentPistolAmmo >= 1 && !TakingTimeToNextShot)
                {
                    pistolParticle.Play();
                    PlayAnimation(weaponAnimationName);
                    pistolSound.pitch = Random.Range(.9f, 1);
                    pistolSound.Play();
                    // Выполняем Raycast только для получения информации о попадании
                    bool hitDetected = Physics.Raycast(gunholder.position, gunholder.forward, out hit, 100f,layerMask);
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
                        StartCoroutine(ShootCor(hitDetected ? hit.point : gunholder.position + gunholder.forward * 100, hit.collider, false, pistolfireRate, pistolDamage));
                    }
                    // Уменьшаем количество патронов
                    currentPistolAmmo--;
                    // Проверяем, нужно ли перезаряжаться
                    if (shuffledPistolClips == null || currentPistolClipIndex >= shuffledPistolClips.Length)
                    {
                        shuffledPistolClips = (AudioClip[])pistolClips.Clone();

                        // Алгоритм Фишера-Йетса для перемешивания
                        for (int i = 0; i < shuffledPistolClips.Length; i++)
                        {
                            int rnd = Random.Range(i, shuffledPistolClips.Length);
                            AudioClip temp = shuffledPistolClips[i];
                            shuffledPistolClips[i] = shuffledPistolClips[rnd];
                            shuffledPistolClips[rnd] = temp;
                            currentPistolClipIndex = 0;
                        }
                    }

                    pistolSound.pitch = Random.Range(0.95f, 1f);
                    pistolSound.clip = shuffledPistolClips[currentPistolClipIndex++];
                    pistolSound.Play();

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
                    shotgunParticle.Play();
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
                        bool hitDetected = Physics.Raycast(gunholder.position, rayDirection, out hit4, 100f, layerMask);
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
                            StartCoroutine(ShootCor(hitDetected ? hit4.point : gunholder.position + rayDirection * 100, hit4.collider, false, shotgunFireRate, shotgunDamage));

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
                    submachinegunParticle.Play();
                    PlayAnimation(weaponAnimationName);
                    submachinegunSound.pitch = Random.Range(.9f, 1);
                    submachinegunSound.Play();
                    // Выполняем Raycast только для получения информации о попадании
                    bool hitDetected = Physics.Raycast(gunholder.position, gunholder.forward, out hit2, 100f, layerMask);
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
                        StartCoroutine(ShootCor(hitDetected ? hit2.point : gunholder.position + gunholder.forward * 100, hit2.collider, false, submachinegunFireRate, submachinegunDamage));
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
                    rifleParticle.Play();
                    PlayAnimation(weaponAnimationName);
                    rifleSound.pitch = Random.Range(.9f, 1);
                    rifleSound.Play();
                    // Выполняем Raycast только для получения информации о попадании
                    bool hitDetected = Physics.Raycast(gunholder.position, gunholder.forward, out hit3, 100f, layerMask);
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
                        StartCoroutine(ShootCor(hitDetected ? hit3.point : gunholder.position + gunholder.forward * 100, hit3.collider, false, rifleFireRate, rifleDamage));
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
        lineRenderer.enabled = false;
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
        lineRenderer.enabled = true;
        reloading = false;
        
    }
    IEnumerator ShootCor(Vector3 hitPoint, Collider hitCollider, bool isEnemyWasHit, float timeToShoot, int damage)
    {
        TakingTimeToNextShot = true;
        AggroNearbyEnemies(transform);
        Sequence sequence = DOTween.Sequence();
        var tracer = Instantiate(shootTracer);
        Vector3 gnhldr = Vector3.zero;

        switch (gun)
        {
            case "pistol":
                gnhldr=gunholderP.position;
                break;
            case "shotgun":
                gnhldr=gunholderS.position;
                break;
            case "submachinegun":
                gnhldr = gunholderSM.position;
                break;
            case "rifle":
                gnhldr = gunholderR.position;
                break;
        }
        float disTr= Vector3.Distance(gnhldr, hitPoint);
        Vector3 direction = hitPoint - gnhldr;
        Quaternion rotation = Quaternion.LookRotation(direction);
        tracer.transform.rotation = rotation;
        // 1. Включаем и ставим в точку A
        sequence.AppendCallback(() =>
        {
            tracer.transform.position = gnhldr;
        });

        // 2. Летим в точку B за 0.1 сек
        sequence.Append(tracer.transform.DOMove(hitPoint, disTr/50));


        // 4. Выключаем
        sequence.AppendCallback(() => Destroy(tracer));
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

    public void AggroNearbyEnemies(Transform hitTransform)
    {
        Collider[] hitColliders = Physics.OverlapSphere(hitTransform.position, _agroRadius);
        List<Enemy> nearbyEnemies = new List<Enemy>();

        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null && !enemy.dead)
            {
                nearbyEnemies.Add(enemy);
            }
        }

        // Запускаем агр у всех найденных врагов
        foreach (Enemy enemy in nearbyEnemies)
        {
            // Если корутина уже запущена, останавливаем её
            if (enemy._damageAgrCoroutine != null)
            {
                enemy.StopCoroutine(enemy._damageAgrCoroutine);
            }

            // Запускаем корутину и сохраняем ссылку
            enemy._damageAgrCoroutine = enemy.StartCoroutine(enemy.DamageAgr(_agroTime));
        }
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

    public IEnumerator StopPlayer(int time)
    {
        enabled = false;
        yield return new WaitForSeconds(time);
        enabled = true;
    }
    // ----------УПРАВЛЕНИЕ АНИМАЦИЯМИ----------//КОНЕЦ
    public void CheckLowHealth()
    {
        if (vignette == null) return;

        float healthPercentage = (float)health / maxHealth;
        bool isLowHealth = healthPercentage <= 0.2f;

        if (isLowHealth)
        {
            vignette.color.Override(Color.red);

            float intensityTarget = 0.25f + Mathf.PingPong(Time.time * 0.25f, 0.35f);
            vignette.intensity.Override(Mathf.Lerp(vignette.intensity.value, intensityTarget, 1f * Time.deltaTime));


            if (!lowHpSource.isPlaying && lowHealthSound != null)
            {
                lowHpSource.clip = lowHealthSound;
                lowHpSource.Play();
            }
        }
        else
        {
            vignette.color.Override(Color.black);
            vignette.intensity.Override(0.25f);


            if (lowHpSource.isPlaying)
            {
                lowHpSource.Stop();
            }
        }
    }
    public void Damage(int dmg)
    {
        health -= dmg;
        StartCoroutine(Slow(2));
    }


    IEnumerator Slow(int slow)
    {
        moveSpeed -= slow;
        yield return new WaitForSeconds(1f);
        healthbarPast.fillAmount = health / 25f;
        moveSpeed += slow;

    }
}
