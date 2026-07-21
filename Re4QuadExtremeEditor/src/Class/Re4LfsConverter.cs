using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Re4QuadExtremeEditor.src.Class
{
    /// <summary>
    /// Wraps Tools\Re4lfs.exe (shipped next to the editor's own .exe) to convert a ".pack.lfs"
    /// file into a plain ".pack" file in place, running the tool hidden (no console window),
    /// so "Load from SMD" can transparently fall back to LFS-packed textures without the user
    /// ever seeing a CMD window pop up.
    /// </summary>
    public static class Re4LfsConverter
    {
        private static string ToolsExePath()
        {
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(exeDir ?? string.Empty, "Tools", "Re4lfs.exe");
        }

        /// <summary>
        /// Runs Tools\Re4lfs.exe on lfsFilePath (e.g. "4400XXXX.pack.lfs") to produce the matching
        /// ".pack" file (i.e. lfsFilePath with the trailing ".lfs" removed) in the same folder.
        /// Returns the resulting .pack path, or null if the tool is missing, fails, or the
        /// converted .pack file doesn't show up afterwards.
        /// </summary>
        public static string ConvertAndGetPackPath(string lfsFilePath)
        {
            if (string.IsNullOrEmpty(lfsFilePath) || !File.Exists(lfsFilePath))
            {
                return null;
            }

            string toolPath = ToolsExePath();
            if (!File.Exists(toolPath))
            {
                return null;
            }

            // lfsFilePath ends with ".lfs"; the output .pack is the same path without it.
            string outputPackPath = lfsFilePath.Substring(0, lfsFilePath.Length - ".lfs".Length);

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = toolPath,
                    Arguments = "\"" + lfsFilePath + "\"",
                    WorkingDirectory = Path.GetDirectoryName(lfsFilePath),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };

                using (Process proc = Process.Start(startInfo))
                {
                    // Drain the redirected streams so the child process never blocks on a full pipe buffer.
                    proc.OutputDataReceived += (s, e) => { };
                    proc.ErrorDataReceived += (s, e) => { };
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();

                    // Re4lfs.exe is expected to be a quick, local conversion; guard against a hang.
                    if (!proc.WaitForExit(30000))
                    {
                        try { proc.Kill(); } catch (Exception) { }
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return File.Exists(outputPackPath) ? outputPackPath : null;
        }
    }
}
