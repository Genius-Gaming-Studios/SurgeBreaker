Hello! Thank you for the purchase. Hope the assets work well.
If they don't, please contact me: slsovest@gmail.com.



______________________
Assembling the robots:


Legs, arms, shoulders and cockpits have containers for mounting other parts, their names start with "Mount_".
(In some cases they may be deep in bone hierarchy).
Just drop the part in the corresponding container, and It'll snap into place.

- Start with legs (can be found in Prefabs/Walkers or Prefabs/Humanoids folders)
- The first container is in ROOT->Hips->Spine_1->Spine_2->Mount_top. 
- Put the shoulders into "Mount_top".
- The containers for the other parts can be found in:
Shoulders: for Arms (From Walkers/Arms folder)
Forearms: for Gadgets (All the parts from Prefabs/Gadgets folder)
Head: for Cockpit


- After the assembly, robots consist of many separate parts and, even with batching, produce high number of draw calls.
You may want to combine non-skinned parts that move along into a single mesh for the sake of optimization.

- All weapons (and gadgets with barrels) contain locators at their barrel ends (named "Barrel_end").

- Now, the shields can only be equipped to the left-side slots. The animation doesn't work right on the other side.
If you'll find some (easy) workaround for this, please write.

- To mount the cockpits from other packs to the shoulders from this one, use the "Prefabs/Cockpit_Spacer_Other_Packs" spacer,
looks better this way.


___________
Animations:


There are 2 types of mechs - Walkers and Humanoids. The pack contains animations only for Walkers.
But since all the mechs have humanoid avatars, you can use the animations from the other packs on the store.
The mechs work well with Walk/Run animations from "Unity Standard Assets" (https://www.assetstore.unity3d.com/en/#!/content/32351).
You can also try "Raw Mocap Data for Mecanim" (https://www.assetstore.unity3d.com/en/#!/content/5330)
Both are from Unity Technologieas and are free.


All the animations should be already sorted and named in Unity, but in case you're using them elsewhere:

Mech_Cockpits@Mech_Cockpit_Open.fbx contains 2 animations:
Open(frames 1-15), Close(20-30)

Mech_Walker@Walker_De_Activate.fbx
Deactivate(1-80), Activate(25-55)

Mech_Weapon_Shield@Weapon_Shield.fbx
(1-12)(12-31)(40-61)(61-71), Unpack(80-94), Pack(110-126), Hit_top(130-134), Hit_bottom(135-139)

Mech_Gadget_Shield@Gadget_Shield.fbx (works for backpack shields as well)
Unpack(1-12), Pack(20-30), Hit_top(40-44), Hit_bottom(45-49)

Models/Mech_Gadget_Saw@Gadget_Saw.fbx
Saw(50-57), Pack(1-21), Unpack(25-41)

Models/Mech_Hand_Claw@Hand_Claw.fbx (same for Mech_Hand_Claw_3x@Hand_Claw_3x.fbx)
Pack(1-13), Unpack(15-23), To-Drill(30-41), Drill(45-54), Pinch(70-85), Grab(90-102)

Models/Mech_Backpack_Dish_Lvl3@Backpack_Dish.fbx
Unpack(1-32), Pack(50-81), Search(100-300)

Mech_Backpack_Radar@Backpack_Radar_Close.fbx
Pack(1-18), Unpack(30-52), Search(60-250)

The "Spine1" bone is not used in any animation, you could use it to rotate the top part of the mech.

If you are familiar with Maya, and want to tweak the animations or create the new ones,
you can find the rigged mech in the "X_Maya_Rig.rar" in the "Models" folder.



___________________
Pilots (StarDudes):


Half of the cockpits have variations with no interior, you won't necessarily need pilots for them.
- I can recommend the StarDudes characters to use as pilots (https://www.assetstore.unity3d.com/en/#!/content/65747).
- The StarDudes characters should be scaled approximately 18 times (or 24 for the "XL" cockpits) to seat in the cockpits.
- You can find the seat pose in "Animations/X_StarDudes@Stardudes_Pilot_Pose.fbx".
- All the cockpits contain "Mount_Pilot_Hips", that's where the StarDudes Hips bone should be.



_________
Textures:


The source .PSD can be found in the "Materials" folder. 
For a quick repaint, adjust the layers in the "COLOR" folder. You can drop your decals and textures (camouflage, for example) in the folder as well. 
Just be careful with texture seams.
You may want to turn off the "FX_Rust" and "FX_Chipped_paint" layers for more clean look.
The baked occlusion and contours may be found in the "BKP" folder.

PBR:
To create the material in Unity 5, just plug in the textures from the "Unity_5_Standard_Shader_(Specular)" folder
into corresponding inputs of Unity 5 standard shader (Specular workflow).

Discovered that the mechs look quite interesing with outlines (you can check Prefabs/Humanoids/Mech_Humanoid_Lvl1_Outline).
These outlines are done with geometry, but there are plenty of shaders on the store that will help to achieve this effect.




I'm fairly new to Unity, if you have any ideas how I could organize the assets in a better way, please, write.
If you need some certain animations, please write me via slsovest@gmail.com.
I will try to include it in the future updates.



________
Updates:


Version 2.0 (March 2017):
Added PBR textures (camouflage and flat colored versions).
Added diffuse camouflage textures.
Refined normal maps.
Added new parts:
- energy shields (3 levels)
- energy shield effects (3 levels)
- 7 Cockpits
- 4 radar backpacks (animated)
- 2 container backpacks
- 10 new weapon gadgets