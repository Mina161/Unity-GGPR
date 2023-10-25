using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Transform t;
    private Rigidbody rb;
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;

    [SerializeField]
    AudioSource pickup;

    [SerializeField]
    AudioSource hit;

    [SerializeField]
    AudioSource kaboom;

    [SerializeField]
    AudioSource transformation;

    [SerializeField]
    AudioSource error;

    [SerializeField]
    AudioSource multiply;

    [SerializeField]
    AudioSource shieldSound;

    [SerializeField]
    AudioSource huh;

    [SerializeField]
    AudioSource death;

    [SerializeField]
    TMP_Text greenText;

    [SerializeField]
    TMP_Text redText;

    [SerializeField]
    TMP_Text blueText;

    [SerializeField]
    TMP_Text scoreText;

    [SerializeField]
    GameObject shield;

    [SerializeField]
    GameObject greenShield;

    [SerializeField]
    public Material material;

    private int score = 0;
    private int blue = 0;
    private int red = 0;
    private int green = 0;
    private bool alive = true;
    private Vector3 touchStartPos;
    private Vector3 touchEndPos;
    private bool isSwiping = false;
    private int currentForm = 0; //0->None 1->Red 2->Green 3->Blue
    private bool nuke = false;
    private int multiplier = 1;
    private bool invincible = false;
    private bool cheater = false;



    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        shield.SetActive(false);
        greenShield.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            setFormRed();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            setFormGreen();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            setFormBlue();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (currentForm)
            {
                case 0: break;
                case 1: powerNuke(); break;
                case 2: powerMultiply(); break;
                case 3: powerShield(); break;
                default: error.Play(); break;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (t.position.x > -5)
            {
                t.Translate(new Vector3(-5, 0, 0));
            }
            else
            {
                error.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (t.position.x < 5)
            {
                t.Translate(new Vector3(5, 0, 0));
            }
            else
            {
                error.Play();
            }
        }

        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                firstPressPos = new Vector2(touch.position.x, touch.position.y);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                secondPressPos = new Vector2(touch.position.x, touch.position.y);
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
                currentSwipe.Normalize();
                if (currentSwipe.x < 0)
                {
                    if (t.position.x > -5)
                    {
                        t.Translate(new Vector3(-5, 0, 0));
                    }
                    else
                    {
                        error.Play();
                    }
                }
                if (currentSwipe.x > 0)
                {
                    if (t.position.x < 5)
                    {
                        t.Translate(new Vector3(5, 0, 0));
                    }
                    else
                    {
                        error.Play();
                    }
                }
            }
        }

        //Cheats
        if (Input.GetKeyDown(KeyCode.I))
        {
            cheater = true;
            if (red < 5)
            {
                red++;
                redText.SetText("Red: " + red);
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            cheater = true;
            if (green < 5)
            {
                green++;
                greenText.SetText("Green: " + green);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            cheater = true;
            if(blue < 5) 
            {
                blue++;
                blueText.SetText("Blue: " + blue);
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            cheater = true;
            invincible = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("red-powerup"))
        {
            other.gameObject.SetActive(false);
            int newScore = 0;
            if (currentForm == 1)
            {
                newScore++;
            }
            newScore++;
            score += newScore * multiplier;
            if (red < 5)
            {
                int newRed = 1 * multiplier;
                red += Mathf.Min(2, newRed);
                red = Mathf.Min(5, red);
                redText.SetText("Red: " + red);
            }
            if (multiplier > 1)
            {
                multiplier = 1;
                greenShield.SetActive(false);
            }
            scoreText.SetText("Score: " + score);
            pickup.Play();
        }

        if (other.gameObject.CompareTag("green-powerup"))
        {
            other.gameObject.SetActive(false);
            int newScore = 0;
            if (currentForm == 2)
            {
                newScore++;
            }
            newScore++;
            score += newScore * multiplier;
            if (multiplier == 1)
            {
                if(green < 5)
                {
                    green++;
                    greenText.SetText("Green: " + green);
                }
                pickup.Play();
            }
            if (multiplier > 1)
            {
                huh.Play();
                multiplier = 1;
                greenShield.SetActive(false);
            }
            scoreText.SetText("Score: " + score);
        }

        if (other.gameObject.CompareTag("blue-powerup"))
        {
            other.gameObject.SetActive(false);
            int newScore = 0;
            if (currentForm == 3)
            {
                newScore++;
            }
            newScore++;
            score += newScore * multiplier;
            if (blue < 5)
            {
                int newBlue = 1 * multiplier;
                blue += Mathf.Min(2, newBlue);
                blue = Mathf.Min(5, blue);
                blueText.SetText("Blue: " + blue);
            }
            if (multiplier > 1)
            {
                multiplier = 1;
                greenShield.SetActive(false);
            }
            scoreText.SetText("Score: " + score);
            pickup.Play();
        }

        if (other.gameObject.CompareTag("Finish") && !invincible)
        {
            if(currentForm == 0)
            {
                alive = false;
                death.Play();
                other.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
            } 
            else
            {
                if (shield.activeSelf)
                {
                    other.gameObject.SetActive(false);
                    shield.SetActive(false);
                    hit.Play();
                }
                else if (greenShield.activeSelf)
                {
                    other.gameObject.SetActive(false);
                    greenShield.SetActive(false);
                    multiplier = 1;
                    currentForm = 0;
                    material.SetColor("_Color", Color.white);
                    hit.Play();
                }
                else
                {
                    other.gameObject.SetActive(false);
                    currentForm = 0;
                    material.SetColor("_Color", Color.white);
                    hit.Play();
                }
            }
        }
    }

    public void setFormRed()
    {
            if (red >= 5 && currentForm != 1)
            {
                shield.SetActive(false);
                greenShield.SetActive(false);
                red--;
                redText.SetText("Red: " + red);
                if (red == 0)
                {
                    currentForm = 0;
                    material.SetColor("_Color", Color.white);
                    return;
                }
                currentForm = 1;
                material.SetColor("_Color", Color.red);
                transformation.Play();
            }
            else
            {
                error.Play();
            }
    }

    public void setFormGreen()
    {
        if (green >= 5 && currentForm != 2)
        {
            shield.SetActive(false);
            green--;
            greenText.SetText("Green: " + green);
            if (green == 0)
            {
                currentForm = 0;
                material.SetColor("_Color", Color.white);
                return;
            }
            currentForm = 2;
            material.SetColor("_Color", Color.green);
            transformation.Play();
        }
        else
        {
            error.Play();
        }
    }

    public void setFormBlue()
    {
        if (blue >= 5 && currentForm != 3)
        {
            greenShield.SetActive(false);
            blue--;
            blueText.SetText("Blue: " + blue);
            if (blue == 0)
            {
                currentForm = 0;
                material.SetColor("_Color", Color.white);
                return;
            }
            currentForm = 3;
            material.SetColor("_Color", Color.blue);
            transformation.Play();
        }
        else
        {
            error.Play();
        }
    }

    public void powerup()
    {
        if (currentForm == 1)
        {
            powerNuke();
        } 
        else if (currentForm == 2)
        {
            powerMultiply();
        }
        else if(currentForm == 3)
        {
            powerShield();
        }
        else
        {
            error.Play();
        }
    } 

    public void powerNuke()
        {
            if (currentForm == 1 && red > 0 && !nuke)
            {
                red--;
                redText.SetText("Red: " + red);
                if (red == 0)
                {
                    currentForm = 0;
                    material.SetColor("_Color", Color.white);
                }
                nuke = true;
                kaboom.Play();
                return;
            }
            error.Play();
    }

    public void powerMultiply()
    {
        if (currentForm == 2 && green > 0 && multiplier < 5)
        {
            green--;
            greenText.SetText("Green: " + green);
            if (green == 0)
            {
                currentForm = 0;
                material.SetColor("_Color", Color.white);
                return;
            }
            multiplier = 5;
            greenShield.SetActive(true);
            multiply.Play();
            return;
        }
        error.Play();
    }

    public void powerShield()
    {
        if (currentForm == 3 && blue > 0 && !shield.activeSelf)
        {
            blue--;
            blueText.SetText("Blue: " + blue);
            if (blue == 0)
            {
                currentForm = 0;
                material.SetColor("_Color", Color.white);
                return;
            }
            shield.SetActive(true);
            shieldSound.Play();
            return;
        }
        error.Play();
    }

    public int getScore()
    {
        return score;
    }

    public bool getCheater()
    {
        return cheater;
    }

    public bool getNuke()
    {
        return nuke;
    }

    public void setNuke(bool nuke)
    {
        this.nuke = nuke;
    }

    public bool getAlive()
    {
        return alive;
    }
}