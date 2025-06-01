using UnityEngine;
using UnityEngine.UI;

public class PlayerEmote : MonoBehaviour
{
    public int stress = 0;
    private int maxStress = 100;

    [Header("Emote Süreleri")]
    [SerializeField] float shortEmoteDuration = 1f;
    [SerializeField] float mediumEmoteDuration = 2f;
    [SerializeField] float longEmoteDuration = 3f;

    [Header("Stres Artışı")]
    [SerializeField] int shortEmoteStress = 1;
    [SerializeField] int mediumEmoteStress = 3;
    [SerializeField] int longEmoteStress = 5;

    [SerializeField] Slider stressSlider;

    private bool isEmoting = false;
    private float emoteTimer = 0f;
    private int currentEmoteStress = 0;

    private PlayerMovement playerMovement;
    private bool emoteInterrupted = false;

    [SerializeField] CubeBoss cubeBoss;
    private Animator animator;

    [SerializeField] GameObject objNah;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        if (stressSlider != null)
        {
            stressSlider.maxValue = maxStress;
            stressSlider.value = 0;
        }
    }

    void Update()
    {
        updateStressBar();

        if (!isEmoting)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartEmote(shortEmoteDuration, shortEmoteStress);
            else if (Input.GetKeyDown(KeyCode.Alpha2) && playerMovement.isGrounded)
                StartEmote(mediumEmoteDuration, mediumEmoteStress);
            else if (Input.GetKeyDown(KeyCode.Alpha3) && playerMovement.isGrounded)
                StartEmote(longEmoteDuration, longEmoteStress);
        }
        else
        {
            emoteTimer -= Time.deltaTime;
            if (emoteTimer <= 0f && !emoteInterrupted)
            {
                EndEmote(true);
            }
        }
    }

    void StartEmote(float duration, int stressAmount)
    {
        if (duration == mediumEmoteDuration)
        {
            animator.SetBool("Taunt2", true);
        }else if (duration == shortEmoteDuration)
        {
            objNah.SetActive(true);
        }else if (duration == longEmoteDuration)
        {
            animator.SetBool("Taunt3", true);
        }
        isEmoting = true;
        emoteTimer = duration;
        currentEmoteStress = stressAmount;
        emoteInterrupted = false;

        if (playerMovement != null)
            playerMovement.enabled = false;

        Debug.Log($"Emote başladı ({duration} sn) → {stressAmount} stres kazandıracak.");
    }

    public void InterruptEmote()
    {
        if (isEmoting)
        {
            Debug.Log("Emote kesildi!");
            emoteInterrupted = true;
            EndEmote(false);
        }
    }

    void EndEmote(bool success)
    {
        isEmoting = false;
        emoteTimer = 0f;

        animator.SetBool("Taunt2", false);
        animator.SetBool("Taunt3", false);
        objNah.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (success)
        {
            stress += currentEmoteStress;
            if (stress >= maxStress)
            {
                if(cubeBoss != null)
                    cubeBoss.Die();
            }

            Debug.Log($"Emote tamamlandı! Toplam stres: {stress}");
        }

        currentEmoteStress = 0;
    }

    public bool IsEmoting()
    {
        return isEmoting;
    }
    
    void updateStressBar()
    {
        if (stressSlider != null)
        {
            stressSlider.value = stress;
        }
    }
}
