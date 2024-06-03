using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using UMA.Editors;
using UMA.CharacterSystem;

namespace UMA.CharacterSystem.Editors
{
    [CustomEditor(typeof(DynamicCharacterAvatar), true)]
	public partial class DynamicCharacterAvatarEditor : Editor
	{		
		public static bool showHelp = false;
		public static bool showWardrobe = false;
		public static bool showEditorCustomization = true;
		public static bool showPrefinedDNA = false;
        public static bool showAnimatorGUI = false;
        public static bool showBlendshapes = false;
        public static bool showUMAFramework = false;

		public static int currentcolorfilter=0;
		public string[] colorfilters = { "Base", "All", "Hide ColorDNA" };
		public List<string> baseColorNames = new List<string>();
		public int currentDNA = 0;
		private string cachedRace = "";
		private string[] cachedRaceDNA = { };
		private string[] rawcachedRaceDNA = { };

		protected DynamicCharacterAvatar thisDCA;
		protected RaceSetterPropertyDrawer _racePropDrawer = new RaceSetterPropertyDrawer();
		protected WardrobeRecipeListPropertyDrawer _wardrobePropDrawer = new WardrobeRecipeListPropertyDrawer();
		protected RaceAnimatorListPropertyDrawer _animatorPropDrawer = new RaceAnimatorListPropertyDrawer();
        SerializedProperty animationController;
        public void OnEnable()
		{
			baseColorNames.Clear();
			baseColorNames.AddRange(new string[] { "skin","hair","eyes"});
			thisDCA = target as DynamicCharacterAvatar;
			/*
			if (thisDCA.context == null)
			{
				thisDCA.context = UMAContextBase.Instance;
				if (thisDCA.context == null)
				{
					thisDCA.context = thisDCA.CreateEditorContext();
				}
			}
			else if (thisDCA.context.gameObject.name == "UMAEditorContext")
			{
				//this will set also the existing Editorcontext if there is one
				thisDCA.CreateEditorContext();
			}
			else if (thisDCA.context.gameObject.transform.parent != null)
			{
				//this will set also the existing Editorcontext if there is one
				if (thisDCA.context.gameObject.transform.parent.gameObject.name == "UMAEditorContext")
					thisDCA.CreateEditorContext();
			}*/
			_racePropDrawer.thisDCA = thisDCA;
			_wardrobePropDrawer.thisDCA = thisDCA;
			_animatorPropDrawer.thisDCA = thisDCA;
		}

		public void SetNewColorCount(int colorCount)
		{
			var newcharacterColors = new List<DynamicCharacterAvatar.ColorValue>();
			for (int i = 0; i < colorCount; i++)
			{
				if (thisDCA.characterColors.Colors.Count > i)
				{
					newcharacterColors.Add(thisDCA.characterColors.Colors[i]);
				}
				else
				{
					newcharacterColors.Add(new DynamicCharacterAvatar.ColorValue(3));
				}
			}
			thisDCA.characterColors.Colors = newcharacterColors;
		}

		protected bool characterAvatarLoadSaveOpen;

		private void BeginVerticalPadded()
        {
			if (EditorGUIUtility.isProSkin)
			{
				GUIHelper.BeginVerticalPadded(10, new Color(1.3f, 1.4f, 1.5f));
			}
			else
			{
				GUIHelper.BeginVerticalPadded(10, new Color(0.75f, 0.875f, 1f));
			}
		}

		private void EndVerticalPadded()
        {
			GUIHelper.EndVerticalPadded(10);
        }

		public override void OnInspectorGUI()
        {
            thisDCA = target as DynamicCharacterAvatar;
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            showHelp = EditorGUILayout.Toggle("Show Help", showHelp);
            thisDCA.userInformation = EditorGUILayout.TextField("User Information", thisDCA.userInformation);
            if (showHelp)
            {
                EditorGUILayout.HelpBox("User Information: This is a field for you to put any information you want to store with the character. It is not used by the system in any way.", MessageType.Info);
            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            if (Application.isPlaying)
            {
                BeginVerticalPadded();
                EditorGUILayout.LabelField("Force Regenerate (Playtime)", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Full Build"))
                {
                    thisDCA.BuildCharacter(true);
                }
                if (GUILayout.Button("Textures"))
                {
                    thisDCA.ForceUpdate(false, true, false);
                }
                if (GUILayout.Button("DNA"))
                {
                    thisDCA.ForceUpdate(true, false, false);
                }
                if (GUILayout.Button("Mesh"))
                {
                    thisDCA.ForceUpdate(false, false, true);
                }
                EditorGUILayout.EndHorizontal();
                EndVerticalPadded();
            }

 //           EditorGUI.BeginChangeCheck();
 //
//			Editor.DrawPropertiesExcluding(serializedObject, new string[] { "hide","BundleCheck", "loadBlendShapes","loadOnlyUsedBlendshapes","loadBlendshapeNormals","loadBlendshapeTangents","loadAllFrames","activeRace","defaultChangeRaceOptions","cacheCurrentState", "rebuildSkeleton", "preloadWardrobeRecipes", "raceAnimationControllers","rawAvatar","RecreateAnimatorOnRaceChange",
//				/* Editor Only Fields */ "editorTimeGeneration",
//                "characterColors","BoundsOffset","_buildCharacterEnabled","keepAvatar","KeepAnimatorController","keepPredefinedDNA",
				///*LoadOtions fields*/ "defaultLoadOptions", "loadPathType", "loadPath", "loadFilename", "loadString", "loadFileOnStart", "waitForBundles", /*"buildAfterLoad",*/
				///*SaveOptions fields*/ "defaultSaveOptions", "savePathType","savePath", "saveFilename", "makeUniqueFilename","ensureSharedColors", 
				///*Moved into AdvancedOptions*/"context","umaData","umaRecipe", "umaAdditionalRecipes","umaGenerator", "animationController","forceSlotMaterials", "defaultRendererAsset","forceRebindAnimator",
				///*Moved into CharacterEvents*/"CharacterCreated", "CharacterBegun", "CharacterStart","CharacterUpdated", "CharacterDestroyed", "CharacterDnaUpdated", "RecipeUpdated", "AnimatorStateSaved", "AnimatorStateRestored","WardrobeAdded","WardrobeRemoved","BuildCharacterBegun","SlotsHidden","WardrobeSuppressed",
				///*PlaceholderOptions fields*/"showPlaceholder", "previewModel", "customModel", "customRotation", "previewColor", "AtlasResolutionScale","DelayUnload","predefinedDNA","alwaysRebuildSkeleton", "umaRecipe"});
  //          if (EditorGUI.EndChangeCheck())
  //          {
  //              serializedObject.ApplyModifiedProperties();
  //          }
            //The base DynamicAvatar properties- get these early because changing the race changes someof them
            SerializedProperty context = serializedObject.FindProperty("context");
            SerializedProperty umaData = serializedObject.FindProperty("umaData");
            SerializedProperty umaGenerator = serializedObject.FindProperty("umaGenerator");
            SerializedProperty umaRecipe = serializedObject.FindProperty("umaRecipe");
            SerializedProperty umaAdditionalRecipes = serializedObject.FindProperty("umaAdditionalRecipes");
            animationController = serializedObject.FindProperty("animationController");

            // ************************************************************
            // Set the race
            // ************************************************************
            SerializedProperty thisRaceSetter = serializedObject.FindProperty("activeRace");
            Rect currentRect = EditorGUILayout.GetControlRect(false, _racePropDrawer.GetPropertyHeight(thisRaceSetter, GUIContent.none));
            EditorGUI.BeginChangeCheck();
            _racePropDrawer.OnGUI(currentRect, thisRaceSetter, new GUIContent(thisRaceSetter.displayName));
            if (EditorGUI.EndChangeCheck())
            {
                bool okToProcess = true;
                // check to see if we changed it while playing, and if so, don't do it again.
                if (Application.isPlaying)
                {
                    if (thisDCA.activeRace.data != null)
                    {
                        if (thisDCA.activeRace.data.raceName == (string)thisRaceSetter.FindPropertyRelative("name").stringValue)
                        {
                            okToProcess = false;
                        }
                    }
                }

                if (okToProcess && thisDCA.editorTimeGeneration)
                {
                    thisDCA.ChangeRace((string)thisRaceSetter.FindPropertyRelative("name").stringValue, DynamicCharacterAvatar.ChangeRaceOptions.useDefaults, true);
                    //Changing the race may cause umaRecipe, animationController to change so forcefully update these too
                    umaRecipe.objectReferenceValue = thisDCA.umaRecipe;
                    animationController.objectReferenceValue = thisDCA.animationController;
                    serializedObject.ApplyModifiedProperties();
                    GenerateSingleUMA(thisDCA.rebuildSkeleton);
                }
            }
            if (showHelp)
            {
                EditorGUILayout.HelpBox("Active Race: Sets the race of the character, which defines the base recipe to build the character, the available DNA, and the available wardrobe.", MessageType.Info);
            }


            //**************************************
            // Begin In-Editor customization
            //**************************************
            showEditorCustomization = EditorGUILayout.Foldout(showEditorCustomization, new GUIContent("Customization", "Properties for customizing the look of the UMA"));

            if (showEditorCustomization)
            {
                BeginVerticalPadded();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Save Preset"))
                {
                    string fileName = EditorUtility.SaveFilePanel("Save Preset", "", "DCAPreset", "umapreset");
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        try
                        {
                            UMAPreset prs = new UMAPreset();
                            prs.DefaultColors = thisDCA.characterColors;
                            prs.PredefinedDNA = thisDCA.predefinedDNA;
                            prs.DefaultWardrobe = thisDCA.preloadWardrobeRecipes;
                            string presetstring = JsonUtility.ToJson(prs);
                            System.IO.File.WriteAllText(fileName, presetstring);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogException(ex);
                            EditorUtility.DisplayDialog("Error", "Error writing preset file: " + ex.Message, "OK");
                        }
                    }
                }
                if (GUILayout.Button("Load Preset"))
                {
                    string fileName = EditorUtility.OpenFilePanel("Load Preset", "", "umapreset");
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        try
                        {
                            string presetstring = System.IO.File.ReadAllText(fileName);
                            thisDCA.InitializeFromPreset(presetstring);
                            UpdateCharacter();
                        }
                        catch (Exception ex)
                        {
                            Debug.LogException(ex);
                            EditorUtility.DisplayDialog("Error", "Error writing preset file: " + ex.Message, "OK");
                        }
                    }
                }
                if (GUILayout.Button("Save AvatarDef"))
                {
                    string fileName = EditorUtility.SaveFilePanel("Save Avatar Definition File", "", "", "umaDef");
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        try
                        {
                            string charstr = thisDCA.GetAvatarDefinition(false, false).ToCompressedString("|");
                            System.IO.File.WriteAllText(fileName, charstr);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogException(ex);
                            EditorUtility.DisplayDialog("Error", "Error writing avatar definition file: " + ex.Message, "OK");
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Regen"))
                {
                    UpdateCharacter();
                }
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.EndHorizontal();

                if (EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Save Avatar Definition"))
                    {
                        string fileName = EditorUtility.SaveFilePanel("Save Avatar Definition", "", "", "adf");
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            try
                            {
                                AvatarDefinition adf = thisDCA.GetAvatarDefinition(false, false);
                                string charstr = adf.ToCompressedString("|");
                                System.IO.File.WriteAllText(fileName, charstr);
                            }
                            catch (Exception ex)
                            {
                                Debug.LogException(ex);
                                EditorUtility.DisplayDialog("Error", "Error writing avatar definition file: " + ex.Message, "OK");
                            }
                        }
                    }
                    if (GUILayout.Button("Load Avatar Definition"))
                    {
                        string fileName = EditorUtility.OpenFilePanel("Load Avatar Definition", "", "adf");
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            try
                            {
                                string presetstring = System.IO.File.ReadAllText(fileName);
                                AvatarDefinition adf = AvatarDefinition.FromCompressedString(presetstring, '|');
                                thisDCA.LoadAvatarDefinition(adf);
                                thisDCA.BuildCharacter(false);
                            }
                            catch (Exception ex)
                            {
                                Debug.LogException(ex);
                                EditorUtility.DisplayDialog("Error", "Error writing preset file: " + ex.Message, "OK");
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("editorTimeGeneration"));
                // wtf not working? EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultRendererAsset"));
                thisDCA.defaultRendererAsset = (UMARendererAsset)EditorGUILayout.ObjectField("Default Renderer Asset", thisDCA.defaultRendererAsset, typeof(UMARendererAsset), false);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    UpdateCharacter();
                }


                //******************************************************************
                // Preload wardrobe
                //Other DCA propertyDrawers
                //in order for the "preloadWardrobeRecipes" prop to properly check if it can load the recipies it gets assigned to it
                //it needs to know that its part of this DCA
                SerializedProperty thisPreloadWardrobeRecipes = serializedObject.FindProperty("preloadWardrobeRecipes");
                Rect pwrCurrentRect = EditorGUILayout.GetControlRect(false, _wardrobePropDrawer.GetPropertyHeight(thisPreloadWardrobeRecipes, GUIContent.none));
                _wardrobePropDrawer.OnGUI(pwrCurrentRect, thisPreloadWardrobeRecipes, new GUIContent(thisPreloadWardrobeRecipes.displayName));
                if (showHelp)
                {
                    EditorGUILayout.HelpBox("Preload Wardrobe: Sets the default wardrobe recipes to use on the Avatar. This is useful when creating specific Avatar prefabs.", MessageType.Info);
                }
                if (_wardrobePropDrawer.changed)
                {
                    serializedObject.ApplyModifiedProperties();
                    if (Application.isPlaying)
                    {
                        thisDCA.ClearSlots();
                        thisDCA.LoadDefaultWardrobe();
                        thisDCA.BuildCharacter(false);
                    }
                    else
                    {
                        GenerateSingleUMA();
                    }
                }
                // *********************************************************************************
                // 
                //NewCharacterColors
                SerializedProperty characterColors = serializedObject.FindProperty("characterColors");
                SerializedProperty newCharacterColors = characterColors.FindPropertyRelative("_colors");
                GUILayout.BeginHorizontal();
                GUILayout.Space(2);
                //for ColorValues as OverlayColorDatas we need to outout something that looks like a list but actully uses a method to add/remove colors because we need the new OverlayColorData to have 3 channels	
                newCharacterColors.isExpanded = EditorGUILayout.Foldout(newCharacterColors.isExpanded, new GUIContent("Character Colors"));
                GUILayout.EndHorizontal();
                var n_origArraySize = newCharacterColors.arraySize;
                var n_newArraySize = n_origArraySize;
                EditorGUI.BeginChangeCheck();
                if (newCharacterColors.isExpanded)
                {
                    var charcol = thisDCA.characterColors._colors;
                    int baseColors = 0;
                    foreach (var c in charcol)
                    {
                        if (c != null)
                        {
                            if (c.isBaseColor)
                            {
                                baseColors++;
                            }
                        }
                    }

                    if (baseColors == 0 && charcol.Count > 0)
                    {
                        foreach (var c in charcol)
                        {
                            if (baseColorNames.Contains(c.name.ToLower()))
                            {
                                c.isBaseColor = true;
                                baseColors++;
                            }
                        }
                    }

                    currentcolorfilter = EditorGUILayout.Popup("Filter Colors", currentcolorfilter, colorfilters);

                    n_newArraySize = EditorGUILayout.DelayedIntField(new GUIContent("Size"), n_origArraySize);
                    EditorGUILayout.Space();
                    EditorGUI.indentLevel++;
                    if (n_origArraySize > 0)
                    {
                        for (int i = 0; i < n_origArraySize; i++)
                        {
                            SerializedProperty currentColor = newCharacterColors.GetArrayElementAtIndex(i);
                            // What a hack. 
                            var col = thisDCA.characterColors._colors[i];
                            if (col == null)
                            {
                                continue;
                            }


                            if (currentcolorfilter == 0)
                            {
                                if (!col.isBaseColor)
                                {
                                    continue;
                                }
                            }
                            //&& !baseColorNames.Contains(currentColor.displayName.ToLower())) continue;
                            if (currentcolorfilter == 2 && currentColor.displayName.ToLower().Contains("colordna"))
                            {
                                continue;
                            }

                            EditorGUILayout.PropertyField(newCharacterColors.GetArrayElementAtIndex(i));
                        }
                    }
                    EditorGUI.indentLevel--;
                }
                if (showHelp)
                {
                    EditorGUILayout.HelpBox("Character Colors: This lets you set predefined colors to be used when building the Avatar. The colors will be assigned to the Shared Colors on the overlays as they are applied to the Avatar.", MessageType.Info);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    if (n_newArraySize != n_origArraySize)
                    {
                        SetNewColorCount(n_newArraySize);//this is not prompting a save so mark the scene dirty...
                        if (!Application.isPlaying)
                        {
                            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        }
                    }
                    serializedObject.ApplyModifiedProperties();
                    if (Application.isPlaying)
                    {
                        thisDCA.UpdateColors(true);
                    }
                    else
                    {
                        GenerateSingleUMA();
                        //thisDCA.UpdateColors(false); // todo: this block is losing all the colors in the recipe somehow...
                        //thisDCA.umaData.isTextureDirty = true;
                        //UpdateUMA();
                    }
                }

                //***********************************************************************************
                // Predefined DNA
                //***********************************************************************************

                // Dropdown of the current DNA.
                // button to "add" it.

                showPrefinedDNA = EditorGUILayout.Foldout(showPrefinedDNA, "Predefined DNA");
                if (showPrefinedDNA)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("keepPredefinedDNA"));
                    if (cachedRace != thisDCA.activeRace.name)
                    {
                        cachedRace = thisDCA.activeRace.name;
                        rawcachedRaceDNA = thisDCA.activeRace.data.GetDNANames().ToArray();
                        List<string> MenuDNA = new List<string>();
                        foreach (string s in rawcachedRaceDNA)
                        {
                            MenuDNA.Add(s.MenuCamelCase());
                        }
                        cachedRaceDNA = MenuDNA.ToArray();
                    }

                    GUILayout.BeginHorizontal();
                    currentDNA = EditorGUILayout.Popup(currentDNA, cachedRaceDNA);
                    if (GUILayout.Button("Add DNA"))
                    {
                        string theDna = rawcachedRaceDNA[currentDNA];

                        if (thisDCA.predefinedDNA == null)
                        {
                            thisDCA.predefinedDNA = new UMAPredefinedDNA();
                        }
                        if (thisDCA.predefinedDNA.ContainsName(theDna))
                        {
                            EditorUtility.DisplayDialog("Error", "Predefined DNA Already contains DNA: " + theDna, "OK");
                        }
                        else
                        {
                            AddSingleDNA(theDna);
                        }
                    }
                    if (GUILayout.Button("Add All"))
                    {
                        foreach (string s in rawcachedRaceDNA)
                        {
                            if (!thisDCA.predefinedDNA.ContainsName(s))
                            {
                                AddSingleDNA(s);
                            }
                        }
                    }
                    if (GUILayout.Button("Clear"))
                    {
                        thisDCA.predefinedDNA.Clear();
                        GenerateSingleUMA();
                        Repaint();
                    }
                    GUILayout.EndHorizontal();

                    if (thisDCA.predefinedDNA != null)
                    {
                        string delme = "";
                        EditorGUI.BeginChangeCheck();
                        foreach (var pd in thisDCA.predefinedDNA.PreloadValues)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(ObjectNames.NicifyVariableName(pd.Name), GUILayout.Width(100));
                            pd.Value = GUILayout.HorizontalSlider(pd.Value, 0.0f, 1.0f);

                            bool delete = GUILayout.Button("\u0078", EditorStyles.miniButton, GUILayout.ExpandWidth(false));
                            if (delete)
                            {
                                delme = pd.Name;
                            }
                            GUILayout.EndHorizontal();
                        }
                        if (!string.IsNullOrEmpty(delme))
                        {
                            thisDCA.predefinedDNA.RemoveDNA(delme);
                            GenerateSingleUMA();
                            Repaint();
                        }
                        if (EditorGUI.EndChangeCheck())
                        {
                            GenerateSingleUMA();
                        }
                    }
                }
                if (showHelp)
                {
                    EditorGUILayout.HelpBox("Predefined DNA is loaded onto the character in the initial character build. Select the DNA in the dropdown, and add it to the list of DNA to load, then edit the values as needed.", MessageType.Info);
                }
                EndVerticalPadded();
            }

            //**************************************
            // End In-Editor customization
            //**************************************


            //the ChangeRaceOptions
            SerializedProperty defaultChangeRaceOptions = serializedObject.FindProperty("defaultChangeRaceOptions");
            defaultChangeRaceOptions.isExpanded = EditorGUILayout.Foldout(defaultChangeRaceOptions.isExpanded, new GUIContent("Race Change Options", "The default options for when the Race is changed. These can be overidden when calling 'ChangeRace' directly."));
            if (defaultChangeRaceOptions.isExpanded)
            {
                BeginVerticalPadded();
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(defaultChangeRaceOptions, GUIContent.none);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cacheCurrentState"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rebuildSkeleton"));
                EditorGUI.indentLevel--;
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                }
                EndVerticalPadded();
            }


            //Move UMAAddidtionalRecipes out of advanced into its own section
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(umaAdditionalRecipes, new GUIContent("Additional Utility Recipes", "Additional Recipes to add when the character is generated, like the capsuleCollider recipe for example"), true);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            GUILayout.Space(2f);

            showAnimatorGUI = EditorGUILayout.Foldout(showAnimatorGUI, "Animator Parameters");
            if (showAnimatorGUI)
            {
                EditorGUI.BeginChangeCheck();
                BeginVerticalPadded();
                ShowAnimatorGUI(thisDCA);
                EndVerticalPadded();
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }

            showBlendshapes = EditorGUILayout.Foldout(showBlendshapes, "Blendshapes");
            if (showBlendshapes)
            {
                EditorGUI.BeginChangeCheck();
                ShowBlendshapesGUI(thisDCA);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }

            GUILayout.Space(2f);
            //Load save fields
            EditorGUI.BeginChangeCheck();
            SerializedProperty loadPathType = serializedObject.FindProperty("loadPathType");
            loadPathType.isExpanded = EditorGUILayout.Foldout(loadPathType.isExpanded, "Legacy Load/Save Options");
            if (loadPathType.isExpanded)
            {
                BeginVerticalPadded();
                SerializedProperty loadString = serializedObject.FindProperty("loadString");
                SerializedProperty loadPath = serializedObject.FindProperty("loadPath");
                SerializedProperty loadFilename = serializedObject.FindProperty("loadFilename");
                SerializedProperty loadFileOnStart = serializedObject.FindProperty("loadFileOnStart");
                SerializedProperty savePathType = serializedObject.FindProperty("savePathType");
                SerializedProperty savePath = serializedObject.FindProperty("savePath");
                SerializedProperty saveFilename = serializedObject.FindProperty("saveFilename");
                //LoadSave Flags
                SerializedProperty defaultLoadOptions = serializedObject.FindProperty("defaultLoadOptions");
                SerializedProperty defaultSaveOptions = serializedObject.FindProperty("defaultSaveOptions");
                //extra LoadSave Options in addition to flags
                //SerializedProperty waitForBundles = serializedObject.FindProperty("waitForBundles");
                SerializedProperty makeUniqueFilename = serializedObject.FindProperty("makeUniqueFilename");
                SerializedProperty ensureSharedColors = serializedObject.FindProperty("ensureSharedColors");

                EditorGUILayout.PropertyField(loadPathType);

                if (loadPathType.enumValueIndex == Convert.ToInt32(DynamicCharacterAvatar.loadPathTypes.String))
                {
                    EditorGUILayout.PropertyField(loadString);
                }
                else
                {
                    if (loadPathType.enumValueIndex <= 1)
                    {
                        EditorGUILayout.PropertyField(loadPath);

                    }
                }

                EditorGUILayout.PropertyField(loadFilename);
                if (loadFilename.stringValue != "")
                {
                    EditorGUILayout.PropertyField(loadFileOnStart);
                }
                EditorGUI.indentLevel++;
                //LoadOptionsFlags
                defaultLoadOptions.isExpanded = EditorGUILayout.Foldout(defaultLoadOptions.isExpanded, new GUIContent("Load Options", "The default options for when a character is loaded from an UMATextRecipe asset or a recipe string. Can be overidden when calling 'LoadFromRecipe' or 'LoadFromString' directly."));
                if (defaultLoadOptions.isExpanded)
                {
                    EditorGUILayout.PropertyField(defaultLoadOptions, GUIContent.none);
                    EditorGUI.indentLevel++;
                    //waitForBundles.boolValue = EditorGUILayout.ToggleLeft(new GUIContent(waitForBundles.displayName, waitForBundles.tooltip), waitForBundles.boolValue);
                    //buildAfterLoad.boolValue = EditorGUILayout.ToggleLeft(new GUIContent(buildAfterLoad.displayName, buildAfterLoad.tooltip), buildAfterLoad.boolValue);
                    //just drawing these as propertyFields because the toolTip on toggle left doesn't work
                    //EditorGUILayout.PropertyField(waitForBundles);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
                if (Application.isPlaying)
                {
                    if (GUILayout.Button("Perform Load"))
                    {
                        thisDCA.DoLoad();
                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(savePathType);
                if (savePathType.enumValueIndex <= 2)
                {
                    EditorGUILayout.PropertyField(savePath);
                }
                EditorGUILayout.PropertyField(saveFilename);
                EditorGUI.indentLevel++;
                defaultSaveOptions.isExpanded = EditorGUILayout.Foldout(defaultSaveOptions.isExpanded, new GUIContent("Legacy Save Options", "The default options for when a character is save to UMATextRecipe asset or a txt. Can be overidden when calling 'DoSave' directly."));
                if (defaultSaveOptions.isExpanded)
                {
                    EditorGUILayout.PropertyField(defaultSaveOptions, GUIContent.none);
                    EditorGUI.indentLevel++;
                    //ensureSharedColors.boolValue = EditorGUILayout.ToggleLeft(new GUIContent(ensureSharedColors.displayName, ensureSharedColors.tooltip), ensureSharedColors.boolValue);
                    //makeUniqueFilename.boolValue = EditorGUILayout.ToggleLeft(new GUIContent(makeUniqueFilename.displayName, makeUniqueFilename.tooltip), makeUniqueFilename.boolValue);
                    //just drawing these as propertyFields because the toolTip on toggle left doesn't work
                    EditorGUILayout.PropertyField(ensureSharedColors);
                    EditorGUILayout.PropertyField(makeUniqueFilename);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
                if (Application.isPlaying)
                {
                    if (GUILayout.Button("Perform Save"))
                    {
                        thisDCA.DoSave();
                    }
                }
                EndVerticalPadded();
                EditorGUILayout.Space();
            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            GUILayout.Space(2f);
            //for CharacterEvents
            EditorGUI.BeginChangeCheck();
            SerializedProperty CharacterCreated = serializedObject.FindProperty("CharacterCreated");
            CharacterCreated.isExpanded = EditorGUILayout.Foldout(CharacterCreated.isExpanded, "Character Events");
            if (CharacterCreated.isExpanded)
            {
                BeginVerticalPadded();
                SerializedProperty CharacterStart = serializedObject.FindProperty("CharacterStart");
                SerializedProperty CharacterBegun = serializedObject.FindProperty("CharacterBegun");
                SerializedProperty CharacterUpdated = serializedObject.FindProperty("CharacterUpdated");
                SerializedProperty CharacterDestroyed = serializedObject.FindProperty("CharacterDestroyed");
                SerializedProperty CharacterDnaUpdated = serializedObject.FindProperty("CharacterDnaUpdated");
                SerializedProperty RecipeUpdated = serializedObject.FindProperty("RecipeUpdated");
                SerializedProperty AnimatorSaved = serializedObject.FindProperty("AnimatorStateSaved");
                SerializedProperty AnimatorRestored = serializedObject.FindProperty("AnimatorStateRestored");
                SerializedProperty WardrobeAdded = serializedObject.FindProperty("WardrobeAdded");
                SerializedProperty WardrobeRemoved = serializedObject.FindProperty("WardrobeRemoved");

                SerializedProperty BuildCharacterBegun = serializedObject.FindProperty("BuildCharacterBegun");
                SerializedProperty SlotsHidden = serializedObject.FindProperty("SlotsHidden");
                SerializedProperty WardrobeSuppressed = serializedObject.FindProperty("WardrobeSuppressed");

                EditorGUILayout.HelpBox("CharacterStart is called in the character Start method, after Initialization, but before auto building.", MessageType.Info);
                EditorGUILayout.PropertyField(CharacterStart);
                EditorGUILayout.HelpBox("CharacterBegun is called when the character is starting the build process", MessageType.Info);
                EditorGUILayout.PropertyField(CharacterBegun);
                EditorGUILayout.HelpBox("CharacterCreated is called after the character has completed generation the first time. It is only called once.", MessageType.Info);
                EditorGUILayout.PropertyField(CharacterCreated);
                EditorGUILayout.HelpBox("CharacterUpdated is called after the character has completed generation. It is called every time the character is generated.", MessageType.Info);
                EditorGUILayout.PropertyField(CharacterUpdated);
                EditorGUILayout.HelpBox("CharacterDestroyed is called when the character is destroyed.", MessageType.Info);
                EditorGUILayout.PropertyField(CharacterDestroyed);
                EditorGUILayout.HelpBox("CharacterDnaUpdated is called during the build process when the character's DNA has been applied.", MessageType.Info);
                EditorGUILayout.PropertyField(CharacterDnaUpdated);

                EditorGUILayout.HelpBox("BuildCharacterBegun is called at the start of BuildCharacter, before the recipes have all been merged.", MessageType.Info);
                EditorGUILayout.PropertyField(BuildCharacterBegun);
                EditorGUILayout.HelpBox("RecipeUpdated is called after the UMAData.UMARecipe has been updated on the character, and it is ready to schedule the build", MessageType.Info);
                EditorGUILayout.PropertyField(RecipeUpdated);
                EditorGUILayout.HelpBox("AnimatorStateSaved is called after the character's animator state has been saved", MessageType.Info);
                EditorGUILayout.PropertyField(AnimatorSaved);
                EditorGUILayout.HelpBox("AnimatorStateRestored is called after the character's animator state has been restored", MessageType.Info);
                EditorGUILayout.PropertyField(AnimatorRestored);
                EditorGUILayout.HelpBox("WardrobeAdded is called after a wardrobe recipe has been added to the character", MessageType.Info);
                EditorGUILayout.PropertyField(WardrobeAdded);
                EditorGUILayout.HelpBox("WardrobeRemoved is called after a wardrobe recipe has been removed from the character", MessageType.Info);
                EditorGUILayout.PropertyField(WardrobeRemoved);
                EditorGUILayout.HelpBox("WardrobeSuppressed is called after recipe generation with a list of all recipes that were suppressed by other items", MessageType.Info);
                EditorGUILayout.PropertyField(WardrobeSuppressed);
                EditorGUILayout.HelpBox("SlotsHidden is called after recipe generation with a list of all slots that were hidden by other items", MessageType.Info);
                EditorGUILayout.PropertyField(SlotsHidden);
                EndVerticalPadded();
            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            GUILayout.Space(2f);
            //for AdvancedOptions
            EditorGUI.BeginChangeCheck();
            context.isExpanded = EditorGUILayout.Foldout(context.isExpanded, "Advanced Options");
            if (context.isExpanded)
            {
                BeginVerticalPadded();
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("alwaysRebuildSkeleton"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("hide"));
#if UMA_ADDRESSABLES
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DelayUnload"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BundleCheck"));
#endif
                EditorGUILayout.PropertyField(serializedObject.FindProperty("forceSlotMaterials"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AtlasResolutionScale"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BoundsOffset"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("markNotReadable"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("markDynamic"));

                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                }
                if (showHelp)
                {
                    EditorGUILayout.HelpBox("Hide: This disables the display of the Avatar without preventing it from being generated. If you want to prevent the character from being generated at all disable the DynamicCharacterAvatar component itself.", MessageType.Info);
                }
                //for _buildCharacterEnabled we want to set the value using the DCS BuildCharacterEnabled property because this actually triggers BuildCharacter
                var buildCharacterEnabled = serializedObject.FindProperty("_buildCharacterEnabled");
                var buildCharacterEnabledValue = buildCharacterEnabled.boolValue;
                EditorGUI.BeginChangeCheck();
                var buildCharacterEnabledNewValue = EditorGUILayout.Toggle(new GUIContent(buildCharacterEnabled.displayName, "Builds the character on recipe load or race changed. If you want to load multiple recipes into a character you can disable this and enable it when you are done. By default this should be true."), buildCharacterEnabledValue);
                if (EditorGUI.EndChangeCheck())
                {
                    if (buildCharacterEnabledNewValue != buildCharacterEnabledValue)
                        thisDCA.BuildCharacterEnabled = buildCharacterEnabledNewValue;
                    serializedObject.ApplyModifiedProperties();
                }
                if (showHelp)
                {
                    EditorGUILayout.HelpBox("Build Character Enabled: Builds the character on recipe load or race changed. If you want to load multiple recipes into a character you can disable this and enable it when you are done. By default this should be true.", MessageType.Info);
                }
                showUMAFramework = EditorGUILayout.Foldout(showUMAFramework, "UMA Framework");
                if (showUMAFramework)
                {
                    EditorGUI.BeginChangeCheck();
                    ShowUMAFramework(context, umaData, umaGenerator);
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                }
                EndVerticalPadded();
            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            GUILayout.Space(2f);



            //for PlaceholderOptions
            EditorGUI.BeginChangeCheck();
            SerializedProperty gizmo = serializedObject.FindProperty("showPlaceholder");
            SerializedProperty enableGizmo = serializedObject.FindProperty("showPlaceholder");
            SerializedProperty previewModel = serializedObject.FindProperty("previewModel");
            SerializedProperty customModel = serializedObject.FindProperty("customModel");
            SerializedProperty customRotation = serializedObject.FindProperty("customRotation");
            SerializedProperty previewColor = serializedObject.FindProperty("previewColor");
            gizmo.isExpanded = EditorGUILayout.Foldout(gizmo.isExpanded, "Placeholder Options");
            if (gizmo.isExpanded)
            {
                BeginVerticalPadded();
                EditorGUILayout.PropertyField(enableGizmo);
                EditorGUILayout.PropertyField(previewModel);
                if (previewModel.enumValueIndex == 2)
                {
                    EditorGUILayout.PropertyField(customModel);
                    EditorGUILayout.PropertyField(customRotation);
                }
                EditorGUILayout.PropertyField(previewColor);
                EndVerticalPadded();
            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }




            if (Application.isPlaying || thisDCA.editorTimeGeneration)
            {
                showWardrobe = EditorGUILayout.Foldout(showWardrobe, "Current Wardrobe");
                if (showWardrobe)
                {
                    string DeleteMe = null;

                    EditorGUI.indentLevel++;
                    Dictionary<string, UMATextRecipe> currentWardrobe = thisDCA.WardrobeRecipes;

                    bool editTimeUpdateNeeded = false;
                    foreach (KeyValuePair<string, UMATextRecipe> item in currentWardrobe)
                    {
						string prepend = "*";
						if(item.Value.disabled)
							prepend = "-";
                        GUILayout.BeginHorizontal();
                        EditorGUI.BeginDisabledGroup(true);
						EditorGUILayout.LabelField(prepend + item.Key, GUILayout.Width(88.0f));
                        EditorGUILayout.TextField(item.Value.DisplayValue + " (" + item.Value.name + ")");
                        EditorGUI.EndDisabledGroup();
                        if (GUILayout.Button("Inspect", EditorStyles.toolbarButton, GUILayout.Width(52)))
                        {
                            InspectorUtlity.InspectTarget(item.Value);
                        }
						if (GUILayout.Button("0/1", EditorStyles.toolbarButton, GUILayout.Width(32))) 
						{
							item.Value.disabled = !item.Value.disabled;
                            if (Application.isPlaying)
                            {
                                thisDCA.BuildCharacter(true);
                            }
                            else
                            {
                                editTimeUpdateNeeded = true;
                            }
						}
                        if (GUILayout.Button("X", EditorStyles.toolbarButton, GUILayout.Width(18)))
                        {
                            DeleteMe = item.Key;
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (editTimeUpdateNeeded)
                    {
                        UpdateCharacter();
                    }

                    if (!string.IsNullOrEmpty(DeleteMe))
                    {
                        currentWardrobe.Remove(DeleteMe);
                        thisDCA.BuildCharacter(true);
                    }

                    GUILayout.Space(10);
                    GUILayout.Label("Additive Recipes");
                    GUILayout.Space(10);
                    Dictionary<string, List<UMATextRecipe>> additiveWardrobe = thisDCA.AdditiveRecipes;

                    foreach (KeyValuePair<string, List<UMATextRecipe>> additem in additiveWardrobe)
                    {
                        foreach (UMATextRecipe item in additem.Value)
                        {
                            GUILayout.BeginHorizontal();
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUILayout.LabelField(additem.Key, GUILayout.Width(88.0f));
                            EditorGUILayout.TextField(item.DisplayValue + " (" + item.name + ")");
                            EditorGUI.EndDisabledGroup();
                            if (GUILayout.Button("Inspect", EditorStyles.toolbarButton, GUILayout.Width(52)))
                            {
                                InspectorUtlity.InspectTarget(item);
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    EditorGUI.indentLevel--;
                }
            }

            List<GameObject> GetRenderers(GameObject parent)
            {
                List<GameObject> objs = new List<GameObject>();

                var renderers = parent.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    Renderer renderer = renderers[i];
                    objs.Add(renderer.gameObject);
                }
                return objs;
            }

            void ShowBlendshapesGUI(DynamicCharacterAvatar thisDCA)
            {
                BeginVerticalPadded();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("loadBlendShapes"));
                // EditorGUILayout.PropertyField(serializedObject.FindProperty("loadOnlyUsedBlendshapes"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("loadBlendshapeNormals"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("loadBlendshapeTangents"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("loadAllFrames"));
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("forceKeepBlendshapes"));
                GUILayout.Space(20);
                GUILayout.EndHorizontal();

                EndVerticalPadded();

            }

            void ShowAnimatorGUI(DynamicCharacterAvatar thisDCA)
            {

                SerializedProperty thisRaceAnimationControllers = serializedObject.FindProperty("raceAnimationControllers");
                Rect racCurrentRect = EditorGUILayout.GetControlRect(false, _animatorPropDrawer.GetPropertyHeight(thisRaceAnimationControllers, GUIContent.none));
                EditorGUI.BeginChangeCheck();

                if (showHelp)
                {
                    EditorGUILayout.HelpBox("Race Animation Controllers: This sets the animation controllers used for each race. When changing the race, the animation controller for the active race will be used by default.", MessageType.Info);
                }

                _animatorPropDrawer.OnGUI(racCurrentRect, thisRaceAnimationControllers, new GUIContent(thisRaceAnimationControllers.displayName));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("keepAvatar"), new GUIContent("Keep Avatar"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("KeepAnimatorController"), new GUIContent("Keep Animator Controller"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rawAvatar"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("forceRebindAnimator"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("RecreateAnimatorOnRaceChange"));


                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    if (Application.isPlaying)
                    {
                        thisDCA.SetExpressionSet();//this triggers any expressions to reset.
                        thisDCA.SetAnimatorController();
                    }
                }
            }

            void GenerateSingleUMA(bool rebuild = false)
            {
                if (Application.isPlaying)
                {
                    thisDCA.BuildCharacter(rebuild);
                    return;
                }

                if (thisDCA.editorTimeGeneration == false)
                {
                    return;
                }

                // Debug.Log("prefab instance asset type: " + PrefabUtility.GetPrefabInstanceStatus(thisDCA.gameObject) + ", asset type: " + PrefabUtility.GetPrefabAssetType(thisDCA.gameObject));

                // Don't generate UMAs from project prefabs or if the gameObject is not active.
                if (!thisDCA.gameObject.activeInHierarchy)//PrefabUtility.GetPrefabInstanceStatus(thisDCA.gameObject) == PrefabInstanceStatus.NotAPrefab && PrefabUtility.GetPrefabAssetType(thisDCA.gameObject) != PrefabAssetType.NotAPrefab)
                {
                    return;
                }

                UMAGenerator ugb = UMAContext.Instance.gameObject.GetComponentInChildren<UMAGenerator>();
                if (ugb == null)
                {
                    EditorUtility.DisplayDialog("Error", "Cannot find generator!", "OK");
                }
                else
                {

                    DynamicCharacterAvatar dca = target as DynamicCharacterAvatar;

                    if (dca.umaData != null)
                    {
                        dca.umaData.SaveMountedItems();
                    }
                    CleanupGeneratedData(rebuild,false);

                    dca.activeRace.SetRaceData();
                    if (dca.activeRace.racedata == null)
                    {
                        return;
                    }

                    dca.LoadDefaultWardrobe();

                    // save the predefined DNA...
                    var dna = dca.predefinedDNA.Clone();
                    dca.BuildCharacter(false, true);
                    dca.predefinedDNA = dna;

                    bool oldFastGen = ugb.fastGeneration;
                    int oldScaleFactor = ugb.InitialScaleFactor;
                    int oldAtlasResolution = ugb.atlasResolution;

                    ugb.FreezeTime = true;
                    ugb.fastGeneration = true;
                    ugb.InitialScaleFactor = ugb.editorInitialScaleFactor;
                    ugb.atlasResolution = ugb.editorAtlasResolution;


                    dca.activeRace.racedata.ResetDNA();

                    ugb.GenerateSingleUMA(dca.umaData, false);

                    ugb.fastGeneration = oldFastGen;
                    ugb.FreezeTime = false;
                    ugb.InitialScaleFactor = oldScaleFactor;
                    ugb.atlasResolution = oldAtlasResolution;

                    var mountedItems = dca.gameObject.GetComponentsInChildren<UMAMountedItem>();
                    for (int i = 0; i < mountedItems.Length; i++)
                    {
                        UMAMountedItem mi = mountedItems[i];
                        mi.ResetMountPoint();
                    }
                    dca.umaData.RestoreSavedItems();
                }
            }

            void CleanupGeneratedData(bool clear, bool killUMAData=true)
            {
                if (Application.isPlaying)
                {
                    return;
                }

                List<GameObject> Cleaners = GetRenderers(thisDCA.gameObject);
                thisDCA.Hide(clear);
                for (int i = 0; i < Cleaners.Count; i++)
                {
                    GameObject go = Cleaners[i];
                    DestroyImmediate(go);
                }
                if (killUMAData)
                {
                    DestroyImmediate(thisDCA.umaData);
                    thisDCA.umaData = null;
                }
                thisDCA.ClearSlots();
            }

            void UpdateCharacter()
            {
                if (thisDCA.gameObject.scene != default)
                {
                    if (thisDCA.editorTimeGeneration)
                    {
                        GenerateSingleUMA();
                    }
                    else
                    {
                        CleanupGeneratedData(true);
                    }
                }
            }
        }

        private void ShowUMAFramework(SerializedProperty context, SerializedProperty umaData, SerializedProperty umaGenerator)
        {
            EditorGUILayout.PropertyField(context);
            EditorGUILayout.PropertyField(umaData);
            EditorGUILayout.PropertyField(umaGenerator);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(animationController);
        }

        private void AddSingleDNA(string theDna)
        {
            float value = 0.5f;

            if (thisDCA.umaData != null)
            {
                var characterDNA = thisDCA.GetDNA();
                if (characterDNA != null)
                {
                    if (characterDNA.ContainsKey(theDna))
                    {
                        value = characterDNA[theDna].Value;
                    }
                }
            }
            thisDCA.predefinedDNA.AddDNA(theDna, value);
        }
    }
}
