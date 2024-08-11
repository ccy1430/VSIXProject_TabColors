using System;
using System.Reflection;
using Community.VisualStudio.Toolkit;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Task = System.Threading.Tasks.Task;

namespace VSIXProject_TabColors
{
    [Command(PackageIds.ResetTabColors)]
    internal sealed class ResetTabColors : BaseCommand<ResetTabColors>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
            var activeDoc = await VS.Documents.GetActiveDocumentViewAsync();
            var _frame = activeDoc.WindowFrame.GetType().GetField("_frame", BindingFlags.Instance | BindingFlags.NonPublic);
            if (_frame == null) return;
            var _frameVal = _frame.GetValue(activeDoc.WindowFrame);
            if(_frameVal == null) return;   
            var ext = _frameVal.GetType().BaseType.GetField("extWindowObj", BindingFlags.Instance | BindingFlags.NonPublic);
            if(ext == null) return;
            var extVal = ext.GetValue(_frameVal);
            if(extVal == null) return;
            var dte = extVal.GetType().GetProperty("DTE");
            if(dte == null) return;
            var dteVal = dte.GetValue(extVal) as DTE;
            if(dteVal == null) return;

            foreach (Document doc in  dteVal.Documents)
            {
                VSIXProject_TabColorsPackage.ChangeColor(doc, doc.Path);
            }

            //var allWindows = await VS.Windows.GetAllDocumentWindowsAsync();

            //foreach (var win in allWindows)
            //{
            //    var doc = await win.GetDocumentViewAsync();
            //}
        }
    }
}