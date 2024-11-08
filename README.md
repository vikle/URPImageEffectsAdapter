# URPCustomPostProcess
### URP render feature wroks as adapter for any custom post process effect.

![image](https://github.com/user-attachments/assets/3a3bac77-8453-4000-8f5d-fb83cb241bae)

### It is easily configured in the usual way
![image](https://github.com/user-attachments/assets/01e7f49c-32aa-4383-ba49-db794ab88772)
![image](https://github.com/user-attachments/assets/806dce4f-aca7-469f-bf42-e8d645c5ef28)

- You can write your own **Effect** if you inherit from generic class `ImageEffectPass<TVolume>`.
  - https://github.com/vikle/URPImageEffectsAdapter/blob/main/Runtime/Effects/Blur/BlurPass.cs#L6-L32

- To write your own **Volume**, you need inherit it from class `ImageEffectVolume`.
  - https://github.com/vikle/URPImageEffectsAdapter/blob/main/Runtime/Effects/Blur/BlurVolume.cs#L6-L29

- To write your own **Volume Editor**, you need inherit it from generic class `ImageEffectVolumeEditor<TVolume>`.
  - https://github.com/vikle/URPImageEffectsAdapter/blob/main/Editor/BlurVolumeEditor.cs#L9-L36
