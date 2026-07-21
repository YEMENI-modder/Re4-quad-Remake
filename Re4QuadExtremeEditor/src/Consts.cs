using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re4QuadExtremeEditor.src
{
    /// <summary>
    /// todas as constantes do programa
    /// </summary>
    public static class Consts
    {
        #region Program Directories
        // lista de de diretorios do programa
        // diretorio principal
        public const string dataDiretory = @"data\";
        // diretorio do arquivo de comfigurações
        public const string ConfigsFileDiretory = @"data\Configs.json";
        // diretorio da lista de Rooms
        public const string RoomListFileDiretory = @"data\RoomList.json";
        // diretorio de onde fica os arquivos RoomJson
        public const string RoomJsonFilesDiretory = @"data\RoomJson\";
        // diretorio da lista de model de itens
        public const string ItemsModelsListFileDiretory = @"data\ItemsModelsList.json";
        // diretorio da lista de model de etcmodel
        public const string EtcModelsListFileDiretory = @"data\EtcModelsList.json";
        // diretorio da lista de model dos inimigos
        public const string EnemiesModelsListFileDiretory = @"data\EnemiesModelsList.json";
        // diretorio da lista de model dos objetos internos
        public const string InternalModelsListFileDiretory = @"data\InternalModelsList.json";
        // diretorio de onde fica os arquivos ModelJson dos itens
        public const string ItemsModelsJsonFilesDiretory = @"data\ItemsModelsJson\";
        // diretorio de onde fica os arquivos ModelJson dos itens
        public const string EtcModelsJsonFilesDiretory = @"data\EtcModelsJson\";
        // diretorio de onde fica os arquivos ModelJson dos itens
        public const string EnemiesModelsJsonFilesDiretory = @"data\EnemiesModelsJson\";
        // diretorio de onde fica os arquivos ModelJson dos objetos internos 
        public const string InternalModelsJsonFilesDiretory = @"data\InternalModelsJson\";
        // diretorio da lista de ObjInfo de itens
        public const string ItemsObjInfoListFileDiretory = @"data\ItemsObjInfoList.json";
        // diretorio da lista de ObjInfo de etcmodel
        public const string EtcModelObjInfoListFileDiretory = @"data\EtcModelObjInfoList.json";
        // diretorio da lista de ObjInfo dos inimigos
        public const string EnemiesObjInfoListFileDiretory = @"data\EnemiesObjInfoList.json";

        // diretorio da lista de PromptMessages
        public const string PromptMessageListFileDiretory = @"data\PromptMessageList.json";

        // diretorio da pasta de linguagens
        public const string langDiretory = @"lang\";
        // diretorio da lista de linguagens, para selecionar
        public const string LangListFileDiretory = @"lang\LangList.json";

        #endregion

        // nome da textura transparente
        public const string TransparentTextureName = "TransparentTexture";

        // nome para os grupos
        public const string ItemsModelGroupName = "ItemsModelGroupName";
        public const string EtcModelGroupName = "EtcModelGroupName";
        public const string EnemiesModelGroupName = "EnemiesModelGroupName";
        public const string InternalModelGroupName = "InternalModelGroupName";

        // nomes dos nodes principais
        public const string NodeESL = "NodeESL";
        public const string NodeETS = "NodeETS";
        public const string NodeITA = "NodeITA";
        public const string NodeAEV = "NodeAEV";
        public const string NodeEXTRAS = "NodeEXTRAS";
        public const string NodeDSE = "NodeDSE";
        public const string NodeFSE = "NodeFSE";
        public const string NodeEFF_Table0 = "NodeEFF_Table0";
        public const string NodeEFF_Table1 = "NodeEFF_Table1";
        public const string NodeEFF_Table2 = "NodeEFF_Table2";
        public const string NodeEFF_Table3 = "NodeEFF_Table3";
        public const string NodeEFF_Table4 = "NodeEFF_Table4";
        public const string NodeEFF_Table6 = "NodeEFF_Table6";
        public const string NodeEFF_Table7 = "NodeEFF_Table7";
        public const string NodeEFF_Table8 = "NodeEFF_Table8";
        public const string NodeEFF_EffectEntry = "NodeEFF_EffectEntry";
        public const string NodeEFF_Table9 = "NodeEFF_Table9";
        public const string NodeSAR = "NodeSAR";
        public const string NodeEAR = "NodeEAR";
        public const string NodeEMI = "NodeEMI";
        public const string NodeESE = "NodeESE";
        public const string NodeLIT_Groups = "NodeLIT_Groups";
        public const string NodeLIT_Entrys = "NodeLIT_Entrys";


        // nomes dos modelos internos usadas nos objetos extras
        public const string ModelKeyWarpPoint = "WarpArrow";
        public const string ModelKeyLadderPoint = "LadderX";
        public const string ModelKeyLadderObj = "Ladder";
        public const string ModelKeyLadderError = "LadderError";
        public const string ModelKeyAshleyPoint = "TextureX";
        public const string ModelKeyGrappleGunPoint = "GrappleGunArrow";
        public const string ModelKeyLocalTeleportationPoint = "LocalTeleportationArrow";

        public const string ModelKeyEffEntryPoint = "EffEntryPoint";
        public const string ModelKeyEffGroupPoint = "EffGroupPoint";
        public const string ModelKeyEffTable9Point = "EffTable9";

        public const string ModelKeyEMIPoint = "EMI_Point";
        public const string ModelKeyESEPoint = "ESE_Point";
        public const string ModelKeyLITPoint = "LIT_Point";


        //o meu maior float
        public const float MyFloatMax = 1000000000000000000000000000f; // 000 000

        // NewAge limits
        public const ushort AmountLimitDSE = 5000;
        public const ushort AmountLimitFSE = 5000;
        public const ushort AmountLimitSAR = 5000;
        public const ushort AmountLimitEAR = 5000;
        public const ushort AmountLimitEMI = 5000;
        public const ushort AmountLimitESE = 5000;
        public const ushort AmountLimitLIT_Groups = 500;
        public const ushort AmountLimitLIT_Entrys = 5000;
        public const ushort AmountLimitEFF_Table0 = 1000;
        public const ushort AmountLimitEFF_Table1 = 1000;
        public const ushort AmountLimitEFF_Table2 = 1000;
        public const ushort AmountLimitEFF_Table3 = 1000;
        public const ushort AmountLimitEFF_Table4 = 1000;
        public const ushort AmountLimitEFF_Table6 = 1000;
        public const ushort AmountLimitEFF_Table7and8 = 1000;
        public const ushort AmountLimitEFF_Table9_Group = 1000;
        public const ushort AmountLimitEFF_Table9_entry = 5000;
        public const ushort AmountLimitEFF_EffectEntry = 10000;
    }
}
