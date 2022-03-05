# Stardust-SDK demo

## Welcome

This page will explain you easily how to use the Stardust SDK for mapping and relocation.

## Why should I pick Stardust over existing SDK's

1. **You only need phone compatible with ARCore/ARkit**: you don't need your users to have the latests phones to be able to use it (we do our tests with an old IPhone 8 but also with Samsung Galaxy S8, Xiaomi Mi8 and even Xiaomi Mix 2).

2. **You can update your maps** : in real life environment changes. It can be the light, the position of objects, the weather or even the crowd. Our SDK manages these different situations by just allowing you to update your map with new datas.

3. **You can easily edit your maps**: We understand that you don't want to have an separate software to manage your maps, so you can edit them online

4. **Privacy**: The datas you send to us are yours, we will not share it with third-parties for any purposes.

5. **Creating your own INTERACTIVE objects and share it! (or not...)**: Our system allows you to upload your own objects to use in the editor or in your apps. The system is compatible with our interaction framework in order to create interactive experiences and not just contemplative ones.

## How can I map ?

### Mapping and update instructions

- Move at a **slow pace** to make sure the captured datas are good quality
- **Do not make sudden rotations** otherwise the generated map result can have issues
- Make sure you send enough datas (our tests show that 150 pictures for a 30 sqm office should be fine)
- Only the data **sent** will be taken into account during the map generation, we only provide the data captured number as a reference
- Once you launched the map generation, the data processing can take between 30 minutes and 1 hour depending on the number of pictures you provided
- The compass will be the origin of your map

### Specific update instructions


- You need to start at thhe **SAME POSITION** and **SAME ORIENTATION** before starting the scene (compass should be around the same place as during the mapping)
- You can only update a map if it has **already** been generated
- You can map places **that were not previously mapped** in the original mapping
- You can map **multiple updates** before effectively running the generation again

## Creating and sharing objects


A full video of the process is available [here](https://www.youtube.com/watch?v=TRg6cKWQMqI).

### Why not just let me upload a FBX or an OBJ file ?

We have in mind that we should allow the creation of interactive objects not only static ones. You can't have interactions scripts on FBX or OBJ unless you write a specific code for it. We want people to be able to create an interactive experience WITHOUT writing any line of code. Examples of what our framework can do can be found [here](https://www.instagram.com/ar3aapp/). All experiences on this app were done without writing a single line of code by our designer thanks to the framework

### Creating new objects

If the video above is not clear you can follow the full [unity tutorial for asset bundle generation](https://docs.unity3d.com/Manual/AssetBundles-Browser.html) or contact our team (devteam[at]neogoma.com).

### Uploading new objects

- Login into the [dashboard](https://stardust.neogoma.com) then select the "Object List" menu
- Select "Create new object"
- Enter the name of your object (**must be the same as the generated bundle name**)
- Choose if you want to make your object public or not (every other developer will be able to use it)
- Select the folders with the files for **each specific platform**. It's **not mandatory** to upload on every platform but if a bundle is not available for a platform a **default error message will be displayed** when trying to download it.
- If you delete an object, it will be **removed from all the maps**.

## Editor commands

### Moving around

You can use the following commands to navigate around in the editor:
- Z or UP: go forward
- S or DOWN : go backward
- A or LEFT: go left
- D or RIGHT: go right
- RIGHT CLICK + moving mouse: Change camera orientation

### Support or contact

If you're having trouble using the SDK, don't hesitate to create a [new issue](https://github.com/Neogoma/stardust-SDK/issues/new/choose) or contact us @ devteam[at]neogoma.com
