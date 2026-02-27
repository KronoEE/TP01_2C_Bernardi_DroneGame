using System.Collections;
using System.Collections.Generic;
public class Emotion 
{
    public PlayerController.EmotionType EmotionType;
    public bool bIsActive;
    public Emotion(PlayerController.EmotionType emotionType, bool isActive)
    {
        EmotionType = emotionType;
        bIsActive = isActive;
    }
}