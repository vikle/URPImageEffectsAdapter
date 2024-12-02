## URP render feature wroks as adapter for any custom post process effect.

Orginal             |  Color Correction & Fog
:-------------------------:|:-------------------------:
![image](https://github.com/user-attachments/assets/304a4045-aa7a-486a-b8a2-a64b2acca617)  | ![image](https://github.com/user-attachments/assets/36eb2c31-b658-4d79-a1fd-ab97673247dd)

<details>
  <summary>Sharpness</summary>
  
![image](https://github.com/user-attachments/assets/9f31e731-2d51-4fcd-afde-4a547134844c)
![image](https://github.com/user-attachments/assets/41774681-ce5a-437c-bd2c-d281e9fc4945)
  
</details>

<details>
   <summary>Color Correction & Fog</summary>

![image](https://github.com/user-attachments/assets/e47cad49-8511-4b51-816e-085528747a2b)
![image](https://github.com/user-attachments/assets/984d3fa9-7e1c-4767-8360-d65f935d7c9f)
![image](https://github.com/user-attachments/assets/36eb2c31-b658-4d79-a1fd-ab97673247dd)
  
</details>

<details>
  <summary>Kuwahara</summary>

![image](https://github.com/user-attachments/assets/2a2d56d7-be85-4973-baff-6565ab5917cd)
![image](https://github.com/user-attachments/assets/a4fcb28c-8273-4931-bee1-4df4b9fc71d4)

</details>

<details>
  <summary>Cell Shading</summary>

![image](https://github.com/user-attachments/assets/a8ccf886-47c9-43bb-999b-0e17251975dc)
![image](https://github.com/user-attachments/assets/3b4f64db-e46b-461e-ac27-5a2ebf6ca9a4)

</details>

<details>
  <summary>Color Blindness</summary>

![image](https://github.com/user-attachments/assets/942fa86b-f3c3-479f-a34c-2fd15efac20b)


<details>
  <summary>Protanomaly</summary>

![image](https://github.com/user-attachments/assets/67b3a05d-77c0-44e2-a8e1-0209240d96fb)
  
</details>

<details>
  <summary>Deuteranomaly</summary>

![image](https://github.com/user-attachments/assets/9d236e9a-d36c-442b-a325-c94cf42a1ce1)
  
</details>

<details>
  <summary>Tritanomaly</summary>

![image](https://github.com/user-attachments/assets/822863a0-8434-4bd4-a4ff-821acc88ceb8)
  
</details>

</details>



<details>
  <summary>Blur</summary>
  
![image](https://github.com/user-attachments/assets/5c7bc2c2-d0ca-47b7-b93a-bc02e5e4b466)
![image](https://github.com/user-attachments/assets/4dc30476-a154-40da-98df-051c85aa8ac2)

<details>
  <summary>Box</summary>

![image](https://github.com/user-attachments/assets/90e0a78b-e024-4ac7-98dc-801ecde9566b)

</details>

<details>
  <summary>Gaussian</summary>

![image](https://github.com/user-attachments/assets/b11859fc-b1aa-4017-9261-c74221592230)
  
</details>
  
</details>

</details>

### Optimization principle: one shader - one pass.
*with an even number of active shader passes
![image](https://github.com/user-attachments/assets/25053a53-b3c2-4359-a8d7-57a6a9137439)

### It is easily configured in the usual way
![image](https://github.com/user-attachments/assets/68dd0a05-15b8-46d1-b9b2-c458c8db8eb5)

- You can write your own **Effect** if you inherit from generic class `ImageEffectPass<TVolume>`.
  - https://github.com/vikle/URPImageEffectsAdapter/blob/main/Runtime/Effects/Blur/BlurPass.cs#L6-L27

- To write your own **Volume**, you need inherit it from class `ImageEffectVolume`.
  - https://github.com/vikle/URPImageEffectsAdapter/blob/main/Runtime/Effects/Blur/BlurVolume.cs#L6-L29

- To write your own **Volume Editor**, you need inherit it from generic class `ImageEffectVolumeEditor<TVolume>`.
  - https://github.com/vikle/URPImageEffectsAdapter/blob/main/Editor/BlurVolumeEditor.cs#L9-L36
