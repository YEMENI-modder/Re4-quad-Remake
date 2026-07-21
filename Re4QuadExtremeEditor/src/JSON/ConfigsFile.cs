using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;

namespace Re4QuadExtremeEditor.src.JSON
{
    static class ConfigsFile
    {

        public static void writeConfigsFile(string filename, Configs config)
        {
            JObject entry = new JObject();
            entry["xfileDiretory"] = config.xfileDiretory;
            entry["xscrDiretory"] = config.xscrDiretory;
            entry["DirectoryUHDRE4"] = config.DirectoryUHDRE4;
            entry["SkyColor"] = config.SkyColor.ToArgb().ToString("X8");

            // colocar novas configurações aqui;
            entry["FrationalSymbol"] = (int)config.FrationalSymbol;
            entry["FrationalAmount"] = config.FrationalAmount;

            entry["ItemDisableRotationAll"] = config.ItemDisableRotationAll;
            entry["ItemDisableRotationIfXorYorZequalZero"] = config.ItemDisableRotationIfXorYorZequalZero;
            entry["ItemDisableRotationIfZisNotGreaterThanZero"] = config.ItemDisableRotationIfZisNotGreaterThanZero;
            entry["ItemRotationCalculationDivider"] = config.ItemRotationCalculationDivider;
            entry["ItemRotationCalculationMultiplier"] = config.ItemRotationCalculationMultiplier;
            entry["ItemRotationOrder"] = (int)config.ItemRotationOrder;


            entry["LoadLangTranslation"] = config.LoadLangTranslation;
            entry["LangID"] = config.LangID;

            entry["ForceUseModernOpenGL"] = config.ForceUseModernOpenGL;
            entry["ForceUseOldOpenGL"] = config.ForceUseOldOpenGL;

            entry["DarkMode"] = config.DarkMode;
            entry["TargetFPS"] = config.TargetFPS;
            entry["LastDirectory"] = config.LastDirectory;

            entry["LastDirectoryESL"] = config.LastDirectoryESL;
            entry["LastDirectoryETS"] = config.LastDirectoryETS;
            entry["LastDirectoryITA"] = config.LastDirectoryITA;
            entry["LastDirectoryAEV"] = config.LastDirectoryAEV;
            entry["LastDirectoryDSE"] = config.LastDirectoryDSE;
            entry["LastDirectoryFSE"] = config.LastDirectoryFSE;
            entry["LastDirectorySAR"] = config.LastDirectorySAR;
            entry["LastDirectoryEAR"] = config.LastDirectoryEAR;
            entry["LastDirectoryEMI"] = config.LastDirectoryEMI;
            entry["LastDirectoryESE"] = config.LastDirectoryESE;
            entry["LastDirectoryLIT"] = config.LastDirectoryLIT;
            entry["LastDirectoryEFFBLOB"] = config.LastDirectoryEFFBLOB;
            entry["LastProjectPath"] = config.LastProjectPath;

            JObject o = new JObject();
            o["Configs"] = entry;
            try
            {
                File.WriteAllText(filename, o.ToString());
            }
            catch (Exception)
            {
            }
            
        }

        public static Configs parseConfigs(string filename)
        {
            Configs config = Utils.GetDefaultConfigs();
            if (File.Exists(filename))
            {
                string json = File.ReadAllText(filename);
                JObject o = JObject.Parse(json);
                if (o["Configs"] != null)
                {
                    JObject oConfigs = (JObject)o["Configs"];

                    if (oConfigs["xfileDiretory"] != null)
                    {
                        config.xfileDiretory = oConfigs["xfileDiretory"].ToString();
                    }

                    if (oConfigs["xscrDiretory"] != null)
                    {
                        config.xscrDiretory = oConfigs["xscrDiretory"].ToString();
                    }

                    if (oConfigs["DirectoryUHDRE4"] != null)
                    {
                        config.DirectoryUHDRE4 = oConfigs["DirectoryUHDRE4"].ToString();
                    }

                    if (oConfigs["SkyColor"] != null)
                    {
                        try
                        {
                            config.SkyColor = Color.FromArgb(int.Parse(oConfigs["SkyColor"].ToString(), System.Globalization.NumberStyles.HexNumber));
                        }
                        catch (Exception)
                        {
                        }
                      
                    }


                    // colocar novas configurações aqui;

                    if (oConfigs["FrationalSymbol"] != null)
                    {
                        int value = 0;
                        try
                        {
                            value = int.Parse(oConfigs["FrationalSymbol"].ToString(), System.Globalization.NumberStyles.Number);
                        }
                        catch (Exception)
                        {
                        }
                        if (value > 3)
                        {
                            value = 0;
                        }
                        else if (value < 0)
                        {
                            value = 0;
                        }

                        config.FrationalSymbol = (Class.Enums.ConfigFrationalSymbol)value;
                    }

                    if (oConfigs["FrationalAmount"] != null)
                    {
                        int value = 9;
                        try
                        {
                            value = int.Parse(oConfigs["FrationalAmount"].ToString(), System.Globalization.NumberStyles.Number);
                        }
                        catch (Exception)
                        {
                        }
                        if (value > 9)
                        {
                            value = 9;
                        }
                        else if (value < 4)
                        {
                            value = 4;
                        }
                        config.FrationalAmount = value;
                    }

                    if (oConfigs["ItemDisableRotationAll"] != null)
                    {
                        try
                        {
                            config.ItemDisableRotationAll = bool.Parse(oConfigs["ItemDisableRotationAll"].ToString());
                        }
                        catch (Exception)
                        {
                        }
                       
                    }

                    if (oConfigs["ItemDisableRotationIfXorYorZequalZero"] != null)
                    {
                        try
                        {
                            config.ItemDisableRotationIfXorYorZequalZero = bool.Parse(oConfigs["ItemDisableRotationIfXorYorZequalZero"].ToString());
                        }
                        catch (Exception)
                        {
                        }

                    }

                    if (oConfigs["ItemDisableRotationIfZisNotGreaterThanZero"] != null)
                    {
                        try
                        {
                            config.ItemDisableRotationIfZisNotGreaterThanZero = bool.Parse(oConfigs["ItemDisableRotationIfZisNotGreaterThanZero"].ToString());
                        }
                        catch (Exception)
                        {
                        }

                    }


                    if (oConfigs["ItemRotationCalculationDivider"] != null)
                    {
                        try
                        {
                            config.ItemRotationCalculationDivider = float.Parse(oConfigs["ItemRotationCalculationDivider"].ToString(), System.Globalization.NumberStyles.Float);
                        }
                        catch (Exception)
                        {
                        }

                    }

                    if (oConfigs["ItemRotationCalculationMultiplier"] != null)
                    {
                        try
                        {
                            config.ItemRotationCalculationMultiplier = float.Parse(oConfigs["ItemRotationCalculationMultiplier"].ToString(), System.Globalization.NumberStyles.Float);
                        }
                        catch (Exception)
                        {
                        }

                    }

                    if (oConfigs["ItemRotationOrder"] != null)
                    {
                        int value = 0;
                        try
                        {
                            value = int.Parse(oConfigs["ItemRotationOrder"].ToString(), System.Globalization.NumberStyles.Number);
                        }
                        catch (Exception)
                        {
                        }
                        if (value > 14)
                        {
                            value = 0;
                        }
                        else if (value < 0)
                        {
                            value = 0;
                        }
                        config.ItemRotationOrder = (Class.Enums.ObjRotationOrder)value;
                    }


                    if (oConfigs["LoadLangTranslation"] != null)
                    {
                        try
                        {
                            config.LoadLangTranslation = bool.Parse(oConfigs["LoadLangTranslation"].ToString());
                        }
                        catch (Exception)
                        {
                        }

                    }

                    if (oConfigs["LangID"] != null)
                    {
                        config.LangID = oConfigs["LangID"].ToString();
                    }


                    //2023-01-22

                    if (oConfigs["ForceUseModernOpenGL"] != null)
                    {
                        try
                        {
                            config.ForceUseModernOpenGL = bool.Parse(oConfigs["ForceUseModernOpenGL"].ToString());
                        }
                        catch (Exception)
                        {
                        }

                    }

                    if (oConfigs["ForceUseOldOpenGL"] != null)
                    {
                        try
                        {
                            config.ForceUseOldOpenGL = bool.Parse(oConfigs["ForceUseOldOpenGL"].ToString());
                        }
                        catch (Exception)
                        {
                        }

                    }

                    if (oConfigs["DarkMode"] != null)
                    {
                        try
                        {
                            config.DarkMode = bool.Parse(oConfigs["DarkMode"].ToString());
                        }
                        catch (Exception)
                        {
                        }
                    }

                    if (oConfigs["TargetFPS"] != null)
                    {
                        try
                        {
                            config.TargetFPS = int.Parse(oConfigs["TargetFPS"].ToString(), System.Globalization.NumberStyles.Number);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    if (oConfigs["LastDirectory"] != null)
                    {
                        config.LastDirectory = oConfigs["LastDirectory"].ToString();
                    }

                    if (oConfigs["LastDirectoryESL"] != null) { config.LastDirectoryESL = oConfigs["LastDirectoryESL"].ToString(); }
                    if (oConfigs["LastDirectoryETS"] != null) { config.LastDirectoryETS = oConfigs["LastDirectoryETS"].ToString(); }
                    if (oConfigs["LastDirectoryITA"] != null) { config.LastDirectoryITA = oConfigs["LastDirectoryITA"].ToString(); }
                    if (oConfigs["LastDirectoryAEV"] != null) { config.LastDirectoryAEV = oConfigs["LastDirectoryAEV"].ToString(); }
                    if (oConfigs["LastDirectoryDSE"] != null) { config.LastDirectoryDSE = oConfigs["LastDirectoryDSE"].ToString(); }
                    if (oConfigs["LastDirectoryFSE"] != null) { config.LastDirectoryFSE = oConfigs["LastDirectoryFSE"].ToString(); }
                    if (oConfigs["LastDirectorySAR"] != null) { config.LastDirectorySAR = oConfigs["LastDirectorySAR"].ToString(); }
                    if (oConfigs["LastDirectoryEAR"] != null) { config.LastDirectoryEAR = oConfigs["LastDirectoryEAR"].ToString(); }
                    if (oConfigs["LastDirectoryEMI"] != null) { config.LastDirectoryEMI = oConfigs["LastDirectoryEMI"].ToString(); }
                    if (oConfigs["LastDirectoryESE"] != null) { config.LastDirectoryESE = oConfigs["LastDirectoryESE"].ToString(); }
                    if (oConfigs["LastDirectoryLIT"] != null) { config.LastDirectoryLIT = oConfigs["LastDirectoryLIT"].ToString(); }
                    if (oConfigs["LastDirectoryEFFBLOB"] != null) { config.LastDirectoryEFFBLOB = oConfigs["LastDirectoryEFFBLOB"].ToString(); }
                    if (oConfigs["LastProjectPath"] != null) { config.LastProjectPath = oConfigs["LastProjectPath"].ToString(); }

                }

            }
            return config;
        }



    }
}
