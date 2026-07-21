using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using Re4QuadExtremeEditor.src.Class.Enums;

namespace Re4QuadExtremeEditor.src
{
    /// <summary>
    /// Representa todos os status (configurações/opções) do programa;
    /// </summary>
    public static class Globals
    {

        #region Configs

        // diretorio dos arquivos pmds das rooms.
        public static string xscrDiretory = @"xscr\";
        public static string DirectoryUHDRE4 = "";

        // diretorio de todos os objetos, itens, etcmodel, inimigos.
        public static string xfileDiretory = @"xfile\";

        // a cor do ceu
        public static Color SkyColor = Color.Azure;

        // float
        public static ConfigFrationalSymbol FrationalSymbol = ConfigFrationalSymbol.AcceptsCommaAndPeriod_OutputPeriod;
        public static int FrationalAmount = 9;

        // itens rotations options

        public static bool ItemDisableRotationAll = false;
        public static bool ItemDisableRotationIfXorYorZequalZero = false;
        public static bool ItemDisableRotationIfZisNotGreaterThanZero = true;
        public static ObjRotationOrder ItemRotationOrder = ObjRotationOrder.RotationXY;
        public static float ItemRotationCalculationMultiplier = 1;
        public static float ItemRotationCalculationDivider = 1;

        #endregion

        #region SelectRoomForm last selection (kept for the current app session only)

        // remembers the user's last choice in SelectRoomForm so re-opening it doesn't reset the combo boxes
        public static RoomModelLoadSource LastSelectedModelSource = RoomModelLoadSource.None;
        public static LoadRoomDllVersion LastSelectedDllVersion = LoadRoomDllVersion.Qingsheng;

        #endregion

        #region Colors

        // cores
        public static Color NodeColorESL = Color.FromArgb(192, 0, 0);
        public static Color NodeColorETS = Color.Maroon;
        public static Color NodeColorITA = Color.FromArgb(0, 0, 192);
        public static Color NodeColorAEV = Color.FromArgb(0, 192, 0);
        public static Color NodeColorEXTRAS = Color.FromArgb(0x0062707E);
        public static Color NodeColorHided = Color.DarkGray;
        public static Color NodeColorEntry = Color.FromArgb(0x00FFFFFF);
        public static Color NodeColorDSE = Color.FromArgb(120, 120, 120);
        public static Color NodeColorFSE = Color.FromArgb(200, 110, 0);
        public static Color NodeColorEFF_Table0 = Color.FromArgb(150, 90, 200);
        public static Color NodeColorEFF_Table1 = Color.FromArgb(150, 90, 200);
        public static Color NodeColorEFF_Table2 = Color.FromArgb(150, 90, 200);
        public static Color NodeColorEFF_Table3 = Color.FromArgb(150, 90, 200);
        public static Color NodeColorEFF_Table4 = Color.FromArgb(150, 90, 200);
        public static Color NodeColorEFF_Table6 = Color.FromArgb(150, 90, 200);
        public static Color NodeColorEFF_Table7 = Color.FromArgb(180, 60, 180);
        public static Color NodeColorEFF_Table8 = Color.FromArgb(180, 60, 180);
        public static Color NodeColorEFF_EffectEntry = Color.FromArgb(200, 100, 200);
        public static Color NodeColorEFF_Table9 = Color.FromArgb(150, 90, 200);
        public static Color NodeColorSAR = Color.FromArgb(128, 0, 128);
        public static Color NodeColorEAR = Color.FromArgb(153, 50, 204);
        public static Color NodeColorEMI = Color.FromArgb(0, 160, 160);
        public static Color NodeColorESE = Color.FromArgb(0, 128, 128);
        public static Color NodeColorLIT = Color.FromArgb(180, 140, 0);


        // color GL
        // cores
        public static Vector4 GL_ColorESL = Utils.ColorToVector4(Color.Red);
        public static Vector4 GL_ColorETS = Utils.ColorToVector4(Color.Maroon);
        public static Vector4 GL_ColorITA = Utils.ColorToVector4(Color.Blue);
        public static Vector4 GL_ColorAEV = Utils.ColorToVector4(Color.Lime);
        public static Vector4 GL_ColorEXTRAS = Utils.ColorToVector4(Color.SlateGray);
        public static Vector4 GL_ColorDSE = Utils.ColorToVector4(Color.Gray);
        public static Vector4 GL_ColorCAM_TriggerZone = Utils.ColorToVector4(Color.Yellow);
        public static Vector4 GL_ColorCAM_Camera = Utils.ColorToVector4(Color.Red);
        public static Vector4 GL_ColorFSE = Utils.ColorToVector4(Color.LightCyan);
        public static Vector4 GL_ColorSAR = Utils.ColorToVector4(Color.Cyan);
        public static Vector4 GL_ColorEAR = Utils.ColorToVector4(Color.DodgerBlue);
        public static Vector4 GL_ColorEMI = Utils.ColorToVector4(Color.Goldenrod);
        public static Vector4 GL_ColorESE = Utils.ColorToVector4(Color.Violet);
        public static Vector4 GL_ColorEFF_Table7 = Utils.ColorToVector4(Color.Teal);
        public static Vector4 GL_ColorEFF_Table8 = Utils.ColorToVector4(Color.SeaGreen);
        public static Vector4 GL_ColorEFF_EffectEntry = Utils.ColorToVector4(Color.DarkSlateGray);
        public static Vector4 GL_ColorEFF_Table9 = Utils.ColorToVector4(Color.DarkViolet);
        public static Vector4 GL_ColorLIT = Utils.ColorToVector4(Color.DarkSlateGray);
        public static Vector4 GL_ColorSelected = Utils.ColorToVector4(Color.Yellow);
        public static Vector4 GL_ColorItemTriggerZone = Utils.ColorToVector4(Color.Fuchsia);
        public static Vector4 GL_ColorItemTriggerZoneSelected = Utils.ColorToVector4(Color.Pink);
        public static Vector4 GL_ColorItemTrigggerRadius = Utils.ColorToVector4(Color.DeepPink);
        public static Vector4 GL_ColorItemTrigggerRadiusSelected = Utils.ColorToVector4(Color.Plum);
        public static Vector4 GL_ColorGrid = Utils.ColorToVector4(Color.DarkGray);

        // more Colors
        public static Vector4 GL_MoreColor_T00_GeneralPurpose = Utils.ColorToVector4(Color.Green);
        public static Vector4 GL_MoreColor_T01_DoorWarp = Utils.ColorToVector4(Color.DarkOrange); //DarkOrange
        public static Vector4 GL_MoreColor_T02_CutSceneEvents = Utils.ColorToVector4(Color.Olive);
        public static Vector4 GL_MoreColor_T06_MulitTriggerZone = Utils.ColorToVector4(Color.Teal);
        public static Vector4 GL_MoreColor_T04_GroupedEnemyTrigger = Utils.ColorToVector4(Color.Sienna); //Thistle //DarkMagenta
        public static Vector4 GL_MoreColor_T05_Message = Utils.ColorToVector4(Color.MediumPurple);
        public static Vector4 GL_MoreColor_T08_TypeWriter = Utils.ColorToVector4(Color.Indigo);
        public static Vector4 GL_MoreColor_T0A_DamagesThePlayer = Utils.ColorToVector4(Color.LightSteelBlue); //Tomato
        public static Vector4 GL_MoreColor_T0B_FalseCollision = Utils.ColorToVector4(Color.Crimson); //Crimson
        public static Vector4 GL_MoreColor_T0D_Unknown = Utils.ColorToVector4(Color.DarkSeaGreen);
        public static Vector4 GL_MoreColor_T0E_Crouch = Utils.ColorToVector4(Color.BlanchedAlmond); //DarkSlateGray //DarkSalmon
        public static Vector4 GL_MoreColor_T10_FixedLadderClimbUp = Utils.ColorToVector4(Color.SteelBlue); //Chocolate
        public static Vector4 GL_MoreColor_T11_ItemDependentEvents = Utils.ColorToVector4(Color.DarkViolet);//DarkViolet //BlueViolet //DarkSlateBlue //Goldenrod //BlanchedAlmond
        public static Vector4 GL_MoreColor_T12_AshleyHideCommand = Utils.ColorToVector4(Color.Lavender);
        public static Vector4 GL_MoreColor_T13_LocalTeleportation = Utils.ColorToVector4(Color.DarkSalmon); //Wheat //DarkViolet
        public static Vector4 GL_MoreColor_T14_UsedForElevators = Utils.ColorToVector4(Color.YellowGreen);
        public static Vector4 GL_MoreColor_T15_AdaGrappleGun = Utils.ColorToVector4(Color.Navy);

        #endregion

        // backup da class config
        public static Re4QuadExtremeEditor.src.JSON.Configs BackupConfigs = null;


        #region Menu options
        // se pode renderizar o modelo 3d da room
        public static bool RenderRoom = true;

        public static bool RenderEnemyESL = true;
        public static bool RenderEtcmodelETS = true;
        public static bool RenderItemsITA = true;
        public static bool RenderEventsAEV = true;

        //enemy renders
        public static bool RenderDisabledEnemy = true;
        public static bool RenderDontShowOnlyDefinedRoom = true;
        public static ushort RenderEnemyFromDefinedRoom = 0x0000;
        public static bool AutoDefinedRoom = false;

        // items render
        public static bool RenderItemTriggerZone = true;
        public static bool RenderItemPositionAtAssociatedObjectLocation = false;
        public static bool RenderItemTriggerRadius = true;

        //special render
        public static bool RenderSpecialTriggerZone = true;
        public static bool RenderExtraObjs = true;
        public static bool RenderFileEFFBLOB = true;
        public static bool RenderFileDSE = true;
        public static bool RenderFileEFF_Table7 = true;
        public static bool RenderFileEFF_Table8 = true;
        public static bool RenderFileEFF_EffectEntry = true;
        public static bool RenderFileEFF_Table9 = true;
        public static bool RenderFileFSE = true;
        public static bool RenderFileESE = true;
        public static bool RenderFileEMI = true;
        public static bool RenderFileLIT = true;
        public static bool RenderFileLIT_Groups = true;
        public static bool RenderFileLIT_Entrys = true;
        public static bool RenderFileEAR = true;
        public static bool RenderFileSAR = true;

        // EFF group display settings
        public static bool EFF_RenderTable7 = true;
        public static bool EFF_RenderTable8 = true;
        public static bool EFF_RenderTable9 = true;
        public static bool EFF_ShowOnlySelectedGroup = false;
        public static ushort EFF_SelectedGroup = 0;
        public static bool EFF_Use_Group_Position = false;

        // LIT group display settings
        public static bool LIT_ShowOnlySelectedGroup = false;
        public static ushort LIT_SelectedGroup = 0;
        public static bool UseMoreSpecialColors = false;
        public static bool RenderExtraWarpDoor = true;
        public static bool HideExtraExceptWarpDoor = false;

        //Etcmodel
        public static bool RenderEtcmodelUsingScale = false;


        public static bool TreeNodeRenderHexValues = false;

        // opção que muda no propetyGrid
        public static bool PropertyGridUseHexFloat = false;

        //search
        public static bool SearchFilterMode = false;


        #endregion


        #region patch Files, diretorios dos arquivos

        public static string FilePathESL = null;
        public static string FilePathETS = null;
        public static string FilePathITA = null;
        public static string FilePathAEV = null;
        public static string FilePathDSE = null;
        public static string FilePathCAM = null;
        public static string FilePathFSE = null;
        public static string FilePathSAR = null;
        public static string FilePathEAR = null;
        public static string FilePathESE = null;
        public static string FilePathEMI = null;
        public static string FilePathLIT = null;
        public static string FilePathEFFBLOB = null;

        #endregion

        // Render Options
        public static int FOV = 60; // field of view (in degrees)

        //opção de lista de inimigos extra sets.
        public static bool CreateEnemyExtraSegmentList = true;


        //cam grid
        public static bool CamGridEnable = false;
        public static int CamGridvalue = 100;


        // linguagens selecionaveis
        public static List<JSON.LangObjForList> Langs = new List<JSON.LangObjForList>();

        // treenode fonts
        public static Font TreeNodeFontText = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
        public static Font TreeNodeFontHex = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold);

        //OpenGLVersion
        public static string OpenGLVersion = "";
        public static bool UseOldGL = false;

        // UI
        public static bool DarkMode = false;
        public static int TargetFPS = 60;

        // Last used directory
        public static string LastDirectory = null;
        public static string LastDirectoryESL = null;
        public static string LastDirectoryETS = null;
        public static string LastDirectoryITA = null;
        public static string LastDirectoryAEV = null;
        public static string LastDirectoryDSE = null;
        public static string LastDirectoryFSE = null;
        public static string LastDirectorySAR = null;
        public static string LastDirectoryEAR = null;
        public static string LastDirectoryEMI = null;
        public static string LastDirectoryESE = null;
        public static string LastDirectoryLIT = null;
        public static string LastDirectoryEFFBLOB = null;
        public static string LastProjectPath = null;
        public static bool IsProjectActive = false;

    }
}
