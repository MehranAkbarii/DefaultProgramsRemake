using DefaultPrograms.Properties;
using FileAssociationLibrary;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Management.Deployment;

namespace FileAssociationLiberary.Internal {


    internal class Helper {

        public static string getPackageNameFromFamilyName(string famillyName) {
            PackageManager packageManager = new PackageManager();
            var packages = packageManager.FindPackagesForUser(string.Empty, famillyName);
            foreach (var package in packages) {
                if (package.DisplayName.Equals("Windows Media Player"))
                    return "Media Player";
                return package.DisplayName;
            }
            return string.Empty;
        }
        public static string GetProgramFriendlyName(string p) {
            string str3;
            if (string.IsNullOrEmpty(p)) {
                return string.Empty;
            }
            string str = FileAssociationManager.CleanExePath(p);
            if (string.IsNullOrEmpty(str)) {
                return p;
            }
            try {
                if (!File.Exists(str)) {
                    str3 = (p != "\"%1\" %*") ? string.Empty : "Windows Application";
                } else {
                    string fileDescription = FileVersionInfo.GetVersionInfo(str).FileDescription;
                    str3 = !string.IsNullOrEmpty(fileDescription) ? fileDescription : Path.GetFileName(str);
                }
            } catch {
                Trace.WriteLine("GetProgramFriendlyName: Couldn't find name of: " + p);
                str3 = !File.Exists(str) ? string.Empty : Path.GetFileName(str);
            }
            return str3;
        }
    }
}
