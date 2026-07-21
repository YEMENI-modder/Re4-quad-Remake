using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Re4QuadExtremeEditor.src.JSON;
using Re4QuadExtremeEditor.src.Class;
using Re4QuadExtremeEditor.src.Class.TreeNodeObj;
using Re4QuadExtremeEditor.src.Class.Files;
using Re4QuadExtremeEditor.src.Class.Enums;
using Re4QuadExtremeEditor.src.Class.Shaders;
using Re4QuadExtremeEditor.src.Class.ObjMethods;
using Re4QuadExtremeEditor.src.Class.CAM;

namespace Re4QuadExtremeEditor.src
{
    /// <summary>
    /// Contem todo o conteudo da modelagem(3d), os conteudos dos arquivos AEV, ESL, ETS e ITA, e as definições dos objetos;
    /// </summary>
    public static class DataBase
    {
        // representa a lista de mapas para serem selecionados
        // poderia carregar na classe SelectRoom, porem deixo aqui para carregar a tradução somente uma vez.
        public static List<RoomInfo> RoomList = new List<RoomInfo>();

        //representa a Room (cenario) selecionada, e a ser renderizada
        public static Room SelectedRoom = null;

        //shaders
        public static IShader ShaderRoom = null;
        public static IShader ShaderObjs = null;
        public static IShader ShaderBoundingBox = null;

        // texturas padrões
        public static int NoTextureIdGL;
        public static int TransparentTextureIdGL;
        public static int SolidTextureIdGL;

        // Opaque white 1x1 texture (alpha = 1.0 everywhere). Used as texture unit 1's default
        // for SMD/UHD room meshes that don't carry their own opacity map, so the alpha channel
        // read by RoomShaderFrag.frag is always 1.0 (fully visible) unless a real opacity map
        // says otherwise — matching NewAge's DataShader.WhiteTexture.Use(TextureUnit.Texture1)
        // default-binding pattern.
        public static int WhiteTextureIdGL;

        // os grupos de objetos presentes no programa
        public static ModelGroup EnemiesModels; // modelos dos inimigos
        public static ModelGroup ItemsModels; // modelos dos itens
        public static ModelGroup EtcModels; // modelos da pasta "etcmodel"
        public static ModelGroup InternalModels; // modelos proprios pra o programa



        // Dicionarios com os ids dos objetos no jogo
        public static Dictionary<ushort, ObjInfo> EnemiesIDs;
        public static Dictionary<ushort, ObjInfo> ItemsIDs;
        public static Dictionary<ushort, ObjInfo> EtcModelIDs;


        // aqui são os objetos que representa os arquivos no programa
        public static FileEnemyEslGroup FileESL;
        public static FileEtcModelEtsGroup FileETS;
        public static FileSpecialGroup FileITA;
        public static FileSpecialGroup FileAEV;
        public static ExtraGroup Extras;
        public static File_DSE_Group FileDSE;
        public static CamFile FileCAM;
        public static File_EMI_Group FileEMI;
        public static File_ESAR_Group FileSAR;
        public static File_ESAR_Group FileEAR;
        public static File_ESE_Group FileESE;
        public static File_FSE_Group FileFSE;
        public static File_LIT_Group FileLIT;
        public static File_EFFBLOB_Group FileEFF;

        //conteudo do treeview
        public static EnemyNodeGroup NodeESL;
        public static EtcModelNodeGroup NodeETS;
        public static SpecialNodeGroup NodeITA;
        public static SpecialNodeGroup NodeAEV;
        public static ExtraNodeGroup NodeEXTRAS;
        public static NewAge_DSE_NodeGroup NodeDSE;
        public static NewAge_EMI_NodeGroup NodeEMI;
        public static NewAge_ESAR_NodeGroup NodeSAR;
        public static NewAge_ESAR_NodeGroup NodeEAR;
        public static NewAge_ESE_NodeGroup NodeESE;
        public static NewAge_FSE_NodeGroup NodeFSE;
        public static NewAge_LIT_Groups_NodeGroup NodeLIT_Groups;
        public static NewAge_LIT_Entrys_NodeGroup NodeLIT_Entrys;
        public static NewAge_EFF_Table0Entry_NodeGroup NodeEFF_Table0;
        public static NewAge_EFF_Table1Entry_NodeGroup NodeEFF_Table1;
        public static NewAge_EFF_Table2Entry_NodeGroup NodeEFF_Table2;
        public static NewAge_EFF_Table3Entry_NodeGroup NodeEFF_Table3;
        public static NewAge_EFF_Table4Entry_NodeGroup NodeEFF_Table4;
        public static NewAge_EFF_Table6Entry_NodeGroup NodeEFF_Table6;
        public static CAM_NodeGroup NodeCAM;
        public static NewAge_EFF_EffectGroup_NodeGroup NodeEFF_Table7_Effect_0;
        public static NewAge_EFF_EffectGroup_NodeGroup NodeEFF_Table8_Effect_1;
        public static NewAge_EFF_EffectEntry_NodeGroup NodeEFF_EffectEntry;
        public static NewAge_EFF_Table9Entry_NodeGroup NodeEFF_Table9;

        // lista de objetos selecionados na treeview
        public static Dictionary<int, TreeNode> SelectedNodes;
        // o ultimo node/objeto selecionado
        public static TreeNode LastSelectNode = null;

    }
}
