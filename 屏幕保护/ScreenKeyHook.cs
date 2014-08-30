using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace 屏幕保护
{
    public class ScreenKeyHook
    {
        private FormScreenSaver formScreenSaver;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        public ScreenKeyHook(FormScreenSaver formScreenSaver)
        {
            this.formScreenSaver = formScreenSaver;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern int SetWindowsHookEx(int hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern bool UnhookWindowsHookEx(int hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern int CallNextHookEx(int hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll")]
        static extern byte MapVirtualKey(byte wCode, int wMap);
        //委托
        private delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        private int hHook;//设置钩子返回的句柄
        private HookProc keyBoardHookProcedure;//委托声明
        //键盘Hook结构函数
        [StructLayout(LayoutKind.Sequential)]
        struct KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;//显示是否这是一个扩展键
            public int time;
            public int dwExtraInfo;
        }
        /// <summary>
        /// 开始安装钩子
        /// </summary>
        public void KeyBoardHookStart()
        {
            //安装键盘钩子
            if (hHook == 0)
            {
                keyBoardHookProcedure = new HookProc(KeyBoardHookProc);
                IntPtr mainModuleIntPtr = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
                hHook = SetWindowsHookEx(WH_KEYBOARD_LL, keyBoardHookProcedure, mainModuleIntPtr, 0);
            }
            else
                MessageBox.Show("钩子已经安装!");
        }
        /// <summary>
        /// 卸载钩子
        /// </summary>
        public void KeyBoardHookStop()
        {
            if (UnhookWindowsHookEx(hHook) == true)
            {
                hHook = 0;
            }
            else
                MessageBox.Show("钩子卸载失败!");
        }
        /// <summary>
        /// 钩子的回调方法
        /// </summary>
        private int KeyBoardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //监控用户键盘输入
            KeyBoardHookStruct input = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
            //Alt+Tab, Alt+Esc, Ctrl+Esc, Windows键,Ctrl+Alt+Del, Windows键+Tab 组合键过滤
            if (wParam == (IntPtr)WM_SYSKEYDOWN)//Alt + * 组合键的屏蔽
                return 1;
            if (input.vkCode == (int)Keys.LWin || input.vkCode == (int)Keys.RWin)//Windows + * 组合键的屏蔽
                return 1;
            if (input.vkCode == (int)Keys.Escape)//* + Esc键的屏蔽
                return 1;
            if (input.vkCode == (int)Keys.ControlKey)// Ctrl + *键的屏蔽
                return 1;

            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }
    }
}
