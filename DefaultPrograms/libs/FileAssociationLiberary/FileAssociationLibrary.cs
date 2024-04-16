using Microsoft.Win32;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace FileAssociationLibrary {

    public static class RegistryKeyExtensions {
        public static RegistryKey TryOpenSubKey(this RegistryKey Key, string Subkey) {
            if ((Key == null) || (Subkey == null)) {
                return null;
            }
            try {
                return Key.OpenSubKey(Subkey);
            } catch (Exception exception1) {
                string source = exception1.ToString();
                if (source.Contains<char>('\n')) {
                    source = source.Substring(0, source.IndexOf('\n') - 1);
                }
                string[] textArray1 = new string[] { "TryOpenSubKey: ", Key.Name, ", ", Subkey, ": ", source };
                Trace.WriteLine(string.Concat(textArray1));
                return null;
            }
        }
        public static RegistryKey TryOpenSubKey(this RegistryKey Key, string subkey, bool writeable) {
            if ((Key == null) || (subkey == null)) {
                return null;
            }
            try {
                return Key.OpenSubKey(subkey, writeable);
            } catch {
                return null;
            }
        }

        public static string TryGetValue(this RegistryKey Key, string Name) =>
            (Key != null) ? (Key.GetValue(Name) as string) : null;

        public static object TryGetValue(this RegistryKey Key, string Name, object DefaultValue) =>
    Key?.GetValue(Name, DefaultValue);

    }
    public class FileAssociationManager {
        // Fields
        private const int READ = 0x2_0019;
        private const uint LOAD_LIBRARY_AS_DATAFILE = 2;
        private static bool SuppressKeyOutput;

        [DllImport("advapi32.dll", EntryPoint = "RegLoadMUIStringW", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern int RegLoadMUIString(IntPtr hKey, string pszValue, StringBuilder pszOutBuf, int cbOutBuf, out int pcbData, uint Flags, string pszDirectory);

        [DllImport("advapi32.dll", EntryPoint = "RegOpenKeyExW", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int RegOpenKeyEx(IntPtr hKey, string lpSubKey, int ulOptions, int samDesired, out IntPtr phkResult);


        [DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int RegCloseKey(IntPtr hKey);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);


        [DllImport("User32.dll", SetLastError = true)]
        private static extern int LoadString(IntPtr hInstance, int uID, StringBuilder lpBuffer, int nBufferMax);


        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint SearchPath(string lpPath, string lpFileName, string lpExtension, int nBufferLength, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpBuffer, out IntPtr lpFilePart);



        private static string UpOneLevel(string Path) =>
    !Path.Contains(@"\") ? Path : Path.Substring(0, Path.LastIndexOf(@"\"));



        private static RegistryKey UpOneLevel(RegistryKey Key, bool Writeable) {
            if (ReferenceEquals(Key, Registry.LocalMachine) || ReferenceEquals(Key, Registry.CurrentUser)) {
                return Key;
            }
            string str = UpOneLevel(Key.Name);
            return (!str.StartsWith("HKEY_CURRENT_USER") ? (!str.StartsWith("HKEY_LOCAL_MACHINE") ? null : Registry.LocalMachine.OpenSubKey(str.Remove(0, @"HKEY_LOCAL_MACHINE\".Length), Writeable)) : Registry.CurrentUser.OpenSubKey(str.Remove(0, @"HKEY_CURRENT_USER\".Length), Writeable));
        }



        public static string CleanExePath(string Path) {
            if (string.IsNullOrEmpty(Path) || Path.StartsWith("\"%1\"")) {
                return null;
            }
            char[] trimChars = new char[] { ' ', '"' };
            string str = Environment.ExpandEnvironmentVariables(Path.Trim(trimChars)).Replace(@"\\", @"\");
            string str3 = Environment.SystemDirectory + @"\rundll32.exe";
            string str4 = Environment.SystemDirectory + @"\mmc.exe";
            string str5 = Environment.SystemDirectory + @"\shell32.dll";
            if (str.StartsWith(str3, StringComparison.InvariantCultureIgnoreCase)) {
                str = str.Substring(str3.Length);
            } else if (str.StartsWith("rundll32.exe", StringComparison.InvariantCultureIgnoreCase)) {
                str = str.Remove(0, "rundll32.exe".Length);
            } else if (str.StartsWith(str4, StringComparison.InvariantCultureIgnoreCase)) {
                str = str.Substring(str4.Length);
            } else if (str.StartsWith("mmc.exe", StringComparison.InvariantCultureIgnoreCase)) {
                str = str.Remove(0, "mmc.exe".Length);
            }
            char[] chArray2 = new char[] { ' ', '"' };
            str = str.Trim(chArray2);
            if (str.StartsWith(str5, StringComparison.InvariantCultureIgnoreCase)) {
                char[] chArray3 = new char[] { ' ', '"' };
                str = str.Remove(0, str5.Length).Trim(chArray3);
            }
            int startIndex = str.IndexOfAny(System.IO.Path.GetInvalidPathChars());
            if (startIndex > -1) {
                str = str.Remove(startIndex);
            }
            int index = str.IndexOf(" /");
            if (index > -1) {
                str = str.Remove(index);
            }
            string path = str;
            string str6 = Regex.Replace(System.IO.Path.GetExtension(path), "[^A-Z0-9.]", "-", RegexOptions.IgnoreCase);
            if (str6.IndexOf('-') > 0) {
                str6 = str6.Remove(str6.IndexOf('-'));
            }
            int num3 = path.IndexOf(str6, 0, StringComparison.InvariantCultureIgnoreCase) + str6.Length;
            if (num3 < path.Length) {
                path = path.Remove(num3);
            }
            if (path.IndexOf('\\') != -1) {
                return path;
            }
            IntPtr lpFilePart = new IntPtr();
            StringBuilder lpBuffer = new StringBuilder(0xff);
            SearchPath(null, path, null, lpBuffer.Capacity, lpBuffer, out lpFilePart);
            return lpBuffer.ToString();
        }


        public static string CleanExePath(string Path, bool PreserveResourceInfo) {
            string str2;
            if (!PreserveResourceInfo) {
                return CleanExePath(Path);
            }
            if ((Path == null) || Path.StartsWith("\"%1\"")) {
                return null;
            }
            char[] trimChars = new char[] { ' ', '"' };
            string name = Path.Trim(trimChars);
            string str3 = null;
            string str4 = Environment.SystemDirectory + @"\rundll32.exe";
            if (!Environment.ExpandEnvironmentVariables(name).StartsWith(str4, StringComparison.InvariantCultureIgnoreCase)) {
                str2 = name;
            } else {
                char[] chArray2 = new char[] { ' ', '"' };
                str2 = Environment.ExpandEnvironmentVariables(name).Substring(str4.Length).Trim(chArray2);
            }
            if ((str2.IndexOf(".exe", 0, StringComparison.InvariantCultureIgnoreCase) > -1) && !str2.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase)) {
                int startIndex = str2.IndexOf(".exe", 0, StringComparison.InvariantCultureIgnoreCase) + 4;
                if (startIndex < str2.Length) {
                    str2 = str2.Remove(startIndex);
                }
            } else if ((str2.IndexOf(".dll", 0, StringComparison.InvariantCultureIgnoreCase) > -1) && !str2.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase)) {
                int startIndex = str2.IndexOf(".dll", 0, StringComparison.InvariantCultureIgnoreCase) + 4;
                if (startIndex < str2.Length) {
                    str3 = str2.Substring(startIndex);
                    str2 = str2.Remove(startIndex);
                }
            }
            if (str2.IndexOf('\\') != -1) {
                return (str2 + str3);
            }
            IntPtr lpFilePart = new IntPtr();
            StringBuilder lpBuffer = new StringBuilder(0xff);
            SearchPath(null, str2, null, lpBuffer.Capacity, lpBuffer, out lpFilePart);
            return lpBuffer.ToString();
        }






        public static string GetResourceString(RegistryKey Key, string ValueName) {
            if (Environment.OSVersion.Version.Major >= 6) {
                IntPtr ptr;
                if (RegOpenKeyEx(new IntPtr(-2_147_483_646L), Key.Name.Substring(Key.Name.IndexOf(@"\") + 1), 0, 0x2_0019, out ptr) == 0) {
                    int num;
                    StringBuilder pszOutBuf = new StringBuilder(0x400);
                    if (RegLoadMUIString(ptr, ValueName, pszOutBuf, 0x400, out num, 0, null) == 0) {
                        RegCloseKey(ptr);
                        return pszOutBuf.ToString();
                    }
                }
                RegCloseKey(ptr);
            }
            return GetResourceString(Registry.GetValue(Key.ToString(), ValueName, null) as string);
        }


        public static string GetResourceString(string resource_path) {
            if (resource_path == null) {
                return null;
            }
            char[] separator = new char[] { ',' };
            string[] strArray = resource_path.Split(separator);
            string path = string.Empty;
            int result = 0;
            if (strArray.Length != 2) {
                char[] trimChars = new char[] { '@', ' ', '\'', '"' };
                path = Environment.ExpandEnvironmentVariables(resource_path.Trim(trimChars));
            } else {
                char[] trimChars = new char[] { '@', ' ', '\'', '"' };
                path = Environment.ExpandEnvironmentVariables(strArray[0].Trim(trimChars));
                strArray[1] = strArray[1].Trim();
                int length = 1;
                while (true) {
                    if (length < strArray[1].Length) {
                        if (char.IsDigit(strArray[1][length])) {
                            length++;
                            continue;
                        }
                        strArray[1] = strArray[1].Substring(0, length);
                    }
                    char[] chArray3 = new char[] { '@', ' ', '\'', '"' };
                    if (!int.TryParse(strArray[1].Trim(chArray3), out result)) {
                        Trace.WriteLine("GetResourceString: TryParse failed on " + resource_path);
                    }
                    break;
                }
            }
            if (!File.Exists(path)) {
                path = Regex.Replace(path, @"^([a-z]):\\Program Files\\", @"$1:\Program Files (x86)\", RegexOptions.IgnoreCase);
            }
            IntPtr hInstance = LoadLibraryEx(path, IntPtr.Zero, 2);
            StringBuilder lpBuffer = new StringBuilder(0xff);
            if (LoadString(hInstance, Math.Abs(result), lpBuffer, lpBuffer.Capacity) > 0) {
                FreeLibrary(hInstance);
                return lpBuffer.ToString();
            }
            FreeLibrary(hInstance);
            if (path.Contains(@"\")) {
                string str3 = path.Remove(path.LastIndexOf(@"\")) + @"\en-US" + path.Remove(0, path.LastIndexOf(@"\")) + ".mui";
                if (File.Exists(str3)) {
                    IntPtr ptr2 = LoadLibraryEx(str3, IntPtr.Zero, 2);
                    if (LoadString(ptr2, Math.Abs(result), lpBuffer, lpBuffer.Capacity) > 0) {
                        FreeLibrary(ptr2);
                        return lpBuffer.ToString();
                    }
                }
            }
            object obj2 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\MUI\StringCacheSettings", "StringCacheGeneration", null);
            string str2 = $"{obj2:X}";
            if (!string.IsNullOrEmpty(str2)) {
                RegistryKey key2 = Registry.CurrentUser.OpenSubKey(@"Software\Classes\Local Settings\MuiCache\" + str2);
                if (key2 != null) {
                    foreach (string str4 in key2.GetSubKeyNames()) {
                        RegistryKey key3 = key2.OpenSubKey(str4);
                        if (key3 != null) {
                            foreach (string str5 in key3.GetValueNames()) {
                                if (string.Equals(Environment.ExpandEnvironmentVariables(str5), resource_path, StringComparison.OrdinalIgnoreCase)) {
                                    string str6 = key3.GetValue(str5) as string;
                                    if (!string.IsNullOrEmpty(str6)) {
                                        return str6;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache");
            if (key != null) {
                foreach (string str7 in key.GetValueNames()) {
                    if (str7.StartsWith(path, StringComparison.OrdinalIgnoreCase)) {
                        string str8 = key.GetValue(str7) as string;
                        if (!string.IsNullOrEmpty(str8)) {
                            return str8;
                        }
                    }
                }
            }
            if (resource_path.Contains("Program Files (x86)") || resource_path.Contains("%ProgramFiles(x86)%")) {
                string resourceString = GetResourceString(resource_path.Replace("Program Files (x86)", "Program Files").Replace("%ProgramFiles(x86)%", "%ProgramFiles%"));
                if (!string.IsNullOrEmpty(resourceString)) {
                    return resourceString;
                }
            }
            Trace.WriteLine("Could not resolve resource string: " + resource_path);
            return resource_path;
        }


        public static List<RegisteredApplication> GetRegisteredApplications() {
            Trace.WriteLine("GetRegisteredApplications...");
            List<RegisteredApplication> list = new List<RegisteredApplication>();
            RegistryKey key = Registry.LocalMachine.TryOpenSubKey(@"Software\RegisteredApplications");
            if (key == null) {
                return list;
            }
            string[] valueNames = key.GetValueNames();
            int index = 0;
            while (true) {
                while (true) {
                    if (index >= valueNames.Length) {
                        return list;
                    }
                    string name = valueNames[index];
                    string str2 = key.GetValue(name, string.Empty) as string;
                    if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(str2)) {
                        Trace.WriteLine(name + " @ " + str2);
                        RegistryKey key2 = Registry.LocalMachine.OpenSubKey(str2);
                        if (key2 == null) {
                            key2 = Registry.LocalMachine.OpenSubKey(Regex.Replace(str2, @"^\\?Software\\", @"Software\WOW6432Node\", RegexOptions.IgnoreCase));
                            if (key2 == null) {
                                Trace.WriteLine("Unknown Capabilites Key for this Application...");
                                break;
                            }
                        }
                        RegisteredApplication item = new RegisteredApplication(name, str2) {
                            Description = key2.TryGetValue("ApplicationDescription", string.Empty) as string,
                            Name = key2.TryGetValue("ApplicationName", name) as string
                        };
                        RegistryKey key3 = UpOneLevel(key2, false);
                        item.DefaultIconPath = !key3.GetSubKeyNames().Contains<string>("DefaultIcon") ? CleanExePath(key3.TryOpenSubKey(@"shell\open\command").TryGetValue(null), true) : (key3.OpenSubKey("DefaultIcon").GetValue(null) as string);
                        list.Add(item);
                        RegistryKey key4 = key2.OpenSubKey("FileAssociations");
                        if (key4 != null) {
                            item.Capabilities.AddRange(key4.GetValueNames());
                            RegistryKey key5 = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts");
                            key5.GetSubKeyNames();
                            foreach (string str3 in item.Capabilities) {
                                item.ProgIDs.Add(str3, key4.GetValue(str3));
                                RegistryKey key6 = key5.TryOpenSubKey(str3).TryOpenSubKey("UserChoice");
                                if ((key6 != null) && (((string)key4.GetValue(str3)) == ((string)key6.GetValue("ProgID")))) {
                                    item.Controlled.Add(str3);
                                }
                            }
                        }
                    }
                    break;
                }
                index++;
            }

        }
    }




    public class RegisteredApplication {
        // Fields
        private string ResolvedName;
        private string ResolvedDescription;
        public string DefaultIconPath;
        public List<string> Capabilities;
        public List<string> Controlled;
        public Hashtable ProgIDs;
        public string RegPath;

        // Methods
        public RegisteredApplication(string Name, string RegPath) {
            this.Name = Name;
            this.RegPath = RegPath;
            this.Capabilities = new List<string>();
            this.ProgIDs = new Hashtable();
            this.Controlled = new List<string>();
        }

        public override string ToString() =>
            this.DisplayName;

        // Properties
        public string Name { get; set; }

        public string DisplayName {
            get {
                if (this.ResolvedName != null) {
                    return this.ResolvedName;
                }
                if (this.Name == null) {
                    return string.Empty;
                }
                if (this.Name.StartsWith("@")) {
                    this.ResolvedName = FileAssociationManager.GetResourceString(this.Name);
                }
                return ((this.ResolvedName == null) ? this.Name : this.ResolvedName);
            }
        }

        public string Description { get; set; }

        public string DisplayDescription {
            get {
                if (this.ResolvedDescription != null) {
                    return this.ResolvedDescription;
                }
                if (this.Description == null) {
                    return string.Empty;
                }
                if (this.Description.StartsWith("@")) {
                    this.ResolvedDescription = FileAssociationManager.GetResourceString(this.Description);
                }
                return ((this.ResolvedDescription == null) ? this.Description : this.ResolvedDescription);
            }
        }
    }

}


