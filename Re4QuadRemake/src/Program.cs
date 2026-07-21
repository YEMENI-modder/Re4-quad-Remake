using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Re4QuadExtremeEditor
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Garante que o diretório de trabalho seja sempre a pasta do próprio .exe,
            // independente de como o programa foi iniciado (duplo clique direto,
            // associação de arquivo .re4qrp, atalho, etc). Sem isso, caminhos relativos
            // como "data\Configs.json" quebram quando o Windows define o diretório de
            // trabalho como a pasta do arquivo .re4qrp em vez da pasta do programa.
            string exeDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (!string.IsNullOrEmpty(exeDirectory))
            {
                System.IO.Directory.SetCurrentDirectory(exeDirectory);
            }

            // Configurar manipuladores globais de exceção
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            try
            {
                RegisterProjectFileAssociation();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                MainForm mainForm = new MainForm();

                string projectPath = args != null && args.Length > 0 ? args[0] : null;
                if (!string.IsNullOrEmpty(projectPath) && System.IO.File.Exists(projectPath) &&
                    projectPath.EndsWith(Re4QuadExtremeEditor.src.Class.ProjectManager.ProjectExtension, StringComparison.OrdinalIgnoreCase))
                {
                    mainForm.Shown += (s, e) => mainForm.OpenProjectFileWhenReady(projectPath);
                }

                Application.Run(mainForm);
            }
            catch {}
            // Não tem como capturar a System.ObjectDisposedException
            // Essa Exception é gerada quando o openGL é de uma versão não suportada pelo programa
            // O try catch impede do windows gerar um "CrashDumps"
        }

        /// <summary>
        /// Registra a associação do arquivo .re4qrp no Windows para que um duplo clique
        /// abra o programa automaticamente com o projeto (similar ao .blend do Blender).
        /// Silenciosamente ignora falhas (ex: falta de permissão de administrador).
        /// </summary>
        private static void RegisterProjectFileAssociation()
        {
            try
            {
                const string ext = ".re4qrp";
                const string progId = "Re4QuadExtremeEditor.Project";
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

                using (RegistryKey extKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\" + ext))
                {
                    extKey.SetValue("", progId);
                }

                using (RegistryKey progIdKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\" + progId))
                {
                    progIdKey.SetValue("", "RE4 Quad Remake Project");
                    using (RegistryKey iconKey = progIdKey.CreateSubKey("DefaultIcon"))
                    {
                        iconKey.SetValue("", exePath + ",0");
                    }
                    using (RegistryKey commandKey = progIdKey.CreateSubKey(@"shell\open\command"))
                    {
                        commandKey.SetValue("", "\"" + exePath + "\" \"%1\"");
                    }
                }
            }
            catch
            {
                // sem permissão ou outro erro: simplesmente não registra a associação
            }
        }

        // Manipulador para exceções de thread UI
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception, "Error in graphical interface");
        }

        // Manipulador para exceções não tratadas de threads não-UI
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                HandleException(ex, "General error");
            }
        }

        // Método para lidar com exceções
        private static void HandleException(Exception ex, string context)
        {
            string fullMessage = $"{context}: {ex.Message}\n\n{ex.StackTrace}\nAn unexpected error occurred, the program may not work correctly from now on.";

            // Escreve o erro completo em um arquivo de log temporário ao lado do .exe,
            // já que a MessageBox pode cortar o texto em telas/resoluções pequenas.
            try
            {
                string logPath = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                    "error_log.txt");
                System.IO.File.AppendAllText(logPath,
                    $"---- {DateTime.Now:yyyy-MM-dd HH:mm:ss} ----\n{fullMessage}\n\n");
            }
            catch
            {
                // se não conseguir escrever o log (ex: sem permissão), ignora
            }

            MessageBox.Show(fullMessage, "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
