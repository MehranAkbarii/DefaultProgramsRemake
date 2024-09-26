using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Management.Deployment;
using Windows.System;
using FileAssociationLibrary;
using FileAssociationLiberary.Internal;

namespace DefaultPrograms {
    public partial class Form1 : Form {
        [DllImport("shell32.dll")]
        static extern bool SHObjectProperties(IntPtr hWnd, int shopObjectType,
    [MarshalAs(UnmanagedType.LPWStr)] string pszObjectName,
    [MarshalAs(UnmanagedType.LPWStr)] string pszPropertyPage);

        [DllImport("user32.dll")]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentProcessId();

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        [DllImport("user32.dll")]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll")]
        static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        const string CLASSNANE_DIALOG = "#32770";
        const int SHOP_FILEPATH = 0x2;
        const int MAX_PATH = 260;
        const int GWL_EXSTYLE = -20;
        const int WS_EX_LAYERED = 0x80000;
        const int LWA_ALPHA = 0x2;
        const int ID_APPLY = 0x3021;
        const int WM_CLOSE = 0x010;
        const int VK_LMENU = 0xA4;
        const int VK_C = 0x43;
        const int KEYEVENTF_KEYUP = 0x2;
        private PackageManager packageManager;
        private FileAssociationManager assocManager;
        List<RegisteredApplication> registeredPrograms;

        public Form1() {
            InitializeComponent();
            packageManager = new PackageManager();
            assocManager = new FileAssociationManager();
            loadUWPApps();
            loadPrograms();
            listViewUWPApps.Columns.Add("Apps").Width = 200;
            listViewFileExtensions.Columns.Add("Extensions").Width = 220;
            listViewFileExtensions.Columns.Add("Description").Width = 300;
            listViewFileExtensions.Columns.Add("Current Defaults").Width = 300;
        }

        private async void loadUWPApps() {
            var packages = packageManager.FindPackagesForUser(string.Empty);
            foreach (var package in packages) {
                if (package != null) {
                    var installedLocation = package.InstalledLocation;
                    if (installedLocation != null) {
                        // Get the path to the AppxManifest.xml file
                        string manifestPath = Path.Combine(installedLocation.Path, "AppxManifest.xml");

                        // Parse the AppxManifest.xml file
                        XDocument doc = XDocument.Load(manifestPath);

                        // Get the file extensions from the manifest
                        var haveExtensions = doc.Descendants()
                                        .Where(e => e.Name.LocalName == "SupportedFileTypes")
                                        .Elements()
                                        .Select(e => e.Value).Any();
                        if (haveExtensions) {
                            if ((!package.DisplayName.Equals("File Explorer")) && (!package.DisplayName.Equals("Web Media Extensions"))) {
                                if (package.DisplayName.Equals("Windows Media Player")) {
                                    ListViewItem listitem = new ListViewItem("Media Player");
                                    listitem.SubItems.Add(package.Id.FullName);
                                    listViewUWPApps.Items.Add(listitem);
                                } else {
                                    ListViewItem item = new ListViewItem(package.DisplayName);
                                    item.SubItems.Add(package.Id.FullName);
                                    listViewUWPApps.Items.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }


        private async Task<Package> findPackageByFullName(string packageFullName) {
            PackageManager packageManager = new PackageManager();
            var packages = packageManager.FindPackagesForUser(string.Empty);

            foreach (var package in packages) {
                if (package.Id.FullName == packageFullName) {
                    return package;
                }
            }

            return null;
        }


        private async Task displayUwpFileExtensions(string packageFullName) {
            PackageManager packageManager = new PackageManager();

            // Find the package
            var package = await findPackageByFullName(packageFullName);

            if (package != null) {
                var installedLocation = package.InstalledLocation;

                if (installedLocation != null) {
                    // Get the path to the AppxManifest.xml file
                    string manifestPath = Path.Combine(installedLocation.Path, "AppxManifest.xml");

                    // Parse the AppxManifest.xml file
                    XDocument doc = XDocument.Load(manifestPath);

                    List<string> associations = new List<string>();

                    // Get the file extensions from the manifest
                    var extensions = doc.Descendants()
                                    .Where(e => e.Name.LocalName == "SupportedFileTypes")
                                    .Elements()
                                    .Select(e => e.Value).ToList();

                    //Get  Protocols from the manifast 
                    var protocols = doc.Descendants()
                           .Where(e => e.Name.LocalName == "Protocol")
                           .Select(e => e.Attribute("Name").Value)
                           .ToList();

                    associations.AddRange(protocols);
                    associations.AddRange(extensions);

                    // Create a ListViewItem for the app
                    foreach (var association in associations) {
                        if ((association != "") && (association != "*")) {
                            ListViewItem item = new ListViewItem(association);
                            if (association.StartsWith(".")) {
                                item.SubItems.Add(AppAssociation.getAssocDescriptionInfo(assocManager, association));
                                item.SubItems.Add(Helper.customizedHandlers(AppAssociation.GetDefaultHandler(assocManager, association)));
                            } else {
                                //get protocols info
                                item.SubItems.Add(AppAssociation.getProtocolDiscription(association));
                            }
                            // Add the ListViewItem to the ListView
                            listViewFileExtensions.Items.Add(item);
                        }

                    }

                }
            }
        }
        public static void showOpenWithDialog(string extension) {
            string fileName = Guid.NewGuid().ToString() + extension;
            string filePath = Path.Combine(Path.GetTempPath(), fileName);
            File.WriteAllText(filePath, string.Empty);
            bool found = false;
            uint currentId = GetCurrentProcessId();
            bool func(IntPtr hWnd, IntPtr lparam) {
                GetWindowThreadProcessId(hWnd, out uint id);
                if (id == currentId) {
                    var sb = new StringBuilder(MAX_PATH);
                    if (GetClassName(hWnd, sb, sb.Capacity) != 0 && sb.ToString() == CLASSNANE_DIALOG) {
                        if (GetWindowText(hWnd, sb, sb.Capacity) != 0 && sb.ToString().Contains(fileName)) {
                            found = true;
                            do { ShowWindow(hWnd, 0); }
                            while (IsWindowVisible(hWnd));
                            SetWindowLong(hWnd, GWL_EXSTYLE, GetWindowLong(hWnd, GWL_EXSTYLE) ^ WS_EX_LAYERED);
                            SetLayeredWindowAttributes(hWnd, 0, 0, LWA_ALPHA);
                            Task.Run(() => {
                                IntPtr applyHandle = GetDlgItem(hWnd, ID_APPLY);
                                while (!IsWindowEnabled(applyHandle)) Thread.Sleep(100);
                                SendMessage(hWnd, WM_CLOSE, 0, 0);
                            });
                            SetForegroundWindow(hWnd);
                            keybd_event(VK_LMENU, 0, 0, 0);
                            keybd_event(VK_C, 0, 0, 0);
                            keybd_event(VK_LMENU, 0, KEYEVENTF_KEYUP, 0);
                            keybd_event(VK_C, 0, KEYEVENTF_KEYUP, 0);
                            File.Delete(filePath);
                        }
                    }
                }
                return true;
            }
            Task.Run(() => {
                while (!found) {
                    EnumWindows(func, IntPtr.Zero);
                    Thread.Sleep(50);
                }
            });
            SHObjectProperties(IntPtr.Zero, SHOP_FILEPATH, filePath, null);
        }

        private void listViewFileExtensions_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (listViewFileExtensions.SelectedItems.Count > 0) {
                string str = listViewFileExtensions.SelectedItems[0].Text;
                if (str.StartsWith("."))
                    showOpenWithDialog(str);
                else {
                    showSelectProtocolDialog(str);
                }
            }
        }

        private void showSelectProtocolDialog(string protocolToFind) {
            var packages = packageManager.FindPackagesForUser(string.Empty);
            Dictionary<string, string> appnames = new Dictionary<string, string>();

            foreach (var package in packages) {
                if (package != null) {
                    var installedLocation = package.InstalledLocation;
                    if (installedLocation != null) {
                        // Get the path to the AppxManifest.xml file
                        string manifestPath = Path.Combine(installedLocation.Path, "AppxManifest.xml");

                        // Parse the AppxManifest.xml file
                        XDocument doc = XDocument.Load(manifestPath);

                        var protocolElements = doc.Descendants()
                        .Where(e => e.Name.LocalName == "Protocol")
                        .Where(e => e.Attribute("Name")?.Value == protocolToFind);

                        if (protocolElements.Any()) {
                            string appName = package.DisplayName;
                            string familyName = package.Id.FamilyName;
                            appnames.Add(appName, familyName);
                        }
                    }
                }
            }
            //open Dialog

            //I should send protocol for change 
            new SelectProtocolDialog(appnames, protocolToFind).Show();
        }


        private void listViewUWPApps_MouseClick(object sender, MouseEventArgs e) {
            listViewFileExtensions.Clear();
            if (listViewUWPApps.SelectedItems[0].SubItems.Count > 1) {
                displayUwpFileExtensions(listViewUWPApps.SelectedItems[0].SubItems[1].Text);
            } else {
                displayProgramExtensions(listViewUWPApps.SelectedItems[0].Text);
            }
            listViewFileExtensions.Columns.Add("Extensions").Width = 220;
            listViewFileExtensions.Columns.Add("Description").Width = 300;
            listViewFileExtensions.Columns.Add("Current Defaults").Width = 300; ;
        }

        private void buttonClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void loadPrograms() {
            registeredPrograms = FileAssociationManager.GetRegisteredApplications();
            foreach (RegisteredApplication application in this.registeredPrograms) {
                if ((!application.DisplayName.Equals("File Explorer")) && (!application.DisplayName.Equals("Windows Search Explorer")))
                    listViewUWPApps.Items.Add(application.DisplayName);
            }
        }
        private void displayProgramExtensions(string programName) {
            RegisteredApplication registeredProgram = registeredPrograms.Single((registeredProgram) => registeredProgram.DisplayName.Equals(programName));
            List<String> extensions = registeredProgram.Capabilities;
            foreach (var extension in extensions) {
                if ((extension != "") && (extension != "*")) {
                    ListViewItem item = new ListViewItem(extension);
                    item.SubItems.Add(AppAssociation.getAssocDescriptionInfo(assocManager, extension));
                    item.SubItems.Add(Helper.customizedHandlers(AppAssociation.GetDefaultHandler(assocManager, extension)));
                    listViewFileExtensions.Items.Add(item);
                }
            }
        }
    }
}
