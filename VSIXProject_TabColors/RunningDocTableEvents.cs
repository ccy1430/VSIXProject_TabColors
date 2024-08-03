using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Diagnostics;

namespace VSIXProject_TabColors
{
    public class RunningDocTableEvents : IVsRunningDocTableEvents
    {
        private IVsRunningDocumentTable _rdt;

        public RunningDocTableEvents()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _rdt = (IVsRunningDocumentTable)ServiceProvider.GlobalProvider.GetService(typeof(SVsRunningDocumentTable));
            _rdt.AdviseRunningDocTableEvents(this, out _);
        }

        public int OnAfterSave(uint docCookie)
        {
            // 文档保存后的处理逻辑
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            // 文档属性变化后的处理逻辑
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            // 文档窗口隐藏后的处理逻辑
            return VSConstants.S_OK;
        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            // 文档第一次锁定后的处理逻辑
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            // 文档窗口显示后的处理逻辑
            Debug.WriteLine("OnAfterDocumentWindowShow");
            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            // 文档窗口显示前的处理逻辑
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            // 文档锁定后的处理逻辑
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }
    }
}