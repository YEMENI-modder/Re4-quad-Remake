using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Re4QuadExtremeEditor.src.Class.Enums;

namespace Re4QuadExtremeEditor.src.JSON
{
    /// <summary>
    /// Representa o arquivo de configurações json, nas quais são replicadas na classe Globals;
    /// </summary>
    public class Configs
    {
        public string xscrDiretory { get; set; }
        public string DirectoryUHDRE4 { get; set; }

        public string xfileDiretory { get; set; }

        public Color SkyColor { get; set; }


        // colocar novas configurões aqui;

        // floats
        public ConfigFrationalSymbol FrationalSymbol { get; set; }
        public int FrationalAmount { get; set; }

        //items rotations
        public bool ItemDisableRotationAll { get; set; }
        public bool ItemDisableRotationIfXorYorZequalZero { get; set; }
        public bool ItemDisableRotationIfZisNotGreaterThanZero { get; set; }
        public ObjRotationOrder ItemRotationOrder { get; set; }
        public float ItemRotationCalculationMultiplier { get; set; }
        public float ItemRotationCalculationDivider { get; set; }

        // lang
        public bool LoadLangTranslation { get; set; }
        public string LangID { get; set; }

        // openGL mode Version
        public bool ForceUseOldOpenGL { get; set; }
        public bool ForceUseModernOpenGL { get; set; }

        // UI
        public bool DarkMode { get; set; }
        public int TargetFPS { get; set; }
        public string LastDirectory { get; set; }
        public string LastDirectoryESL { get; set; }
        public string LastDirectoryETS { get; set; }
        public string LastDirectoryITA { get; set; }
        public string LastDirectoryAEV { get; set; }
        public string LastDirectoryDSE { get; set; }
        public string LastDirectoryFSE { get; set; }
        public string LastDirectorySAR { get; set; }
        public string LastDirectoryEAR { get; set; }
        public string LastDirectoryEMI { get; set; }
        public string LastDirectoryESE { get; set; }
        public string LastDirectoryLIT { get; set; }
        public string LastDirectoryEFFBLOB { get; set; }
        public string LastProjectPath { get; set; }
    }
}
