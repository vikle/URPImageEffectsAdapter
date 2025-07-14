# URP render feature for custom post process effects.

Orginal             |  Color Correction & Fog
:-------------------------:|:-------------------------:
![image](https://github.com/user-attachments/assets/304a4045-aa7a-486a-b8a2-a64b2acca617)  | ![image](https://github.com/user-attachments/assets/36eb2c31-b658-4d79-a1fd-ab97673247dd)

<details>
  <summary>[click] Sharpness</summary>
  
![image](https://github.com/user-attachments/assets/9f31e731-2d51-4fcd-afde-4a547134844c)
![image](https://github.com/user-attachments/assets/41774681-ce5a-437c-bd2c-d281e9fc4945)
  
</details>

<details>
   <summary>[click] Color Correction & Fog</summary>

![image](https://github.com/user-attachments/assets/e47cad49-8511-4b51-816e-085528747a2b)
![image](https://github.com/user-attachments/assets/984d3fa9-7e1c-4767-8360-d65f935d7c9f)
![image](https://github.com/user-attachments/assets/36eb2c31-b658-4d79-a1fd-ab97673247dd)
  
</details>

<details>
  <summary>[click] Kuwahara</summary>

![image](https://github.com/user-attachments/assets/2a2d56d7-be85-4973-baff-6565ab5917cd)
![image](https://github.com/user-attachments/assets/a4fcb28c-8273-4931-bee1-4df4b9fc71d4)

</details>

<details>
  <summary>[click] Cell Shading</summary>

![image](https://github.com/user-attachments/assets/a8ccf886-47c9-43bb-999b-0e17251975dc)
![image](https://github.com/user-attachments/assets/3b4f64db-e46b-461e-ac27-5a2ebf6ca9a4)

</details>

<details>
  <summary>[click] Color Blindness</summary>

![image](https://github.com/user-attachments/assets/942fa86b-f3c3-479f-a34c-2fd15efac20b)


<details>
  <summary>[click] Protanomaly</summary>

![image](https://github.com/user-attachments/assets/67b3a05d-77c0-44e2-a8e1-0209240d96fb)
  
</details>

<details>
  <summary>[click] Deuteranomaly</summary>

![image](https://github.com/user-attachments/assets/9d236e9a-d36c-442b-a325-c94cf42a1ce1)
  
</details>

<details>
  <summary>[click] Tritanomaly</summary>

![image](https://github.com/user-attachments/assets/822863a0-8434-4bd4-a4ff-821acc88ceb8)
  
</details>

</details>



<details>
  <summary>[click] Blur</summary>
  
![image](https://github.com/user-attachments/assets/5c7bc2c2-d0ca-47b7-b93a-bc02e5e4b466)
![image](https://github.com/user-attachments/assets/4dc30476-a154-40da-98df-051c85aa8ac2)

<details>
  <summary>[click] Box</summary>

![image](https://github.com/user-attachments/assets/90e0a78b-e024-4ac7-98dc-801ecde9566b)

</details>

<details>
  <summary>[click] Gaussian</summary>

![image](https://github.com/user-attachments/assets/b11859fc-b1aa-4017-9261-c74221592230)
  
</details>
  
</details>

</details>

## Optimization principle: one shader - one pass.
*with an even sum of shader passes

![image](https://github.com/user-attachments/assets/1ab1978d-ff2f-4333-9258-a41ec4bbd075)

## Easily configured of usual way

![image](https://github.com/user-attachments/assets/68dd0a05-15b8-46d1-b9b2-c458c8db8eb5)
![image](https://github.com/user-attachments/assets/f4a7b514-0fb9-40f7-b088-ac6db9ef08a7)

* You can write your own **Effect** if you inherit from generic class `ImageEffectPass<TVolume>`.
```c#
[CreateAssetMenu(menuName = "Custome Effect", fileName = "CustomEffectFile", order = 51)]
public sealed class CustomPass : ImageEffectPass<CustomEffectVolume>
{
    protected override Shader OnInitializeShader()
    {
        return Shader.Find("CustomShaderPath");
    }
        
    protected override void OnPrepare(Material material, CustomEffectVolume volume, Queue<int> shaderPasses)
    {
                    
    }
};
```

* To write your own **Volume**, you need inherit it from class `ImageEffectVolume`.
```c#
[VolumeComponentMenuForRenderPipeline("Custom Effect", typeof(UniversalRenderPipeline))]
public sealed class CustomEffectVolume : ImageEffectVolume
{
    public ClampedIntParameter intParam = new ClampedIntParameter(0, 0, 10);
    public ClampedFloatParameter floatParam = new ClampedFloatParameter(0.5f, 0f, 1f);
        
    public override bool IsActive()
    {
        return true;
    }
 };
```

* To write your own **Volume Editor**, you need inherit it from generic class `ImageEffectVolumeEditor<TVolume>`.
```c#
[CustomEditor(typeof(CustomEffectVolume))]
public sealed class CustomEffectVolumeEditor : ImageEffectVolumeEditor<CustomEffectVolume>
{
    SerializedDataParameter m_intParam;
    SerializedDataParameter m_floatParam;

    public override void OnEnable()
    {
        m_intParam = UnpackParameter(p => p.intParam);
        m_floatParam = UnpackParameter(p => p.floatParam);
    }

    public override void OnInspectorGUI()
    {
        PropertyField(m_intParam);
        PropertyField(m_floatParam);
    }
};
```
