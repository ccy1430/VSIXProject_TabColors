using Community.VisualStudio.Toolkit;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Task = System.Threading.Tasks.Task;
//using Microsoft.VisualStudio.PlatformUI.Shell;
//using Microsoft.VisualStudio.PlatformUI.Shell.Controls;

namespace VSIXProject_TabColors
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(VSIXProject_TabColorsPackage.PackageGuidString)]
    //[InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.ShellInitialized_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class VSIXProject_TabColorsPackage : AsyncPackage
    {
        /// <summary>
        /// VSIXProject_TabColorsPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "ce986b90-09b5-46a9-8b23-29b6be151e5c";

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.

            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            base.Initialize();
            var dte = (DTE2)await GetServiceAsync(typeof(DTE));
            var events = dte.Events as Events2;
            var documentEvents = events.DocumentEvents;
            documentEvents.DocumentOpened += OnDocumentOpened;

            var documents = dte.Documents;
            foreach (Document doc in documents)
            {
                OnDocumentOpened(doc);
            }



            Debug.WriteLine("------------------------------------------------");
            Debug.WriteLine("tabcolor");
            Debug.WriteLine("------------------------------------------------");
        }

        private void OnDocumentOpened(Document document)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Debug.WriteLine($"Document opened: {document}");
            var window = document?.ActiveWindow;
            if (window == null) return;

            var rootView = window.GetType().GetRuntimeProperty("DockViewElement").GetValue(window);
            if (rootView == null) return;

            //+ { ProjectTemplate.csproj}
            //Microsoft.VisualStudio.PlatformUI.Shell.ViewElement { Microsoft.VisualStudio.Platform.WindowManagement.DocumentView}

            //frame.SetProperty((int)__VSFPROPID.VSFPROPID_BkColor, (uint)Color.Red.ToArgb());
            //Debug.WriteLine(frame.GetType());


            //IEnumerable<WindowFrame> allTabs = await VS.Windows.GetAllDocumentWindowsAsync();
            //foreach (var frame in allTabs)
            //{

            //    object innerFrame = _getFrameField.GetValue(frame);

            //    Type rootviewtype = rootView.GetType();
            //    var windowsFrame = rootviewtype.GetRuntimeProperty("WindowFrame").GetValue(rootView);
            //    if (windowsFrame == null) continue;
            //    string tabpath = windowsFrame.GetType().GetRuntimeProperty("EffectiveDocumentMoniker").GetValue(windowsFrame) as string;
            //    if (tabpath == null) continue;
            //    int pathSubIndex = tabpath.LastIndexOf('\\');
            //    if (pathSubIndex == -1) continue;

            SolidColorBrush insteadBrush = null;
            if (!cacheColors.TryGetValue(document.Path, out insteadBrush))
            {
                var rand = new Random(document.Path.GetHashCode());
                insteadBrush = new SolidColorBrush(new Color()
                {
                    R = (byte)rand.Next(256),
                    G = (byte)rand.Next(256),
                    B = (byte)rand.Next(256),
                    A = 255,
                });
                cacheColors.Add(document.Path, insteadBrush);
            }


            var tabbrush_property = rootView.GetType().GetRuntimeProperty("TabAccentBrush");
            tabbrush_property.SetValue(rootView, insteadBrush);
        }
        private static readonly Dictionary<string, SolidColorBrush> cacheColors = new Dictionary<string, SolidColorBrush>();

        #endregion
    }
}
