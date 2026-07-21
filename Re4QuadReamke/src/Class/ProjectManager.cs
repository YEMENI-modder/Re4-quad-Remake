using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Re4QuadExtremeEditor.src.Class.Enums;
using Re4QuadExtremeEditor.src.Class.Files;
using Re4QuadExtremeEditor.src.JSON;
using SimpleEndianBinaryIO;

namespace Re4QuadExtremeEditor.src.Class
{
    /// <summary>
    /// Responsavel por salvar/carregar o arquivo de projeto .re4qrp (RE4 Quad Remake Project)
    /// O arquivo guarda uma copia binaria completa de cada arquivo aberto no momento do save,
    /// alem de qual room estava selecionada, para restaurar a sessao inteira independente
    /// de qualquer edicao nao salva.
    /// </summary>
    public static class ProjectManager
    {
        // "RE4QRP" + versao do formato
        private static readonly byte[] Magic = { (byte)'R', (byte)'E', (byte)'4', (byte)'Q', (byte)'R', (byte)'P' };
        private const int FormatVersion = 1;

        public const string ProjectExtension = ".re4qrp";

        // Identificadores de bloco dentro do arquivo de projeto
        private enum BlockId : byte
        {
            Room = 0,
            ESL = 1,
            ETS = 2,
            ITA = 3,
            AEV = 4,
            DSE = 5,
            FSE = 6,
            SAR = 7,
            EAR = 8,
            EMI = 9,
            ESE = 10,
            LIT = 11,
            EFFBLOB = 12,
            Camera = 13,
        }

        public static void SaveProject(string path, Camera camera)
        {
            using (FileStream fs = new FileInfo(path).Create())
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(Magic);
                bw.Write(FormatVersion);

                // Room
                if (DataBase.SelectedRoom != null)
                {
                    WriteBlockHeader(bw, BlockId.Room, 0);
                    long roomLenPos = fs.Position - 4;
                    long roomBlockStart = fs.Position;
                    WriteString(bw, DataBase.SelectedRoom.GetRoomInfo.RoomKey);
                    bw.Write((byte)DataBase.SelectedRoom.ModelSource);
                    long roomBlockEnd = fs.Position;
                    fs.Position = roomLenPos;
                    bw.Write((int)(roomBlockEnd - roomBlockStart));
                    fs.Position = roomBlockEnd;
                }

                // Camera (posição, yaw e pitch atuais do viewport)
                if (camera != null)
                {
                    WriteBlockHeader(bw, BlockId.Camera, 0);
                    long camLenPos = fs.Position - 4;
                    long camBlockStart = fs.Position;
                    bw.Write(camera.Position.X);
                    bw.Write(camera.Position.Y);
                    bw.Write(camera.Position.Z);
                    bw.Write(camera.YawDegrees);
                    bw.Write(camera.PitchDegrees);
                    long camBlockEnd = fs.Position;
                    fs.Position = camLenPos;
                    bw.Write((int)(camBlockEnd - camBlockStart));
                    fs.Position = camBlockEnd;
                }

                WriteFileBlock(bw, BlockId.ESL, DataBase.FileESL != null, Globals.FilePathESL, null, w => FileManager.SaveFileESL(w));
                WriteFileBlock(bw, BlockId.ETS, DataBase.FileETS != null, Globals.FilePathETS, DataBase.FileETS?.GetRe4Version, w => FileManager.SaveFileETS(w));
                WriteFileBlock(bw, BlockId.ITA, DataBase.FileITA != null, Globals.FilePathITA, DataBase.FileITA?.GetRe4Version, w => FileManager.SaveFileITA(w));
                WriteFileBlock(bw, BlockId.AEV, DataBase.FileAEV != null, Globals.FilePathAEV, DataBase.FileAEV?.GetRe4Version, w => FileManager.SaveFileAEV(w));
                WriteFileBlock(bw, BlockId.DSE, DataBase.FileDSE != null, Globals.FilePathDSE, null, w => FileManager.SaveFileDSE(w));
                WriteFileBlock(bw, BlockId.FSE, DataBase.FileFSE != null, Globals.FilePathFSE, null, w => FileManager.SaveFileFSE(w));
                WriteFileBlock(bw, BlockId.SAR, DataBase.FileSAR != null, Globals.FilePathSAR, null, w => FileManager.SaveFileSAR(w));
                WriteFileBlock(bw, BlockId.EAR, DataBase.FileEAR != null, Globals.FilePathEAR, null, w => FileManager.SaveFileEAR(w));
                WriteFileBlock(bw, BlockId.EMI, DataBase.FileEMI != null, Globals.FilePathEMI, null, w => FileManager.SaveFileEMI(w));
                WriteFileBlock(bw, BlockId.ESE, DataBase.FileESE != null, Globals.FilePathESE, null, w => FileManager.SaveFileESE(w));
                WriteFileBlock(bw, BlockId.LIT, DataBase.FileLIT != null, Globals.FilePathLIT, null, w => FileManager.SaveFileLIT(w));

                if (DataBase.FileEFF != null)
                {
                    WriteBlockHeader(bw, BlockId.EFFBLOB, 0); // tamanho corrigido depois
                    long lenPos = fs.Position - 4;
                    long blockStart = fs.Position; // inclui path + byte de endian no tamanho do bloco
                    WriteString(bw, Globals.FilePathEFFBLOB ?? string.Empty);
                    bw.Write((byte)DataBase.FileEFF.Endian);
                    using (FileStream tmp = CreateTempFileStream(out string tmpPath))
                    {
                        FileManager.SaveFileEFFBLOB(tmp);
                        tmp.Flush();
                        tmp.Position = 0;
                        tmp.CopyTo(fs);
                        DeleteTempFile(tmpPath);
                    }
                    long blockEnd = fs.Position;
                    fs.Position = lenPos;
                    bw.Write((int)(blockEnd - blockStart));
                    fs.Position = blockEnd;
                }
            }
        }

        public static void LoadProject(string path, Camera camera)
        {
            using (FileStream fs = new FileInfo(path).OpenRead())
            using (BinaryReader br = new BinaryReader(fs))
            {
                byte[] magic = br.ReadBytes(Magic.Length);
                if (!magic.SequenceEqual(Magic))
                {
                    throw new InvalidDataException("Not a valid RE4QRP project file.");
                }
                int version = br.ReadInt32();

                while (fs.Position < fs.Length)
                {
                    BlockId id = (BlockId)br.ReadByte();
                    int length = br.ReadInt32();
                    long blockEnd = fs.Position + length;

                    switch (id)
                    {
                        case BlockId.Camera:
                            {
                                float posX = br.ReadSingle();
                                float posY = br.ReadSingle();
                                float posZ = br.ReadSingle();
                                float yaw = br.ReadSingle();
                                float pitch = br.ReadSingle();
                                if (camera != null)
                                {
                                    camera.SetToFlyMode();
                                    camera.Position = new OpenTK.Vector3(posX, posY, posZ);
                                    camera.YawDegrees = yaw;
                                    camera.PitchDegrees = pitch;
                                    camera.RefreshOrientation();
                                }
                                break;
                            }
                        case BlockId.Room:
                            {
                                string roomKey = ReadString(br);

                                // backward-compat: older .re4qrp files don't have this byte at all
                                Enums.RoomModelLoadSource modelSource = Enums.RoomModelLoadSource.Xcar;
                                if (fs.Position < blockEnd)
                                {
                                    modelSource = (Enums.RoomModelLoadSource)br.ReadByte();
                                }

                                RoomInfo info = DataBase.RoomList.FirstOrDefault(r => r.RoomKey == roomKey);
                                if (info != null)
                                {
                                    if (DataBase.SelectedRoom != null)
                                    {
                                        DataBase.SelectedRoom.ClearGL();
                                        DataBase.SelectedRoom = null;
                                    }
                                    DataBase.SelectedRoom = new Room(info, modelSource);
                                }
                                break;
                            }
                        case BlockId.ESL:
                            {
                                string filePath = ReadString(br);
                                Globals.FilePathESL = string.IsNullOrEmpty(filePath) ? null : filePath;
                                int dataLen = (int)(blockEnd - fs.Position);
                                LoadBytesIntoFile(br, dataLen, "ESL", (fi, fstream) => FileManager.LoadFileESL(fstream, fi));
                                break;
                            }
                        case BlockId.ETS:
                            {
                                string filePath = ReadString(br);
                                Globals.FilePathETS = string.IsNullOrEmpty(filePath) ? null : filePath;
                                Re4Version ver = (Re4Version)br.ReadByte();
                                int dataLen = (int)(blockEnd - fs.Position);
                                LoadBytesIntoFile(br, dataLen, "ETS", (fi, fstream) =>
                                {
                                    if (ver == Re4Version.Classic) FileManager.LoadFileETS_Classic(fstream, fi);
                                    else FileManager.LoadFileETS_UHD(fstream, fi);
                                });
                                break;
                            }
                        case BlockId.ITA:
                            {
                                string filePath = ReadString(br);
                                Globals.FilePathITA = string.IsNullOrEmpty(filePath) ? null : filePath;
                                Re4Version ver = (Re4Version)br.ReadByte();
                                int dataLen = (int)(blockEnd - fs.Position);
                                LoadBytesIntoFile(br, dataLen, "ITA", (fi, fstream) =>
                                {
                                    if (ver == Re4Version.Classic) FileManager.LoadFileITA_Classic(fstream, fi);
                                    else FileManager.LoadFileITA_UHD(fstream, fi);
                                });
                                break;
                            }
                        case BlockId.AEV:
                            {
                                string filePath = ReadString(br);
                                Globals.FilePathAEV = string.IsNullOrEmpty(filePath) ? null : filePath;
                                Re4Version ver = (Re4Version)br.ReadByte();
                                int dataLen = (int)(blockEnd - fs.Position);
                                LoadBytesIntoFile(br, dataLen, "AEV", (fi, fstream) =>
                                {
                                    if (ver == Re4Version.Classic) FileManager.LoadFileAEV_Classic(fstream, fi);
                                    else FileManager.LoadFileAEV_UHD(fstream, fi);
                                });
                                break;
                            }
                        case BlockId.DSE:
                            {
                                string filePath = ReadString(br);
                                Globals.FilePathDSE = string.IsNullOrEmpty(filePath) ? null : filePath;
                                int dataLen = (int)(blockEnd - fs.Position);
                                LoadBytesIntoFile(br, dataLen, "DSE", (fi, fstream) => FileManager.LoadFileDSE(fstream, fi));
                                break;
                            }
                        case BlockId.FSE:
                            {
                                string filePath = ReadString(br);
                                Globals.FilePathFSE = string.IsNullOrEmpty(filePath) ? null : filePath;
                                int dataLen = (int)(blockEnd - fs.Position);
                                LoadBytesIntoFile(br, dataLen, "FSE", (fi, fstream) => FileManager.LoadFileFSE(fstream, fi));
                                break;
                            }
                        case BlockId.SAR:
                            {
                                string filePath = ReadString(br);
                                Globals.FilePathSAR = string.IsNullOrEmpty(filePath) ? null : filePath;
                                int dataLen = (int)(blockEnd - fs.Position);
                                LoadBytesIntoFile(br, dataLen, "SAR", (fi, fstream) => FileManager.LoadFileSAR(fstream, fi));
                                break;
                            }
                        case BlockId.EAR:
                            {
                                string filePath = ReadString(br);
                                Globals.FilePathEAR = string.IsNullOrEmpty(filePath) ? null : filePath;
                                int dataLen = (int)(blockEnd - fs.Position);
                                LoadBytesIntoFile(br, dataLen, "EAR", (fi, fstream) => FileManager.LoadFileEAR(fstream, fi));
                                break;
                            }
                        case BlockId.EMI:
                            {
                                string filePath = ReadString(br);
                                Globals.FilePathEMI = string.IsNullOrEmpty(filePath) ? null : filePath;
                                int dataLen = (int)(blockEnd - fs.Position);
                                LoadBytesIntoFile(br, dataLen, "EMI", (fi, fstream) => FileManager.LoadFileEMI_UHD(fstream, fi));
                                break;
                            }
                        case BlockId.ESE:
                            {
                                string filePath = ReadString(br);
                                Globals.FilePathESE = string.IsNullOrEmpty(filePath) ? null : filePath;
                                int dataLen = (int)(blockEnd - fs.Position);
                                LoadBytesIntoFile(br, dataLen, "ESE", (fi, fstream) => FileManager.LoadFileESE_UHD(fstream, fi));
                                break;
                            }
                        case BlockId.LIT:
                            {
                                string filePath = ReadString(br);
                                Globals.FilePathLIT = string.IsNullOrEmpty(filePath) ? null : filePath;
                                int dataLen = (int)(blockEnd - fs.Position);
                                LoadBytesIntoFile(br, dataLen, "LIT", (fi, fstream) => FileManager.LoadFileLIT_UHD(fstream, fi));
                                break;
                            }
                        case BlockId.EFFBLOB:
                            {
                                string filePath = ReadString(br);
                                Globals.FilePathEFFBLOB = string.IsNullOrEmpty(filePath) ? null : filePath;
                                Endianness endian = (Endianness)br.ReadByte();
                                int dataLen = (int)(blockEnd - fs.Position);
                                byte[] data = br.ReadBytes(dataLen);
                                using (FileStream tmp = CreateTempFileStreamFromBytes(data, out string tmpPath))
                                {
                                    FileManager.LoadFileEFFBLOB(tmp, endian);
                                    DeleteTempFile(tmpPath);
                                }
                                break;
                            }
                    }

                    // garante o alinhamento correto mesmo se algum bloco nao ler tudo
                    fs.Position = blockEnd;
                }
            }
        }

        // ===== helpers =====

        private static void WriteBlockHeader(BinaryWriter bw, BlockId id, int length)
        {
            bw.Write((byte)id);
            bw.Write(length);
        }

        private static void WriteString(BinaryWriter bw, string value)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value ?? string.Empty);
            bw.Write(bytes.Length);
            bw.Write(bytes);
        }

        private static string ReadString(BinaryReader br)
        {
            int len = br.ReadInt32();
            byte[] bytes = br.ReadBytes(len);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        private static void WriteFileBlock(BinaryWriter bw, BlockId id, bool present, string filePath, Re4Version? version, Action<FileStream> saveFunc)
        {
            if (!present) return;

            FileStream fs = (FileStream)bw.BaseStream;
            WriteBlockHeader(bw, id, 0);
            long lenPos = fs.Position - 4;
            long blockStart = fs.Position; // tudo a partir daqui conta para "length"

            WriteString(bw, filePath ?? string.Empty);

            if (version.HasValue)
            {
                bw.Write((byte)version.Value);
            }

            using (FileStream tmp = CreateTempFileStream(out string tmpPath))
            {
                saveFunc(tmp);
                tmp.Flush();
                tmp.Position = 0;
                tmp.CopyTo(fs);
                DeleteTempFile(tmpPath);
            }
            long blockEnd = fs.Position;

            fs.Position = lenPos;
            bw.Write((int)(blockEnd - blockStart));
            fs.Position = blockEnd;
        }

        private static void LoadBytesIntoFile(BinaryReader br, int length, string tag, Action<FileInfo, FileStream> loadFunc)
        {
            byte[] data = br.ReadBytes(length);
            using (FileStream tmp = CreateTempFileStreamFromBytes(data, out string tmpPath))
            {
                loadFunc(new FileInfo(tmpPath), tmp);
                DeleteTempFile(tmpPath);
            }
        }

        private static FileStream CreateTempFileStream(out string path)
        {
            path = Path.Combine(Path.GetTempPath(), "re4qrp_" + Guid.NewGuid().ToString("N") + ".tmp");
            return new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        }

        private static FileStream CreateTempFileStreamFromBytes(byte[] data, out string path)
        {
            path = Path.Combine(Path.GetTempPath(), "re4qrp_" + Guid.NewGuid().ToString("N") + ".tmp");
            File.WriteAllBytes(path, data);
            return new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }

        private static void DeleteTempFile(string path)
        {
            try { if (File.Exists(path)) File.Delete(path); } catch { /* ignore cleanup errors */ }
        }
    }
}
