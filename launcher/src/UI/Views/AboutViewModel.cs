/*
 * Copyright © Windower Dev Team
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation files
 * (the "Software"),to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
 * BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
 * ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

namespace Windower.UI.Views
{
    using Core;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;

    public class AboutViewModel : ViewModelBase
    {
        private INavigationService navigation;

        public AboutViewModel(INavigationService navigation)
        {
            this.navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

            Close = new DelegateCommand(ExecuteClose);
            OpenWebpage = new DelegateCommand(ExecuteOpenWebpage);
        }

        public ICommand Close { get; }

        public ICommand OpenWebpage { get; }

        public static string Copyright { get; } = GetCopyright();

        public static Version LauncherVersion { get; } = GetLauncherVersion();

        public static Version CoreVersion { get; } = GetCoreVersion();

        public static string BuildTag { get; } = Program.BuildTag;

        private static string GetCopyright()
        {
            var attribute = typeof(AboutViewModel).Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute))
                .FirstOrDefault();
            return ((AssemblyCopyrightAttribute)attribute)?.Copyright;
        }

        private static Version GetLauncherVersion() => typeof(AboutViewModel).Assembly.GetName().Version;

        private static Version GetCoreVersion()
        {
            FileVersionInfo info;
            try
            {
                info = FileVersionInfo.GetVersionInfo(Launcher.CorePath);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            return new Version(info.ProductMajorPart, info.ProductMinorPart, info.ProductBuildPart, info.ProductPrivatePart);
        }

        private void ExecuteClose(object arg) => navigation.Close();

        private void ExecuteOpenWebpage(object arg)
        {
            if (arg is string url)
            {
                Process.Start(url);
            }
        }
    }
}
