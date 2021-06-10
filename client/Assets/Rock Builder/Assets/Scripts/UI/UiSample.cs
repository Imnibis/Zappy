using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using System.IO;
using RockBuilder;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          RockBuilderWindow
    ///   Description:    The UI of the RockBuilder tool.
    ///   Author:         Kim Hunkeler                   
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class RockBuilderWindow : EditorWindow
    {
        private GUIStyle guiStyle = new GUIStyle();
        private GUIStyle infoText = new GUIStyle();
        private GUIStyle guiColor = new GUIStyle();
        int toolbarInt = 0;
        string[] toolbarStrings = { "Rocks", "Gemstones", "Help" };

        // Rocks Parameter
        string firstParameterRocks = "Rock_01"; // Name of the object
        string secondParameterRocks = ""; // Shape selection
        // Activate again for the custom part.
        // int thirdParameterRocks = 0; // Number of points for the own shape
        int fourthParamaterRocks = 1; // Divider
        bool fifthParamaterRocks = false; // Smooth
        bool sixthParamaterRocks = false; // Collider
        int seventhParamaterRocks = 0; // LODs
        string eightParameterRocks = ""; // Rock rather round or squared
        int ninthParamaterRocks = 6; // Edges 
        float rockHeight = 1.0f; // Height of the rock
        float rockWidth = 1.0f; // Width of the rock
        float rockDepth = 1.0f; // Deepth of the rock
        float rockNoise = 0f; // Displacement of all axes of the individual vertices
        float rockBevelSize = 0.1f; // Size of the edges
        private Material rockMaterial; // Material

        // Gemstones Parameter
        string firstParameterGemstones = "Gemstone_01"; // Name of the object
        string secondParameterGemstones = ""; // Shape
        int thirdParamaterGemstones = 3; // Vertices
        float fourthParamaterGemstones = 1.0f; // Radius
        float fifthParamaterGemstones = 1.0f; // Height
        float sixthParamaterGemstones = 0.5f; // Peak Height
        bool seventhParameterGemstones = false; // Smooth
        int eightParamaterGemstones = 0; // LODs
        bool ninthParameterGemstones = false; // Collider
        private Material gemstoneMaterial; // Material

        // Objectreference for a cube rock
        CubeRock cubeRock;

        // Objectreference for a sphere rock
        SphereRock sphereRock;

        // Objectreference for a custom rock
        CustomRock customRock;

        // Objectreference for a crystal
        Crystal crystal;

        // Objectreference for a gem
        Gem gem;

        // Objectreference for a diamond
        Diamond diamond;

        [MenuItem("Tools/RockBuilder")]

        public static void ShowWindow()
        {
            GetWindow<RockBuilderWindow>("Rock Builder");
        }

        void OnGUI()
        {

            // UI Code for all the rock builder tabs (rocks, gemstones and help)
            toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);

            // UI Part "Rocks"
            if (toolbarInt == 0)
            {
                GUILayout.Space(5);

                guiStyle.fontSize = 20;
                guiStyle.margin = new RectOffset(3, 0, 0, 0);
                guiColor.normal.textColor = Color.black;
                GUILayout.Label("Rock Builder - Rocks", guiStyle);

                GUILayout.Space(20);

                // First rocks parameter => The name of the generated object           
                firstParameterRocks = EditorGUILayout.TextField("Object Name", firstParameterRocks);

                GUILayout.Space(15);

                // Selection of the rock shape     
                GUILayout.Label("Choose Shape");

                GUILayout.Space(5);

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField(secondParameterRocks);
                EditorGUI.EndDisabledGroup();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                //GUILayout.Box(LoadPNG("Assets/Rock Builder/Assets/Images/Gem_Icon_2.png"), new GUILayoutOption[] { GUILayout.Width(30), GUILayout.Height(30) });
                if (GUILayout.Button("Standard", GUILayout.Height(32)))
                {
                    secondParameterRocks = "Standard";
                }
                //GUILayout.Box(LoadPNG("Assets/Rock Builder/Assets/Images/Crystal_icon.png"), new GUILayoutOption[] { GUILayout.Width(30), GUILayout.Height(30) });
                if (GUILayout.Button("   Custom   ", GUILayout.Height(32)))
                {
                    secondParameterRocks = "Custom";
                    // customRock = CustomRockService.Instance.CreateEmptyCustomRock(firstParameterRocks);
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(15);

                // The second part of rocks is only shown, when a shape has been selected
                if (secondParameterRocks != "" && secondParameterRocks == "Standard")
                {
                    UpdateRocks();

                    // Selection of the stone shape (round or square)     
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(eightParameterRocks);
                    EditorGUI.EndDisabledGroup();

                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();
                    GUILayout.Box(LoadPNG("Assets/Rock Builder/Assets/Images/Rounded_Icon.png"), new GUILayoutOption[] { GUILayout.Width(30), GUILayout.Height(30) });
                    if (GUILayout.Button("Rounded", GUILayout.Height(32)))
                    {
                        sphereRock = SphereRockService.Instance.CreateEmptySphereRock(firstParameterRocks);
                        eightParameterRocks = "Rounded";
                    }
                    GUILayout.Box(LoadPNG("Assets/Rock Builder/Assets/Images/Squared_icon.png"), new GUILayoutOption[] { GUILayout.Width(30), GUILayout.Height(30) });
                    if (GUILayout.Button("   Squared   ", GUILayout.Height(32)))
                    {
                        cubeRock = CubeRockService.Instance.CreateEmptyCubeRock(firstParameterRocks);
                        eightParameterRocks = "Squared";
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);

                    // Displays the other rocks parameter just, if rounded or squared was selected
                    if (eightParameterRocks != "" && (cubeRock != null || sphereRock != null))
                    {
                        // Edges just for the rounded stones
                        if (eightParameterRocks == "Rounded")
                        {
                            // Ninth rocks parameter => Slidebar for the desired edge count between 6 and 100 
                            ninthParamaterRocks = EditorGUILayout.IntSlider("Edges", ninthParamaterRocks, 6, 100);
                        }

                        // Divider just for the squared stones
                        if (eightParameterRocks == "Squared")
                        {
                            // Fourth rocks parameter => Slidebar for the divider between 1 and 50
                            fourthParamaterRocks = EditorGUILayout.IntSlider("Divider", fourthParamaterRocks, 1, 50);
                        }

                        // Restricts the user inputs for the rock height => 0.01 - 1000
                        if (rockHeight < 0.009 || rockHeight > 1000)
                        {
                            rockHeight = 1.0f;
                        }
                        rockHeight = EditorGUILayout.FloatField("Height", rockHeight);

                        // Restricts the user inputs for the rock width => 0.01 - 1000
                        if (rockWidth < 0.009 || rockWidth > 1000)
                        {
                            rockWidth = 1.0f;
                        }
                        rockWidth = EditorGUILayout.FloatField("Width", rockWidth);

                        // Restricts the user inputs for the rock depth => 0.01 - 1000
                        if (rockDepth < 0.009 || rockDepth > 1000)
                        {
                            rockDepth = 1.0f;
                        }
                        rockDepth = EditorGUILayout.FloatField("Depth", rockDepth);

                        // Bevel size just for the squared stones
                        if (eightParameterRocks == "Squared")
                        {
                            // Restricts the user inputs for the bevel size => 0.01 - 1000
                            if (rockBevelSize < 0 || rockBevelSize > 1000)
                            {
                                rockBevelSize = 0f;
                            }
                            rockBevelSize = EditorGUILayout.FloatField("Bevel Size", rockBevelSize);
                        }

                        // Restricts the user inputs for the noise => 0 - 1000
                        if (rockNoise < 0 || rockNoise > 1000)
                        {
                            rockNoise = 0.1f;
                        }
                        rockNoise = EditorGUILayout.FloatField("Noise", rockNoise);

                        // Fifth rocks parameter => The checkbox to smooth an object        
                        fifthParamaterRocks = EditorGUILayout.Toggle("Smooth", fifthParamaterRocks);

                        // Sixth rocks parameter => The checkbox to give the object a collider        
                        sixthParamaterRocks = EditorGUILayout.Toggle("Collider", sixthParamaterRocks);

                        // Seventh rocks parameter => Slidebar for the number of LODs
                        seventhParamaterRocks = EditorGUILayout.IntSlider("LODs", seventhParamaterRocks, 0, 3);

                        GUILayout.Space(15);

                        // Selection of the rock materials     
                        GUILayout.Label("Choose Material");

                        GUILayout.Space(5);

                        rockMaterial = (Material)EditorGUILayout.ObjectField(rockMaterial, typeof(Material), true);

                        GUILayout.Space(15);

                        // Button to generate the rock
                        if (GUILayout.Button("Let's rock!", GUILayout.Height(25)))
                        {
                            // Generate existing mesh if diamondgenerator exists, otherwise create a new diamond generator
                            if (cubeRock)
                            {
                                cubeRock = CubeRockService.Instance.CreateCubeRock(cubeRock, rockMaterial);
                            }
                            if (sphereRock)
                            {
                                sphereRock = SphereRockService.Instance.CreateSphereRock(sphereRock, rockMaterial);
                            }
                        }
                    }
                }

                // This is the rock part, if the user wants to create his own shape
                if (secondParameterRocks != "" && secondParameterRocks != "Standard")
                {

                    EditorGUILayout.HelpBox("In the first release, the custom shape is not available yet. This feature will follow as soon as possible.", MessageType.Info);

                    // // NOT in the first release!
                    

                    // // Update UI depending on the selected custom rock
                    // UpdateRocks();

                    // // Third rocks parameter => Button for the number of points to create your own stone shape
                    // if (GUILayout.Button("Add Point", GUILayout.Height(25)))
                    // {
                    //     thirdParameterRocks = thirdParameterRocks + 1;
                    // }

                    // GUILayout.Space(15);

                    // // Fourth rocks parameter => Slidebar for the desired polycount between 10 and 10'000
                    //     fourthParamaterRocks = EditorGUILayout.IntSlider("Polycount", fourthParamaterRocks, 10, 10000);

                    // // Fifth rocks parameter => The checkbox to smooth an object         
                    // fifthParamaterRocks = EditorGUILayout.Toggle("Smooth", fifthParamaterRocks);

                    // // Sixth rocks parameter => The checkbox to give the object a collider         
                    // sixthParamaterRocks = EditorGUILayout.Toggle("Collider", sixthParamaterRocks);

                    // // Seventh rocks parameter => Slidebar for the number of LODs
                    // seventhParamaterRocks = EditorGUILayout.IntSlider("LODs", seventhParamaterRocks, 0, 3);

                    // GUILayout.Space(15);

                    // // Selection of the rock materials      
                    // GUILayout.Label("Choose Material");

                    // GUILayout.Space(5);

                    // rockMaterial = (Material)EditorGUILayout.ObjectField(rockMaterial, typeof(Material), true);

                    // GUILayout.Space(15);

                    // // Button to generate the rock
                    // if (GUILayout.Button("Let's rock!", GUILayout.Height(25)))
                    // {
                    //     // Generate existing mesh if diamondgenerator exists, otherwise create a new diamond generator
                    //     if (customRock)
                    //     {
                    //         customRock = CustomRockService.Instance.CreateCustomRock(customRock, rockMaterial);
                    //     }
                    // }

                

                }
            }

            // UI Part "Gemstones"
            if (toolbarInt == 1)
            {
                GUILayout.Space(5);

                guiStyle.fontSize = 20;
                guiColor.normal.textColor = Color.black;
                GUILayout.Label("Rock Builder - Gemstones", guiStyle);

                GUILayout.Space(20);

                // First gemstones parameter => The name of the generated object           
                firstParameterGemstones = EditorGUILayout.TextField("Object Name", firstParameterGemstones);

                GUILayout.Space(15);

                // Selection of the gemstone shape     
                GUILayout.Label("Choose Shape");

                GUILayout.Space(5);

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField(secondParameterGemstones);
                EditorGUI.EndDisabledGroup();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.Box(LoadPNG("Assets/Rock Builder/Assets/Images/Crystal_icon.png"), new GUILayoutOption[] { GUILayout.Width(30), GUILayout.Height(30) });
                if (GUILayout.Button(" Crystal ", GUILayout.Height(32)) && crystal == null)
                {
                    secondParameterGemstones = "Crystal";
                    crystal = CrystalService.Instance.CreateEmptyCrystal(firstParameterGemstones);
                }
                GUILayout.Box(LoadPNG("Assets/Rock Builder/Assets/Images/Gem_Icon_2.png"), new GUILayoutOption[] { GUILayout.Width(30), GUILayout.Height(30) });
                if (GUILayout.Button("   Gem   ", GUILayout.Height(32)))
                {
                    secondParameterGemstones = "Gem";
                    gem = GemService.Instance.CreateEmptyGem(firstParameterGemstones);
                }
                GUILayout.Box(LoadPNG("Assets/Rock Builder/Assets/Images/Diamond_icon.png"), new GUILayoutOption[] { GUILayout.Width(30), GUILayout.Height(30) });
                if (GUILayout.Button("Diamond", GUILayout.Height(32)) && diamond == null)
                {
                    secondParameterGemstones = "Diamond";
                    diamond = DiamondService.Instance.CreateEmptyDiamond(firstParameterGemstones);
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(15);

                // The second part of gemstones is only shown, when a shape has been selected
                if (secondParameterGemstones != "" && (crystal != null || diamond != null || gem != null))
                {
                    UpdateGemStones();

                    // Third gemstones parameter => Slidebar for the number of vertices
                    if (secondParameterGemstones == "Crystal")
                        thirdParamaterGemstones = EditorGUILayout.IntSlider("Edges", thirdParamaterGemstones, 3, 200);
                    if (secondParameterGemstones == "Diamond" || secondParameterGemstones == "Gem")
                    {
                        // The number of the diamond and gem edges has to be even
                        thirdParamaterGemstones = EditorGUILayout.IntSlider("Edges", thirdParamaterGemstones, 6, 200);
                        if (thirdParamaterGemstones % 2 != 0)
                        {
                            thirdParamaterGemstones = thirdParamaterGemstones + 1;
                        }
                    }

                    // Restricts the user inputs for the radius => 0.01 - 1000
                    if (fourthParamaterGemstones < 0.009 || fourthParamaterGemstones > 1000)
                    {
                        fourthParamaterGemstones = 1.0f;
                    }
                    // Fourth gemstones parameter => Radius of the object
                    if (secondParameterGemstones != "Gem")
                        fourthParamaterGemstones = EditorGUILayout.FloatField("Radius", fourthParamaterGemstones);
                    else
                        fourthParamaterGemstones = EditorGUILayout.FloatField("Height", fourthParamaterGemstones);

                    // Restricts the user inputs for the crystal peak height => 0.01 - 1000
                    if (sixthParamaterGemstones < 0.009 || sixthParamaterGemstones > 1000)
                    {
                        sixthParamaterGemstones = 0.5f;
                    }
                    // Sixth gemstones parameter => Crown or peak height 
                    if (secondParameterGemstones == "Crystal")
                        sixthParamaterGemstones = EditorGUILayout.FloatField("Peak Height", sixthParamaterGemstones);
                    if (secondParameterGemstones == "Gem")
                        sixthParamaterGemstones = EditorGUILayout.FloatField("Width", sixthParamaterGemstones);
                    if (secondParameterGemstones == "Diamond")
                        sixthParamaterGemstones = EditorGUILayout.FloatField("Crown Height", sixthParamaterGemstones);

                    // Restricts the user inputs for the height => 0.01 - 1000
                    if (fifthParamaterGemstones < 0.009 || fifthParamaterGemstones > 1000)
                    {
                        fifthParamaterGemstones = 1.0f;
                    }
                    // Fifth gemstones parameter => Body height if crystal or diamond / Depth if gem
                    if (secondParameterGemstones != "Gem")
                        fifthParamaterGemstones = EditorGUILayout.FloatField("Body Height", fifthParamaterGemstones);
                    else
                        fifthParamaterGemstones = EditorGUILayout.FloatField("Depth", fifthParamaterGemstones);

                    // Seventh gemstones parameter => The checkbox to smooth an object         
                    seventhParameterGemstones = EditorGUILayout.Toggle("Smooth", seventhParameterGemstones);

                    // Ninth gemstones parameter => The checkbox to give the object a collider         
                    ninthParameterGemstones = EditorGUILayout.Toggle("Collider", ninthParameterGemstones);

                    // Eight gemstones parameter => Slidebar for the number of LODs  
                    eightParamaterGemstones = EditorGUILayout.IntSlider("LODs", eightParamaterGemstones, 0, 3);

                    GUILayout.Space(15);

                    // Selection of the gemstone materials      
                    GUILayout.Label("Choose Material");

                    GUILayout.Space(5);

                    gemstoneMaterial = (Material)EditorGUILayout.ObjectField(gemstoneMaterial, typeof(Material), true);

                    GUILayout.Space(15);

                    // Button to generate the gemstone
                    if (GUILayout.Button("Let's rock!", GUILayout.Height(25)))
                    {

                        // Generate existing mesh if diamondgenerator exists, otherwise create a new diamond generator
                        if (crystal)
                        {
                            crystal = CrystalService.Instance.CreateCrystal(crystal, gemstoneMaterial);
                        }
                        if (gem)
                        {
                            gem = GemService.Instance.CreateGem(gem, gemstoneMaterial);
                        }
                        if (diamond)
                        {
                            diamond = DiamondService.Instance.CreateDiamond(diamond, gemstoneMaterial);
                        }
                    }
                }
            }

            // UI Part "Help"
            if (toolbarInt == 2)
            {
                GUILayout.Space(5);

                infoText.wordWrap = true;
                infoText.margin = new RectOffset(5, 5, 0, 0);
                guiStyle.fontSize = 20;
                GUILayout.Label("Rock Builder - Help", guiStyle);

                GUILayout.Space(15);

                EditorGUILayout.HelpBox("Rock Builder is a simple tool to create rocks and gemstones with different shaders. It's made by two students from SAE Zurich as their bachelor thesis.", MessageType.Info);

                GUILayout.Space(15);

                if (GUILayout.Button("Open User Manual", GUILayout.Height(25)))
                {
                    // Opens the user manual PDF file
                    Application.OpenURL(System.Environment.CurrentDirectory + "/Assets/Rock Builder/Documentation/UserManual_RockBuilder.pdf");
                }

                GUILayout.Space(20);

                GUILayout.Label("Contact", guiStyle);

                GUILayout.Space(15);

                GUILayout.Label("Please send us an email if you have any questions or if you have problems with the tool. We will do our best to answer the email as fast as possible. Thank you!", infoText);

                GUILayout.Space(15);

                // The mail mask opens with a click
                if (GUILayout.Button("Email: rockbuilder-help@hotmail.com", EditorStyles.label))
                    Application.OpenURL("mailto:rockbuilder-help@hotmail.com?");
            }
        }

        private void Update()
        {
            CheckIfCrystalSelected();
            CheckIfGemSelected();
            CheckIfDiamondSelected();
            CheckIfCubeRockSelected();
            CheckIfSphereRockSelected();
            CheckIfCustomRockSelected();
        }

        // Required to display PNG files on the UI
        private Texture2D LoadPNG(string filePath)
        {

            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); // This will auto-resize the texture dimensions
            }
            return tex;
        }

        private void CheckIfCrystalSelected()
        {
            if (crystal == null)
            {
                crystal = CrystalService.Instance.GetCrystalFromSelection();
                if (crystal != null)
                {
                    toolbarInt = 1;
                    firstParameterGemstones = crystal.name;
                    secondParameterGemstones = "Crystal";
                    thirdParamaterGemstones = crystal.edges;
                    fourthParamaterGemstones = crystal.radius;
                    fifthParamaterGemstones = crystal.height;
                    sixthParamaterGemstones = crystal.heightPeak;
                    seventhParameterGemstones = crystal.smoothFlag;
                    eightParamaterGemstones = crystal.lodCount;
                    ninthParameterGemstones = crystal.colliderFlag;
                    if (crystal.GetComponent<MeshRenderer>().sharedMaterial != null)
                    {
                        gemstoneMaterial = crystal.GetComponent<MeshRenderer>().sharedMaterial;
                    }
                    this.Repaint();
                }
            }
            else
            {
                if (crystal != CrystalService.Instance.GetCrystalFromSelection())
                {
                    crystal = null;
                    this.Repaint();
                }
            }
        }

        private void CheckIfGemSelected()
        {
            if (gem == null)
            {
                gem = GemService.Instance.GetGemFromSelection();
                if (gem != null)
                {
                    toolbarInt = 1;
                    firstParameterGemstones = gem.name;
                    secondParameterGemstones = "Gem";
                    thirdParamaterGemstones = gem.edges;
                    sixthParamaterGemstones = gem.width;
                    fourthParamaterGemstones = gem.height;
                    fifthParamaterGemstones = gem.depth;
                    seventhParameterGemstones = gem.smoothFlag;
                    eightParamaterGemstones = gem.lodCount;
                    ninthParameterGemstones = gem.colliderFlag;
                    if (gem.GetComponent<MeshRenderer>().sharedMaterial != null)
                    {
                        gemstoneMaterial = gem.GetComponent<MeshRenderer>().sharedMaterial;
                    }
                    this.Repaint();
                }
            }
            else
            {
                if (gem != GemService.Instance.GetGemFromSelection())
                {
                    gem = null;
                    this.Repaint();
                }
            }
        }

        private void CheckIfDiamondSelected()
        {
            if (diamond == null)
            {
                diamond = DiamondService.Instance.GetDiamondFromSelection();
                if (diamond != null)
                {
                    toolbarInt = 1;
                    firstParameterGemstones = diamond.name;
                    secondParameterGemstones = "Diamond";
                    thirdParamaterGemstones = diamond.edges;
                    fourthParamaterGemstones = diamond.radius;
                    fifthParamaterGemstones = diamond.pavillonHeight;
                    sixthParamaterGemstones = diamond.crownHeight;
                    seventhParameterGemstones = diamond.smoothFlag;
                    eightParamaterGemstones = diamond.lodCount;
                    ninthParameterGemstones = diamond.colliderFlag;
                    if (diamond.GetComponent<MeshRenderer>().sharedMaterial != null)
                    {
                        gemstoneMaterial = diamond.GetComponent<MeshRenderer>().sharedMaterial;
                    }
                    this.Repaint();
                }
            }
            else
            {
                if (diamond != DiamondService.Instance.GetDiamondFromSelection())
                {
                    diamond = null;
                    this.Repaint();
                }
            }
        }

        private void CheckIfCubeRockSelected()
        {
            if (cubeRock == null)
            {
                cubeRock = CubeRockService.Instance.GetCubeRockFromSelection();
                if (cubeRock != null)
                {
                    toolbarInt = 0;
                    firstParameterRocks = cubeRock.name;
                    secondParameterRocks = "Standard";
                    rockHeight = cubeRock.height;
                    rockWidth = cubeRock.width;
                    rockDepth = cubeRock.depth;
                    rockNoise = cubeRock.noise;
                    rockBevelSize = cubeRock.bevelSize;
                    fourthParamaterRocks = cubeRock.divider;
                    seventhParamaterRocks = cubeRock.lodCount;
                    sixthParamaterRocks = cubeRock.colliderFlag;
                    eightParameterRocks = "Squared";
                    if (cubeRock.GetComponent<MeshRenderer>().sharedMaterial != null)
                    {
                        rockMaterial = cubeRock.GetComponent<MeshRenderer>().sharedMaterial;
                    }
                    this.Repaint();
                }
            }
            else
            {
                if (cubeRock != CubeRockService.Instance.GetCubeRockFromSelection())
                {
                    cubeRock = null;
                    this.Repaint();
                }
            }
        }

        private void CheckIfSphereRockSelected()
        {
            if (sphereRock == null)
            {
                sphereRock = SphereRockService.Instance.GetSphereRockFromSelection();
                if (sphereRock != null)
                {
                    toolbarInt = 0;
                    firstParameterRocks = sphereRock.name;
                    secondParameterRocks = "Standard";
                    rockHeight = sphereRock.height;
                    rockWidth = sphereRock.width;
                    rockDepth = sphereRock.depth;
                    rockNoise = sphereRock.noise;
                    fifthParamaterRocks = sphereRock.smoothFlag;
                    seventhParamaterRocks = sphereRock.lodCount;
                    sixthParamaterRocks = sphereRock.colliderFlag;
                    eightParameterRocks = "Rounded";
                    ninthParamaterRocks = sphereRock.edges;
                    if (sphereRock.GetComponent<MeshRenderer>().sharedMaterial != null)
                    {
                        rockMaterial = sphereRock.GetComponent<MeshRenderer>().sharedMaterial;
                    }
                    this.Repaint();
                }
            }
            else
            {
                if (sphereRock != SphereRockService.Instance.GetSphereRockFromSelection())
                {
                    sphereRock = null;
                    this.Repaint();
                }
            }
        }
        private void CheckIfCustomRockSelected()
        {
            if (customRock == null)
            {
                customRock = CustomRockService.Instance.GetCustomRockFromSelection();
                if (customRock != null)
                {
                    toolbarInt = 0;
                    firstParameterRocks = customRock.name;
                    secondParameterRocks = "Custom";
                    fifthParamaterRocks = customRock.smoothFlag;
                    seventhParamaterRocks = customRock.lodCount;
                    sixthParamaterRocks = customRock.colliderFlag;
                    if (customRock.GetComponent<MeshRenderer>().sharedMaterial != null)
                    {
                        rockMaterial = customRock.GetComponent<MeshRenderer>().sharedMaterial;
                    }
                    this.Repaint();
                }
            }
            else
            {
                if (customRock != CustomRockService.Instance.GetCustomRockFromSelection())
                {
                    customRock = null;
                    this.Repaint();
                }
            }
        }

        private void UpdateGemStones()
        {
            if (crystal != null)
            {
                crystal.edges = thirdParamaterGemstones;
                crystal.radius = fourthParamaterGemstones;
                crystal.height = fifthParamaterGemstones;
                crystal.heightPeak = sixthParamaterGemstones;
                crystal.smoothFlag = seventhParameterGemstones;
                crystal.lodCount = eightParamaterGemstones;
                crystal.colliderFlag = ninthParameterGemstones;
            }
            if (gem != null)
            {
                gem.edges = thirdParamaterGemstones;
                gem.width = sixthParamaterGemstones;
                gem.height = fourthParamaterGemstones;
                gem.depth = fifthParamaterGemstones;
                gem.smoothFlag = seventhParameterGemstones;
                gem.lodCount = eightParamaterGemstones;
                gem.colliderFlag = ninthParameterGemstones;
            }
            if (diamond != null)
            {
                diamond.edges = thirdParamaterGemstones;
                diamond.radius = fourthParamaterGemstones;
                diamond.pavillonHeight = fifthParamaterGemstones;
                diamond.crownHeight = sixthParamaterGemstones;
                diamond.smoothFlag = seventhParameterGemstones;
                diamond.lodCount = eightParamaterGemstones;
                diamond.colliderFlag = ninthParameterGemstones;
            }
        }

        private void UpdateRocks()
        {
            if (cubeRock != null)
            {
                cubeRock.smoothFlag = fifthParamaterRocks;
                cubeRock.lodCount = seventhParamaterRocks;
                cubeRock.height = rockHeight;
                cubeRock.width = rockWidth;
                cubeRock.depth = rockDepth;
                cubeRock.noise = rockNoise;
                cubeRock.bevelSize = rockBevelSize;
                cubeRock.divider = fourthParamaterRocks;
                cubeRock.colliderFlag = sixthParamaterRocks;
            }

            if (sphereRock != null)
            {
                sphereRock.smoothFlag = fifthParamaterRocks;
                sphereRock.lodCount = seventhParamaterRocks;
                sphereRock.edges = ninthParamaterRocks;
                sphereRock.height = rockHeight;
                sphereRock.width = rockWidth;
                sphereRock.depth = rockDepth;
                sphereRock.noise = rockNoise;
                sphereRock.colliderFlag = sixthParamaterRocks;
            }

            if (customRock != null)
            {
                customRock.smoothFlag = fifthParamaterRocks;
                customRock.lodCount = seventhParamaterRocks;
                customRock.colliderFlag = sixthParamaterRocks;
            }
        }

        private void OnDestroy()
        {
            if (crystal && crystal.mesh == null)
            {
                DestroyImmediate(crystal.gameObject);
            }

            if (gem && gem.mesh == null)
            {
                DestroyImmediate(gem.gameObject);
            }

            if (diamond && diamond.mesh == null)
            {
                DestroyImmediate(diamond.gameObject);
            }

            if (cubeRock && cubeRock.mesh == null)
            {
                DestroyImmediate(cubeRock.gameObject);
            }

            if (sphereRock && sphereRock.mesh == null)
            {
                DestroyImmediate(sphereRock.gameObject);
            }

            if (customRock && customRock.mesh == null)
            {
                DestroyImmediate(customRock.gameObject);
            }
        }
    }
}