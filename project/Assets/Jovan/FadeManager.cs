using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance{set;get;}
    public Image fadeImage; 
    private bool isIntransition;
    private float transition;
    private bool isShowing;
    private float duration; 
    public int index;
    public FadeTrigger FadeTrig;
    


    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(this);
        
     
    }


    void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (FadeTrig.fadeOut == true){
            Fade(false,1.80f);
        }
        
    }
    void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void Fade(bool showing, float duration){
        isShowing = showing;
        isIntransition = true;
        this.duration = duration;
        transition = (isShowing) ? 0 : 1; 
    }
    
    
    void Update(){
        if(!isIntransition){return;}

        transition += (isShowing) ? Time.deltaTime * (1/duration) : -Time.deltaTime * (1/duration);
        //this.GetComponent<Image>().color = Color.Lerp(new Color(1,1,1,0), Color.white, transition);
        fadeImage.color = Color.Lerp(new Color(1,1,1,0), Color.white, transition);

        if(transition > 1 || transition < 0){
            isIntransition = false; 
        }
        /* if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("Playground 1")){
            print("fade out");
            Fade(false,1.25f);
        } */
        
    }


 
   
}
