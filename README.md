## URP render feature wroks as adapter for any custom post process effect.

![image](https://github.com/user-attachments/assets/68827b20-39ab-4ee6-9cb2-032d2af01c59)
![image](https://github.com/user-attachments/assets/a0a4f6c1-6aa8-42b0-85af-e90e66fe23f1)


### It is easily configured in the usual way
![image](https://github.com/user-attachments/assets/01e7f49c-32aa-4383-ba49-db794ab88772)
![image](https://github.com/user-attachments/assets/806dce4f-aca7-469f-bf42-e8d645c5ef28)

- You can write your own **Effect** if you inherit from generic class `ImageEffectPass<TVolume>`.
  - https://github.com/vikle/URPImageEffectsAdapter/blob/main/Runtime/Effects/Blur/BlurPass.cs#L6-L32

- To write your own **Volume**, you need inherit it from class `ImageEffectVolume`.
  - https://github.com/vikle/URPImageEffectsAdapter/blob/main/Runtime/Effects/Blur/BlurVolume.cs#L6-L29

- To write your own **Volume Editor**, you need inherit it from generic class `ImageEffectVolumeEditor<TVolume>`.
  - https://github.com/vikle/URPImageEffectsAdapter/blob/main/Editor/BlurVolumeEditor.cs#L9-L36
