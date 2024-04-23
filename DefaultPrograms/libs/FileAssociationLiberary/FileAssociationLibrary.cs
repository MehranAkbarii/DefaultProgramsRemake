using Microsoft.Win32;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml;
using System.Linq;
using System.Runtime.CompilerServices;
using OpenXmlPowerTools;
using FileAssociationLiberary.Internal;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.VisualBasic.Devices;
using System.Drawing;

namespace FileAssociationLibrary {




    public class IconInfo {
        // Fields
        public IconInfo LesserPrecedenceIcon;
        public string ResourcePath;
    }


    public class ApplicationInfo {
        // Methods
        public ApplicationInfo() {
        }

        public ApplicationInfo(Verb OpenWithVerb, bool isNoOpenWith) {
            this.OpenWithVerb = OpenWithVerb;
            this.isNoOpenWith = isNoOpenWith;
        }

        public override string ToString() =>
            this.OpenWithVerb.ToString();

        // Properties
        public string KeyLocation { get; set; }

        public string KeyName { get; set; }

        public string ValueName { get; set; }

        public Verb OpenWithVerb { get; set; }

        public bool isNoOpenWith { get; set; }
    }




    public class DropTargetInfo {
        // Fields
        private string _friendlytargetname;
        private string _targetobjectpath;
        private string _defaulticonpath;

        // Methods
        public DropTargetInfo(RegistryKey Key, string CLSID) {
            this.Key = Key;
            this.CLSID = CLSID;
        }

        public override string ToString() =>
            this.FriendlyTargetName;

        // Properties
        public string CLSID { get; private set; }

        public string FriendlyTargetName {
            get {
                this._friendlytargetname ??= (Registry.GetValue(@"HKEY_CLASSES_ROOT\CLSID\" + this.CLSID, string.Empty, string.Empty) as string);
                return this._friendlytargetname;
            }
        }

        public string TargetObjectPath {
            get {
                if (this._targetobjectpath == null) {
                    RegistryKey key = Registry.ClassesRoot.TryOpenSubKey("CLSID").TryOpenSubKey(this.CLSID);
                    string str = key.TryOpenSubKey("InProcServer32").TryGetValue(string.Empty);
                    if (str != null) {
                        this._targetobjectpath = str;
                    } else {
                        string str2 = key.TryOpenSubKey("LocalServer32").TryGetValue(string.Empty);
                        this._targetobjectpath = str2;
                    }
                }
                return this._targetobjectpath;
            }
        }

        public string DefaultIconPath {
            get {
                if (this._defaulticonpath == null) {
                    RegistryKey key = Registry.ClassesRoot.TryOpenSubKey("CLSID").TryOpenSubKey(this.CLSID);
                    this._defaulticonpath = key.TryOpenSubKey("DefaultIcon").TryGetValue(string.Empty);
                }
                return this._targetobjectpath;
            }
        }

        public RegistryKey Key { get; private set; }
    }



    public class DDEInfo {
        // Methods
        public DDEInfo(string Message, string Application, string Topic) {
            this.Message = Message;
            this.Application = Application;
            this.Topic = Topic;
        }

        public override string ToString() =>
            this.Message;

        // Properties
        public string Message { get; set; }

        public string Application { get; set; }

        public string Topic { get; set; }
    }





    public enum RegistryLocation {
        System,
        SystemExtension,
        SystemProgID,
        SystemPerceivedType,
        SystemBaseClass,
        SystemAllFSObjects,
        User,
        UserExtension,
        UserProgID,
        UserPerceivedType,
        UserBaseClass,
        UserAllFSObjects,
        None,
        BothSystemAndUser
    }

    public class VerbList : ICloneable {
        // Fields
        public List<Verb> Verbs = new List<Verb>();
        private string _defaultverbkeyname;
        private Verb _defaultverb;
        public RegistryLocation HomogenousVerbsLocation = RegistryLocation.None;

        // Methods
        public void Add(Verb item) {
            this.Verbs.Add(item);
        }

        public object Clone() {
            Verb[] array = new Verb[this.Verbs.Count];
            this.Verbs.CopyTo(array);
            return (base.MemberwiseClone() as VerbList);
        }

        public override string ToString() =>
            this.DefaultVerbKeyName + ", " + this.Verbs.Count.ToString() + " verbs";

        internal bool VerbExists(string p) =>
            this.Verbs.Any<Verb>(verb => string.Equals(verb.KeyName, p, StringComparison.InvariantCultureIgnoreCase));

        // Properties
        public string DefaultVerbKeyName {
            get =>
                this._defaultverbkeyname;
            set =>
                this._defaultverbkeyname = value;
        }

        public Verb DefaultVerb {
            get {
                if (this._defaultverb == null) {
                    foreach (Verb verb in this.Verbs) {
                        if (string.Equals(verb.KeyName, this.DefaultVerbKeyName, StringComparison.InvariantCultureIgnoreCase)) {
                            this._defaultverb = verb;
                        }
                    }
                }
                return this._defaultverb;
            }
        }
    }


    public class DelegateExecuteInfo {
        // Fields
        private string _friendlytargetname;
        private string _targetobjectpath;

        // Methods
        public DelegateExecuteInfo(RegistryKey Key, string CLSID) {
            this.Key = Key;
            this.CLSID = CLSID;
        }

        public override string ToString() =>
            this.FriendlyTargetName;

        // Properties
        public string CLSID { get; private set; }

        public string FriendlyTargetName {
            get {
                this._friendlytargetname ??= (Registry.GetValue(@"HKEY_CLASSES_ROOT\CLSID\" + this.CLSID, string.Empty, string.Empty) as string);
                return this._friendlytargetname;
            }
        }

        public string TargetObjectPath {
            get {
                if (this._targetobjectpath == null) {
                    RegistryKey key = Registry.ClassesRoot.TryOpenSubKey("CLSID").TryOpenSubKey(this.CLSID);
                    string str = key.TryOpenSubKey("InProcServer32").TryGetValue(string.Empty);
                    if (str != null) {
                        this._targetobjectpath = str;
                    } else {
                        string str2 = key.TryOpenSubKey("LocalServer32").TryGetValue(string.Empty);
                        this._targetobjectpath = str2;
                    }
                }
                return this._targetobjectpath;
            }
        }

        public RegistryKey Key { get; private set; }
    }




    [Serializable, DebuggerDisplay("{_key} @ {Location}")]
    public class Verb : ICloneable, IXmlSerializable {
        // Fields
        private string _key;
        public string DefaultValue;
        public string MUIVerb;
        public string Command;
        public RegistryKey LocationKey;
        public RegistryLocation Location;
        public Verb LesserPrecedenceHorizontal;
        public Verb LesserPrecedenceVertical;
        public bool isShellExtension;
        public DropTargetInfo DropTarget;
        private DropTargetInfo _shelldroptarget;
        public DDEInfo DDE;
        private DDEInfo _shelldde;
        public DelegateExecuteInfo DelegateExecute;
        private DelegateExecuteInfo _shelldelegateexecute;
        public string IconPath;
        private string _iconpath;
        private Dictionary<string, string> CanonicalVerbNameMap;

        // Methods
        public Verb() {
            this.isShellExtension = false;
            this.isVisible = true;
            this.isNeverDefault = false;
            this.CanonicalVerbNameMap = this.LoadCanonicalVerbNameMap();
        }

        public Verb(string Key, string Value, string MUIVerb, string Command, string IconPath, DropTargetInfo DropTarget, DDEInfo DDE, DelegateExecuteInfo DelegateExecute, RegistryKey LocationKey, RegistryLocation Location) {
            this.KeyName = Key;
            this.DefaultValue = Value;
            this.MUIVerb = MUIVerb;
            this.Command = Command;
            this.IconPath = IconPath;
            this.LocationKey = LocationKey;
            this.Location = Location;
            this.isShellExtension = false;
            this.DropTarget = DropTarget;
            this.DelegateExecute = DelegateExecute;
            this.DDE = DDE;
            this.isVisible = true;
            this.isNeverDefault = false;
            this.CanonicalVerbNameMap = this.LoadCanonicalVerbNameMap();
        }

        public object Clone() =>
            base.MemberwiseClone();

        public XmlSchema GetSchema() =>
            null;

        public string GetShellCommandPath() {
            Verb objA = this;
            string command = null;
            while (true) {
                if (objA != null) {
                    if (!string.IsNullOrEmpty(objA.Command)) {
                        command = objA.Command;
                    }
                    if (command == null) {
                        objA = objA.LesserPrecedenceVerb;
                        if (!ReferenceEquals(objA, this)) {
                            continue;
                        }
                    }
                }
                return command;
            }
        }

        public DDEInfo GetShellDDE() {
            if (this._shelldde == null) {
                Verb objA = this;
                DDEInfo dDE = null;
                while (true) {
                    if (objA != null) {
                        if (objA.DDE != null) {
                            if (dDE == null) {
                                dDE = objA.DDE;
                            } else {
                                string message = dDE.Message;
                                string text4 = message;
                                if (message == null) {
                                    string local1 = message;
                                    text4 = objA.DDE.Message;
                                }
                                dDE.Message = text4;
                                string application = dDE.Application;
                                string text5 = application;
                                if (application == null) {
                                    string local2 = application;
                                    text5 = objA.DDE.Application;
                                }
                                dDE.Application = text5;
                                string topic = dDE.Topic;
                                string text6 = topic;
                                if (topic == null) {
                                    string local3 = topic;
                                    text6 = objA.DDE.Topic;
                                }
                                dDE.Topic = text6;
                            }
                        }
                        if (dDE == null) {
                            objA = objA.LesserPrecedenceVerb;
                            if (!ReferenceEquals(objA, this)) {
                                continue;
                            }
                        }
                    }
                    this._shelldde = dDE;
                    break;
                }
            }
            return this._shelldde;
        }

        public DelegateExecuteInfo GetShellDelegateExecute() {
            if (this._shelldelegateexecute == null) {
                Verb objA = this;
                DelegateExecuteInfo delegateExecute = null;
                while (true) {
                    if (objA != null) {
                        if (objA.DelegateExecute != null) {
                            delegateExecute ??= objA.DelegateExecute;
                        }
                        if (delegateExecute == null) {
                            objA = objA.LesserPrecedenceVerb;
                            if (!ReferenceEquals(objA, this)) {
                                continue;
                            }
                        }
                    }
                    this._shelldelegateexecute = delegateExecute;
                    break;
                }
            }
            return this._shelldelegateexecute;
        }

        public string GetShellDisplayText() {
            Verb objA = this;
            string mUIVerb = null;
            while (true) {
                if (objA != null) {
                    if (!string.IsNullOrEmpty(objA.MUIVerb)) {
                        mUIVerb = objA.MUIVerb;
                    }
                    if (mUIVerb == null) {
                        objA = objA.LesserPrecedenceVerb;
                        if (!ReferenceEquals(objA, this)) {
                            continue;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(mUIVerb)) {
                    string resourceString = FileAssociationManager.GetResourceString(mUIVerb);
                    if (!resourceString.StartsWith("@") || (resourceString != mUIVerb)) {
                        return resourceString;
                    }
                }
                objA = this;
                string defaultValue = null;
                while (true) {
                    if (objA != null) {
                        if (!string.IsNullOrEmpty(objA.DefaultValue)) {
                            defaultValue = objA.DefaultValue;
                        }
                        if (defaultValue == null) {
                            objA = objA.LesserPrecedenceVerb;
                            if (!ReferenceEquals(objA, this)) {
                                continue;
                            }
                        }
                    }
                    return (string.IsNullOrEmpty(defaultValue) ? ((string.IsNullOrEmpty(this.KeyName) || !this.CanonicalVerbNameMap.ContainsKey(this.KeyName.ToLower())) ? this.KeyName : this.CanonicalVerbNameMap[this.KeyName.ToLower()]) : defaultValue);
                }
            }
        }

        public DropTargetInfo GetShellDropTarget() {
            if (this._shelldroptarget == null) {
                Verb objA = this;
                DropTargetInfo dropTarget = null;
                while (true) {
                    if (objA != null) {
                        if ((objA.DropTarget != null) && (objA.DropTarget.CLSID != null)) {
                            dropTarget ??= objA.DropTarget;
                        }
                        if (dropTarget == null) {
                            objA = objA.LesserPrecedenceVerb;
                            if (!ReferenceEquals(objA, this)) {
                                continue;
                            }
                        }
                    }
                    this._shelldroptarget = dropTarget;
                    break;
                }
            }
            return this._shelldroptarget;
        }

        public string GetShellIconPath() {
            Verb objA = this;
            string iconPath = null;
            while (true) {
                if (objA != null) {
                    if (objA.IconPath != null) {
                        iconPath = objA.IconPath;
                    }
                    if (iconPath == null) {
                        objA = objA.LesserPrecedenceVerb;
                        if (!ReferenceEquals(objA, this)) {
                            continue;
                        }
                    }
                }
                return iconPath;
            }
        }

        private Dictionary<string, string> LoadCanonicalVerbNameMap() {
            Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
            dictionary1.Add("play", "Play");
            dictionary1.Add("edit", "Edit");
            dictionary1.Add("open", "Open");
            dictionary1.Add("print", "Print");
            dictionary1.Add("printto", "Print");
            dictionary1.Add("runas", "Run as administrator");
            return dictionary1;
        }

        public void ReadXml(XmlReader reader) {
            reader.ReadStartElement();
            this.KeyName = reader.ReadElementString("Key");
            string str = reader.ReadElementString("LocationKey");
            char[] separator = new char[] { '\\' };
            this.LocationKey = (str.Split(separator)[0].ToUpper() == "HKEY_LOCAL_MACHINE") ? Registry.LocalMachine : Registry.CurrentUser;
            this.LocationKey = this.LocationKey.OpenSubKey(str.Substring(str.IndexOf('\\') + 1));
            this.Location = FileAssociationManager.KeyToLocation(this.LocationKey);
            this.DefaultValue = reader.ReadElementString("DefaultValue");
            this.MUIVerb = reader.ReadElementString("MuiVerb");
            this.Command = reader.ReadElementString("Command");
            reader.ReadEndElement();
        }

        public override string ToString() =>
            this.GetShellDisplayText();

        public void WriteXml(XmlWriter writer) {
            writer.WriteElementString("Key", this.KeyName);
            writer.WriteElementString("LocationKey", (this.LocationKey == null) ? string.Empty : this.LocationKey.ToString());
            writer.WriteElementString("DefaultValue", this.DefaultValue);
            writer.WriteElementString("MuiVerb", this.MUIVerb);
            writer.WriteElementString("Command", this.Command);
        }

        // Properties
        public string KeyName {
            get =>
                this._key;
            set =>
                this._key = value;
        }

        public Verb LesserPrecedenceVerb =>
            (this.LesserPrecedenceHorizontal != null) ? this.LesserPrecedenceHorizontal : this.LesserPrecedenceVertical;

        public bool isAutoplayDropTarget =>
            this.DropTarget != null;

        public bool isVisible { get; internal set; }

        public bool isNeverDefault { get; internal set; }

        public bool isEnabled { get; set; }
    }




    public class ExtensionInfo : IComparable<ExtensionInfo> {
        // Fields
        public RegistryLocation Location;
        private string _percievedType;
        private FileTypeDescription _descriptioninfo;
        private VerbList _shellexverbview;
        private VerbList _verbview;
        private Dictionary<RegistryLocation, VerbList> _rawverblist;
        private bool iconpathisnull;
        private string _iconresourcepath;
        public List<ApplicationInfo> OpenWithList;

        // Methods
        public ExtensionInfo() {
            this.OpenWithList = new List<ApplicationInfo>();
        }

        public ExtensionInfo(string Extension, ProgID ProgID) {
            this.OpenWithList = new List<ApplicationInfo>();
            this.Extension = Extension;
            this.ProgID = ProgID;
        }

        public ExtensionInfo(string Extension, ProgID ProgID, string PerceivedType) {
            this.OpenWithList = new List<ApplicationInfo>();
            this.Extension = Extension;
            this.ProgID = ProgID;
            this.PerceivedType = PerceivedType;
        }

        public void RefreshVerbView() {
            this._verbview = null;
        }

        int IComparable<ExtensionInfo>.CompareTo(ExtensionInfo other) =>
            this.Extension.CompareTo(other.Extension);

        public override string ToString() =>
            this.Extension;

        // Properties
        public string Extension { get; set; }

        public ProgID ProgID { get; set; }

        public string PerceivedType {
            get {
                if (this._percievedType == null) {
                    this._percievedType = FileAssociationManager.GetPerceivedType(this.Extension);
                    this._percievedType ??= string.Empty;
                }
                return this._percievedType;
            }
            set =>
                this._percievedType = value;
        }

        public FileTypeDescription DescriptionInfo {
            get {
                this._descriptioninfo ??= FileAssociationManager.GetTypeDescriptionView(this);
                return this._descriptioninfo;
            }
            set =>
                this._descriptioninfo = value;
        }



        public VerbList ContextMenuShellExtensions {
            get {
                this._shellexverbview ??= FileAssociationManager.GetContextMenuShellExtensions(this);
                return this._shellexverbview;
            }
            set =>
                this._shellexverbview = value;
        }

        public VerbList ContextMenuVerbs {
            get {
                this._verbview ??= FileAssociationManager.GetContextMenu(this);
                return this._verbview;
            }
            set =>
                this._verbview = value;
        }

        public Dictionary<RegistryLocation, VerbList> RawVerbList {
            get {
                this._rawverblist ??= FileAssociationManager.GetRawVerbList(this);
                return this._rawverblist;
            }
        }


        public string IconResourcePath {
            get {
                if ((this._iconresourcepath == null) && !this.iconpathisnull) {
                    this._iconresourcepath = new FileAssociationManager().GetIconView(this);
                }
                if (this._iconresourcepath == null) {
                    this.iconpathisnull = true;
                }
                return this._iconresourcepath;
            }
            set {
                if (value == null) {
                    this.iconpathisnull = true;
                }
                this._iconresourcepath = value;
            }
        }
    }



    public enum AssociationType {
        FileExtension,
        UrlProtocol,
        StartMenuClient,
        MimeType
    }



    public class FileTypeDescription {
        // Fields
        public string Data;
        public FileTypeDescription LesserPrecedenceDescription;
        [NonSerialized]
        public RegistryKey LocationKey;

        // Methods
        public FileTypeDescription() {
        }

        public FileTypeDescription(string Name, string Data) {
            this.Name = Name;
            this.Data = Data;
        }

        public FileTypeDescription(RegistryKey LocationKey, string Name, string Data) {
            this.LocationKey = LocationKey;
            this.Name = Name;
            this.Data = Data;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);
        [DllImport("User32.dll")]
        private static extern int LoadString(IntPtr hInstance, int uID, StringBuilder lpBuffer, int nBufferMax);
        public override string ToString() =>
            this.Description;

        // Properties
        public string Description {
            get =>
                (this.Data != null) ? (!this.Data.StartsWith("@") ? this.Data : FileAssociationManager.GetResourceString(this.Data)) : string.Empty;
            set =>
                this.Data = value;
        }

        public string Name {
            get =>
                this.LocationKey?.Name.Substring((int)(this.LocationKey?.Name.LastIndexOf('\\')));
            set {
            }
        }
    }



    public static class AppAssociation {

        internal const string CLSID_Application_Registration = "591209c7-767b-42b2-9fba-44ee4615f2c7";
        static IApplicationAssociationRegistration? registrationManager;
        static IApplicationAssociationRegistrationInternal? registrationManagerInternal;


        static AppAssociation() {
            object appRegistration = Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(CLSID_Application_Registration)));
            registrationManager = appRegistration as IApplicationAssociationRegistration;
            registrationManagerInternal = appRegistration as IApplicationAssociationRegistrationInternal;
        }

        public static string getAssocDescriptionInfo(FileAssociationManager assocManager, string association) {
            ExtensionInfo extensionInfo = assocManager.GetExtensionInfo(association);
            string extDescInfo = extensionInfo.DescriptionInfo.Description;
            if (extDescInfo == "" || extDescInfo.StartsWith("@") || extDescInfo.Equals("Windows Photo Viewer")) {
                string[] parts = association.Split(".");
                return parts[1].ToUpper() + " File";
            }
            return extDescInfo;
        }



        public static string GetDefaultHandler(FileAssociationManager assocManager, string association, AssociationType type = AssociationType.FileExtension) {
            if (registrationManagerInternal == null)
                throw new PlatformNotSupportedException();
            ExtensionInfo extensionInfo = assocManager.GetExtensionInfo(association);
            string programFriendlyName = "Unknown application";
            if (extensionInfo.ContextMenuVerbs.DefaultVerb == null) {
                string str = @"SOFTWARE\Classes\" + association + @"\OpenWithProgids";
                RegistryKey key = Registry.CurrentUser.OpenSubKey(str);
                string assoc = "";
                if (key != null) {
                    assoc = (key.GetValueNames().Length > 0) ? key.GetValueNames()[0] : "";
                    if (assoc.StartsWith("AppX")) {
                        return getUWPappNameFromProgID(assoc);
                    } else if (assoc == "") {
                        str = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\" + association + @"\OpenWithProgids";
                        key = Registry.CurrentUser.OpenSubKey(str);
                        if (key != null) {
                            assoc = (key.GetValueNames().Length > 1) ? key.GetValueNames()[1] : "";
                            if (assoc.StartsWith("AppX")) {
                                return getUWPappNameFromProgID(assoc);
                            } else {
                                str = @"SOFTWARE\Classes\" + assoc + @"\shell\Open\command";
                                key = Registry.LocalMachine.OpenSubKey(str);
                                if (key != null) {
                                    string exepath = key.GetValue("") as string;
                                    exepath = FileAssociationManager.CleanExePath(exepath, false);
                                    string exeName = Helper.GetProgramFriendlyName(exepath);
                                    if (exeName == "") {
                                        return programFriendlyName;
                                    }
                                    return exeName;
                                }
                            }
                        }
                    } else {
                        str = @"SOFTWARE\Classes\" + assoc + @"\shell\Open\command";
                        key = Registry.LocalMachine.OpenSubKey(str);
                        if (key != null) {
                            string exepath = key.GetValue("") as string;
                            exepath = FileAssociationManager.CleanExePath(exepath, false);
                            string exeName = Helper.GetProgramFriendlyName(exepath);
                            if (exeName == "") {
                                return programFriendlyName;
                            }
                            return exeName;
                        } else {
                            str = association + @"\OpenWithProgids";
                            key = Registry.ClassesRoot.OpenSubKey(str);
                            if (key != null) {
                                assoc = (key.GetValueNames().Length > 0) ? key.GetValueNames()[0] : "";
                                if (assoc.StartsWith("AppX")) {
                                    return getUWPappNameFromProgID(assoc);
                                } else {
                                    str = assoc + @"\shell\Open\command";
                                    key = Registry.ClassesRoot.OpenSubKey(str);
                                    if (key != null) {
                                        string exepath = key.GetValue("") as string;
                                        exepath = FileAssociationManager.CleanExePath(exepath, false);
                                        string exeName = Helper.GetProgramFriendlyName(exepath);
                                        if (exeName == "") {
                                            return programFriendlyName;
                                        }
                                        return exeName;
                                    }
                                }
                            }
                        }
                    }
                } else {
                    str = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\" + association + @"\OpenWithProgids";
                    key = Registry.CurrentUser.OpenSubKey(str);
                    if (key != null) {
                        assoc = (key.GetValueNames().Length > 1) ? key.GetValueNames()[1] : "";
                        if (assoc.StartsWith("AppX")) {
                            return getUWPappNameFromProgID(assoc);
                        } else {
                            str = @"SOFTWARE\Classes\" + assoc + @"\shell\Open\command";
                            key = Registry.LocalMachine.OpenSubKey(str);
                            if (key != null) {
                                string exepath = key.GetValue("") as string;
                                exepath = FileAssociationManager.CleanExePath(exepath, false);
                                string exeName = Helper.GetProgramFriendlyName(exepath);
                                if (exeName == "") {
                                    return programFriendlyName;
                                }
                                return exeName;
                            }
                        }
                    } else {
                        str = association + @"\OpenWithProgids";
                        key = Registry.ClassesRoot.OpenSubKey(str);
                        if (key != null) { 
                        assoc = (key.GetValueNames().Length > 0) ? key.GetValueNames()[0] : "";
                            if (assoc.StartsWith("AppX")) {
                                return getUWPappNameFromProgID(assoc);
                            } else {
                                str = assoc + @"\shell\Open\command";
                                key = Registry.ClassesRoot.OpenSubKey(str);
                                if (key != null) {
                                    string exepath = key.GetValue("") as string;
                                    exepath = FileAssociationManager.CleanExePath(exepath, false);
                                    string exeName = Helper.GetProgramFriendlyName(exepath);
                                    if (exeName == "") {
                                        return programFriendlyName;
                                    }
                                    return exeName;
                                }
                            }
                        }
                    }

                }
            } else if (!string.IsNullOrEmpty(extensionInfo.ContextMenuVerbs.DefaultVerb.Command)) {
                string exeName = Helper.GetProgramFriendlyName(FileAssociationManager.CleanExePath(extensionInfo.ContextMenuVerbs.DefaultVerb.Command, false));
                if (exeName == "") {
                    return programFriendlyName;
                }
                return exeName;
            } else {
                registrationManagerInternal.QueryCurrentDefault(association, type, AssociationLevel.User, out var info);
                if (info != null) {
                    if (info.StartsWith("AppX")) {
                        return getUWPappNameFromProgID(info);
                    }
                }
            }
            return programFriendlyName;
        }

        public static string getUWPappNameFromProgID(string progId) {
            string fullName = "";
            string str = @"SOFTWARE\Classes\" + progId + @"\Application";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(str);
            if (key != null) {
                //get UWP app name based on info
                if (key.GetValueNames().Contains("AppUserModelID")) {
                    string appFamilyname = (string)key.GetValue("AppUserModelID");
                    string[] parts = appFamilyname.Split('!');
                    appFamilyname = parts[0];
                    fullName = Helper.getPackageNameFromFamilyName(appFamilyname);
                }
            }
            return fullName;
        }
    }
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
    public static class CaseInsensitiveContains {
        // Methods
        public static bool Contains(this IEnumerable<string> List, List<string> PotentialMatches, bool IgnoreCase) =>
            List.Any<string>(item => PotentialMatches.Any<string>(candidate => string.Compare(item, candidate, !IgnoreCase) == 0));

        public static bool Contains(this IEnumerable<string> List, string PotentialMatch, bool IgnoreCase) {
            bool flag;
            if (IgnoreCase) {
                return List.Contains<string>(PotentialMatch);
            }
            using (IEnumerator<string> enumerator = List.GetEnumerator()) {
                while (true) {
                    if (enumerator.MoveNext()) {
                        if (string.Compare(enumerator.Current, PotentialMatch, StringComparison.OrdinalIgnoreCase) != 0) {
                            continue;
                        }
                        flag = true;
                    } else {
                        return false;
                    }
                    break;
                }
            }
            return flag;
        }
    }

    public class ProgID {
        // Fields
        [XmlIgnore]
        public RegistryKey LocationKey;
        public RegistryLocation Location;
        public bool isSPAD;
        public bool NeverShowExt;
        public FileTypeDescription Name;
        public IconInfo Icon;

        // Methods
        public ProgID() {
            this.isSPAD = false;
            this.Location = RegistryLocation.None;
        }

        public ProgID(RegistryKey LocationKey, string Name, bool isSPAD) {
            this.LocationKey = LocationKey;
            this.KeyName = Name;
            this.isSPAD = isSPAD;
        }

        public override string ToString() =>
            this.KeyName;

        // Properties
        public string KeyName { get; set; }
    }


    public class FileAssociationManager {

        public static Dictionary<RegistryLocation, VerbList> GetRawVerbList(ExtensionInfo info) {
            Dictionary<RegistryLocation, VerbList> dictionary1 = new Dictionary<RegistryLocation, VerbList>();
            Dictionary<RegistryLocation, VerbList> dictionary2 = new Dictionary<RegistryLocation, VerbList>();
            dictionary2.Add(RegistryLocation.UserProgID, (info.ProgID != null) ? GetVerbs(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\" + info.ProgID.KeyName)) : new VerbList());
            Dictionary<RegistryLocation, VerbList> local2 = dictionary2;
            Dictionary<RegistryLocation, VerbList> local3 = dictionary2;
            local3.Add(RegistryLocation.SystemProgID, (info.ProgID != null) ? GetVerbs(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\" + info.ProgID.KeyName)) : new VerbList());
            Dictionary<RegistryLocation, VerbList> local1 = local3;
            local1.Add(RegistryLocation.UserExtension, GetVerbs(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.Extension)));
            local1.Add(RegistryLocation.SystemExtension, GetVerbs(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.Extension)));
            local1.Add(RegistryLocation.UserPerceivedType, GetVerbs(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.PerceivedType)));
            local1.Add(RegistryLocation.SystemPerceivedType, GetVerbs(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.PerceivedType)));
            local1.Add(RegistryLocation.UserBaseClass, GetVerbs(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\*")));
            local1.Add(RegistryLocation.SystemBaseClass, GetVerbs(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\*")));
            local1.Add(RegistryLocation.UserAllFSObjects, GetVerbs(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\AllFileSystemObjects")));
            local1.Add(RegistryLocation.SystemAllFSObjects, GetVerbs(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\AllFileSystemObjects")));
            return local1;
        }
        public static VerbList GetShellExtensions(RegistryKey Location) {
            VerbList list = new VerbList();
            RegistryKey key = Location.TryOpenSubKey(@"shellex\ContextMenuHandlers");
            if (key != null) {
                foreach (string str in key.GetSubKeyNames()) {
                    string str2 = (string)key.OpenSubKey(str).GetValue(string.Empty);
                    Verb item = new Verb {
                        LocationKey = key.OpenSubKey(str)
                    };
                    string str3 = item.LocationKey.GetValue(string.Empty, null) as string;
                    if (!string.IsNullOrEmpty(str3) && !str3.StartsWith("{")) {
                        item.DefaultValue = str3;
                    } else if (!str.StartsWith("{")) {
                        item.DefaultValue = str;
                    } else {
                        string fileDescription = Registry.GetValue(@"HKEY_CLASSES_ROOT\CLSID\" + str, string.Empty, null) as string;
                        if (string.IsNullOrEmpty(fileDescription)) {
                            fileDescription = Registry.GetValue(@"HKEY_CLASSES_ROOT\Wow6432Node\CLSID\" + str, string.Empty, null) as string;
                        }
                        if (string.IsNullOrEmpty(fileDescription)) {
                            fileDescription = Registry.GetValue(@"HKEY_CLASSES_ROOT\CLSID\" + str + @"\InProcServer32", string.Empty, null) as string;
                            if (!string.IsNullOrEmpty(fileDescription)) {
                                fileDescription = FileVersionInfo.GetVersionInfo(fileDescription).FileDescription;
                            }
                        }
                        if (string.IsNullOrEmpty(fileDescription)) {
                            fileDescription = Registry.GetValue(@"HKEY_CLASSES_ROOT\Wow6432Node\CLSID\" + str + @"\InProcServer32", string.Empty, null) as string;
                            if (!string.IsNullOrEmpty(fileDescription)) {
                                fileDescription = FileVersionInfo.GetVersionInfo(fileDescription).FileDescription;
                            }
                        }
                        item.DefaultValue = string.IsNullOrEmpty(fileDescription) ? str : fileDescription;
                    }
                    item.KeyName = str;
                    item.Command = str2;
                    item.Location = KeyToLocation(item.LocationKey);
                    item.isShellExtension = true;
                    list.Verbs.Add(item);
                }
            }
            return list;
        }


        internal static VerbList GetContextMenuShellExtensions(ExtensionInfo info) {
            VerbList list = (info.ProgID == null) ? new VerbList() : HorizontalMerge(GetShellExtensions(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\" + info.ProgID.KeyName)), GetShellExtensions(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\" + info.ProgID.KeyName)));
            VerbList list2 = HorizontalMerge(GetShellExtensions(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.Extension)), GetShellExtensions(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.Extension)));
            VerbList list3 = HorizontalMerge(GetShellExtensions(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.PerceivedType)), GetShellExtensions(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.PerceivedType)));
            VerbList list4 = HorizontalMerge(GetShellExtensions(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\*")), GetShellExtensions(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\*")));
            VerbList list5 = HorizontalMerge(GetShellExtensions(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\AllFilesystemObjects")), GetShellExtensions(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\AllFilesystemObjects")));
            VerbList list6 = new VerbList();
            list6.Verbs.AddRange(list.Verbs);
            list6.Verbs.AddRange(list2.Verbs);
            list6.Verbs.AddRange(list3.Verbs);
            list6.Verbs.AddRange(list4.Verbs);
            list6.Verbs.AddRange(list5.Verbs);
            list6.Verbs = list6.Verbs.Except<Verb>(VerticalMerge(list6.Verbs)).ToList<Verb>();
            return list6;
        }

        private List<RegistryKey> GetVerbLocations(ExtensionInfo info) {
            List<RegistryKey> list = new List<RegistryKey>();
            if (info.ProgID != null) {
                list.Add(Registry.CurrentUser.OpenSubKey(@"Software\Classes\" + info.ProgID.KeyName));
                list.Add(Registry.LocalMachine.OpenSubKey(@"Software\Classes\" + info.ProgID.KeyName));
                list.Add(Registry.CurrentUser.OpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.Extension));
                list.Add(Registry.LocalMachine.OpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.Extension));
                list.Add(Registry.CurrentUser.OpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.PerceivedType));
                list.Add(Registry.LocalMachine.OpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.PerceivedType));
                list.Add(Registry.CurrentUser.OpenSubKey(@"Software\Classes\*"));
                list.Add(Registry.LocalMachine.OpenSubKey(@"Software\Classes\*"));
                list.Add(Registry.CurrentUser.OpenSubKey(@"Software\Classes\AllFilesystemObjects"));
                list.Add(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\AllFilesystemObjects"));
            }
            return list;
        }









        internal string GetIconView(ExtensionInfo info) {
            string str = "null";
            //    List<RegistryKey>.Enumerator enumerator = this.GetVerbLocations(info).GetEnumerator();
            //    if (enumerator == ) { 
            //    while (enumerator.MoveNext()) {
            //        RegistryKey key = enumerator.Current.OpenSubKey("DefaultIcon");
            //        if (key != null) {
            //            str = (string)key.GetValue(string.Empty);
            //            if (str != null) {
            //                break;
            //            }
            //        }
            //    }
            //}
            return str;
        }

        private static ProgID GetProgID(string extension) {
            RegistryKey key;
            ProgID gid = new ProgID {
                isSPAD = true
            };
            string str = Registry.CurrentUser.TryOpenSubKey((@"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\" + extension + @"\UserChoice")).TryGetValue("Progid");
            if (str != null) {
                gid.KeyName = str;
                key = Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\" + str);
                if (key != null) {
                    if (key.GetValueNames().Contains("NeverShowExt", false)) {
                        gid.NeverShowExt = true;
                    }
                    if (!str.StartsWith(@"CLSID\") || (key.TryGetValue(string.Empty) != null)) {
                        gid.LocationKey = key;
                        return gid;
                    }
                } else {
                    key = Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\" + str);
                    if (key != null) {
                        if (key.GetValueNames().Contains("NeverShowExt", false)) {
                            gid.NeverShowExt = true;
                        }
                        if (!str.StartsWith(@"CLSID\") || (key.TryGetValue(string.Empty) != null)) {
                            gid.LocationKey = key;
                            return gid;
                        }
                    }
                }
            }
            gid.isSPAD = false;
            str = Registry.CurrentUser.TryOpenSubKey((@"Software\Classes\" + extension)).TryGetValue(string.Empty);
            if (str != null) {
                gid.KeyName = str;
                key = Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\" + str);
                if (key == null) {
                    key = Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\" + str);
                    if (key != null) {
                        if (key.GetValueNames().Contains("NeverShowExt", false)) {
                            gid.NeverShowExt = true;
                        }
                        if (!str.StartsWith(@"CLSID\") || (key.TryGetValue(string.Empty) != null)) {
                            gid.LocationKey = key;
                            return gid;
                        }
                    }
                } else {
                    if (!str.StartsWith(@"CLSID\") || (key.TryGetValue(string.Empty) != null)) {
                        gid.LocationKey = key;
                        return gid;
                    }
                }
            }
            gid.isSPAD = false;
            str = Registry.LocalMachine.TryOpenSubKey((@"Software\Classes\" + extension)).TryGetValue(string.Empty);
            if (str != null) {
                gid.KeyName = str;
                key = Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\" + str);
                if (key != null) {
                    if (key.GetValueNames().Contains("NeverShowExt", false)) {
                        gid.NeverShowExt = true;
                    }
                    if (!str.StartsWith(@"CLSID\") || (key.TryGetValue(string.Empty) != null)) {
                        gid.LocationKey = key;
                        return gid;
                    }
                } else {
                    key = Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\" + str);
                    if (key != null) {
                        if (key.GetValueNames().Contains("NeverShowExt", false)) {
                            gid.NeverShowExt = true;
                        }
                        if (!str.StartsWith(@"CLSID\") || (key.TryGetValue(string.Empty) != null)) {
                            gid.LocationKey = key;
                            return gid;
                        }
                    }
                }
            }
            if ((gid != null) && string.IsNullOrEmpty(gid.KeyName)) {
                gid = null;
            }
            return null;
        }





        public static FileTypeDescription GetTypeDescriptionView(ExtensionInfo Extension) {
            string str;
            ProgID progID = Extension.ProgID;
            if ((progID == null) || (progID.LocationKey == null)) {
                FileTypeDescription description = new FileTypeDescription();
                if (Extension.Extension == null) {
                    description.Data = "Multiple Files";
                } else {
                    char[] chArray1 = new char[] { '.' };
                    description.Data = Extension.Extension.Trim(chArray1).ToUpper() + " File";
                }
                return description;
            }
            List<FileTypeDescription> source = new List<FileTypeDescription>();
            RegistryKey locationKey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\").TryOpenSubKey(progID.KeyName);
            if ((locationKey != null) && locationKey.GetValueNames().Contains("FriendlyTypeName", false)) {
                string data = locationKey.GetValue("FriendlyTypeName") as string;
                if (data != null) {
                    source.Add(new FileTypeDescription(locationKey, "FriendlyTypeName", data));
                }
            }
            RegistryKey key2 = Registry.LocalMachine.OpenSubKey(@"software\classes").TryOpenSubKey(progID.KeyName);
            if ((key2 != null) && key2.GetValueNames().Contains("FriendlyTypeName", false)) {
                string data = key2.GetValue("FriendlyTypeName") as string;
                if (data != null) {
                    source.Add(new FileTypeDescription(key2, "FriendlyTypeName", data));
                }
            }
            if (locationKey != null) {
                str = locationKey.GetValue(string.Empty) as string;
                if (str == string.Empty) {
                    char[] chArray2 = new char[] { '.' };
                    str = Extension.Extension.Trim(chArray2).ToUpper() + " File";
                }
                source.Add(new FileTypeDescription(locationKey, string.Empty, str));
            }
            if (key2 != null) {
                str = key2.GetValue(string.Empty) as string;
                if (str == string.Empty) {
                    char[] chArray3 = new char[] { '.' };
                    str = Extension.Extension.Trim(chArray3).ToUpper() + " File";
                }
                source.Add(new FileTypeDescription(key2, string.Empty, str));
            }
            for (int i = 0; i < (source.Count - 1); i++) {
                source[i].LesserPrecedenceDescription = source[i + 1];
            }
            if (source.Count<FileTypeDescription>() > 0) {
                return source[0];
            }
            FileTypeDescription description2 = new FileTypeDescription();
            char[] trimChars = new char[] { '.' };
            description2.Data = Extension.Extension.Trim(trimChars).ToUpper() + " File";
            return description2;
        }



        public static VerbList HorizontalMerge(VerbList LessImportant, VerbList MoreImportant) {
            VerbList list = new VerbList();
            List<Verb> second = new List<Verb>();
            foreach (Verb verb2 in MoreImportant.Verbs) {
                foreach (Verb verb3 in LessImportant.Verbs) {
                    if (string.Equals(verb2.KeyName, verb3.KeyName, StringComparison.OrdinalIgnoreCase)) {
                        verb2.LesserPrecedenceHorizontal = verb3;
                        second.Add(verb3);
                    }
                }
            }
            list.Verbs.AddRange(MoreImportant.Verbs);
            list.Verbs.AddRange(LessImportant.Verbs.Except<Verb>(second));


            if (!string.IsNullOrEmpty(MoreImportant.DefaultVerbKeyName) || !string.IsNullOrEmpty(LessImportant.DefaultVerbKeyName)) {
                list.DefaultVerbKeyName = (MoreImportant.DefaultVerbKeyName == null) ? LessImportant.DefaultVerbKeyName : MoreImportant.DefaultVerbKeyName;
                if ((list.DefaultVerbKeyName == "printto") && ((MoreImportant.HomogenousVerbsLocation == RegistryLocation.UserProgID) && list.VerbExists("open"))) {
                    list.DefaultVerbKeyName = "open";
                } else if (!list.VerbExists(list.DefaultVerbKeyName)) {
                    list.DefaultVerbKeyName = null;
                }
            } else if ((MoreImportant.HomogenousVerbsLocation == RegistryLocation.UserProgID) && list.VerbExists("open")) {
                list.DefaultVerbKeyName = "open";
            } else if ((LessImportant.HomogenousVerbsLocation == RegistryLocation.SystemProgID) && list.VerbExists("open")) {
                list.DefaultVerbKeyName = "open";
            } else {
                object keyName;
                if (list.Verbs.Count <= 0) {
                    keyName = null;
                }
            }
            Verb defaultVerb = list.DefaultVerb;
            if (list.Verbs.IndexOf(defaultVerb) > 0) {
                list.Verbs.Remove(defaultVerb);
                list.Verbs.Insert(0, defaultVerb);
            }
            list.HomogenousVerbsLocation = RegistryLocation.None;
            return list;
        }





        public static string GetPerceivedType(string extension) {
            try {
                return (string.IsNullOrEmpty(extension) ? null : (Registry.GetValue(@"HKEY_CLASSES_ROOT\" + extension, "PerceivedType", null) as string));
            } catch (SecurityException) {
                Trace.WriteLine("*GetPerceivedType: System.Security.SecurityException for " + extension);
                return null;
            }
        }
        public static RegistryLocation KeyToLocation(RegistryKey Key) =>
    (Key != null) ? (!Key.Name.StartsWith(@"HKEY_LOCAL_MACHINE\Software\Classes\", StringComparison.InvariantCultureIgnoreCase) ? (!Key.Name.StartsWith(@"HKEY_CURRENT_USER\Software\Classes\") ? RegistryLocation.None : (!Key.Name.StartsWith(@"HKEY_CURRENT_USER\Software\Classes\SystemFileAssociations", StringComparison.InvariantCultureIgnoreCase) ? (!Key.Name.StartsWith(@"HKEY_CURRENT_USER\Software\Classes\*", StringComparison.InvariantCultureIgnoreCase) ? (!Key.Name.StartsWith(@"HKEY_CURRENT_USER\Software\Classes\AllFileSystemObjects", StringComparison.InvariantCultureIgnoreCase) ? RegistryLocation.UserProgID : RegistryLocation.UserAllFSObjects) : RegistryLocation.UserBaseClass) : (Key.Name.StartsWith(@"HKEY_CURRENT_USER\Software\Classes\SystemFileAssociations\.", StringComparison.InvariantCultureIgnoreCase) ? RegistryLocation.UserExtension : RegistryLocation.UserPerceivedType))) : (!Key.Name.StartsWith(@"HKEY_LOCAL_MACHINE\Software\Classes\SystemFileAssociations", StringComparison.InvariantCultureIgnoreCase) ? (!Key.Name.StartsWith(@"HKEY_LOCAL_MACHINE\Software\Classes\*", StringComparison.InvariantCultureIgnoreCase) ? (!Key.Name.StartsWith(@"HKEY_LOCAL_MACHINE\Software\Classes\AllFileSystemObjects", StringComparison.InvariantCultureIgnoreCase) ? RegistryLocation.SystemProgID : RegistryLocation.SystemAllFSObjects) : RegistryLocation.SystemBaseClass) : (Key.Name.StartsWith(@"HKEY_LOCAL_MACHINE\Software\Classes\SystemFileAssociations\.", StringComparison.InvariantCultureIgnoreCase) ? RegistryLocation.SystemExtension : RegistryLocation.SystemPerceivedType))) : RegistryLocation.None;



        public static VerbList GetVerbs(RegistryKey location) {
            VerbList list = new VerbList();
            if (location != null) {
                RegistryKey key = location.TryOpenSubKey("shell");
                if (key == null) {
                    return list;
                }
                string str = key.GetValue(string.Empty) as string;
                int length = -1;
                if (str != null) {
                    length = str.IndexOf(' ');
                }
                list.DefaultVerbKeyName = (length == -1) ? str : str.Substring(0, length);
                list.HomogenousVerbsLocation = KeyToLocation(location);
                foreach (string str2 in key.GetSubKeyNames()) {
                    RegistryKey key2 = key.OpenSubKey(str2);
                    if (!key2.GetValueNames().Contains("ProgrammaticAccessOnly", false)) {
                        string mUIVerb = key2.GetValue("MuiVerb") as string;
                        string str4 = key2.GetValue(string.Empty) as string;
                        if ((str4 != null) && str4.StartsWith("@")) {
                            string resourceString = GetResourceString(str4);
                            if ((resourceString != null) && (resourceString != str4)) {
                                str4 = resourceString;
                            } else {
                                str4 = str2;
                                Trace.WriteLine("GetVerbs: Could not resolve resource: " + resourceString);
                            }
                        }
                        string command = key2.TryOpenSubKey("Command").TryGetValue(string.Empty);
                        DropTargetInfo dropTarget = null;
                        RegistryKey key3 = key2.TryOpenSubKey("DropTarget");
                        string cLSID = key3.TryGetValue("CLSID");
                        if (cLSID != null) {
                            dropTarget = new DropTargetInfo(key3, cLSID);
                        }
                        DDEInfo dDE = null;
                        RegistryKey key4 = key2.TryOpenSubKey("ddeexec");
                        if (key4 != null) {
                            dDE = new DDEInfo(key4.TryGetValue(null), key4.TryOpenSubKey("Application").TryGetValue(null), key4.TryOpenSubKey("Topic").TryGetValue(null));
                        }
                        DelegateExecuteInfo delegateExecute = null;
                        RegistryKey key5 = key2.TryOpenSubKey("command");
                        string str7 = key5.TryGetValue("DelegateExecute");
                        if (str7 != null) {
                            delegateExecute = new DelegateExecuteInfo(key5, str7);
                        }
                        bool flag = !string.Equals(str2, "printto", StringComparison.OrdinalIgnoreCase);
                        bool flag2 = key2.GetValueNames().Contains("NeverDefault", false);
                        bool flag3 = key2.GetValueNames().Contains("LegacyDisable", false);
                        Verb item = new Verb(str2, str4, mUIVerb, command, key2.TryGetValue("Icon"), dropTarget, dDE, delegateExecute, key.OpenSubKey(str2), KeyToLocation(key.OpenSubKey(str2)));
                        item.isVisible = flag;
                        item.isNeverDefault = flag2;
                        item.isEnabled = flag3;
                        list.Verbs.Add(item);
                    }
                }
            }
            return list;
        }





        internal static VerbList GetContextMenu(ExtensionInfo info) {
            VerbList list2;
            List<RegistryLocation> list7;
            VerbList list = new VerbList();
            if (info.ProgID != null) {
                list2 = HorizontalMerge(GetVerbs(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\" + info.ProgID.KeyName)), GetVerbs(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\" + info.ProgID.KeyName)));
            } else {
                list2 = new VerbList();
                Trace.WriteLine("*GetContextMenu: Null ProgID for" + info.Extension);
            }
            VerbList list3 = HorizontalMerge(GetVerbs(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.Extension)), GetVerbs(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.Extension)));
            VerbList list4 = HorizontalMerge(GetVerbs(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.PerceivedType)), GetVerbs(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\SystemFileAssociations\" + info.PerceivedType)));
            VerbList list5 = HorizontalMerge(GetVerbs(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\*")), GetVerbs(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\*")));
            VerbList list6 = HorizontalMerge(GetVerbs(Registry.LocalMachine.TryOpenSubKey(@"Software\Classes\AllFilesystemObjects")), GetVerbs(Registry.CurrentUser.TryOpenSubKey(@"Software\Classes\AllFilesystemObjects")));
            if (list2.DefaultVerb != null) {
                list.DefaultVerbKeyName = list2.DefaultVerbKeyName;
                List<RegistryLocation> list1 = new List<RegistryLocation>();
                list1.Add(RegistryLocation.UserProgID);
                list1.Add(RegistryLocation.UserExtension);
                list1.Add(RegistryLocation.UserPerceivedType);
                list1.Add(RegistryLocation.UserBaseClass);
                list1.Add(RegistryLocation.UserAllFSObjects);
                list7 = list1;
            } else if (list3.DefaultVerb != null) {
                list.DefaultVerbKeyName = list3.DefaultVerbKeyName;
                List<RegistryLocation> list8 = new List<RegistryLocation>();
                list8.Add(RegistryLocation.UserExtension);
                list8.Add(RegistryLocation.UserPerceivedType);
                list8.Add(RegistryLocation.UserBaseClass);
                list8.Add(RegistryLocation.UserAllFSObjects);
                list8.Add(RegistryLocation.UserProgID);
                list7 = list8;
            } else if (list4.DefaultVerb != null) {
                list.DefaultVerbKeyName = list4.DefaultVerbKeyName;
                List<RegistryLocation> list9 = new List<RegistryLocation>();
                list9.Add(RegistryLocation.UserPerceivedType);
                list9.Add(RegistryLocation.UserBaseClass);
                list9.Add(RegistryLocation.UserAllFSObjects);
                list9.Add(RegistryLocation.UserExtension);
                list9.Add(RegistryLocation.UserProgID);
                list7 = list9;
            } else if (list5.DefaultVerb != null) {
                list.DefaultVerbKeyName = list5.DefaultVerbKeyName;
                List<RegistryLocation> list10 = new List<RegistryLocation>();
                list10.Add(RegistryLocation.UserBaseClass);
                list10.Add(RegistryLocation.UserAllFSObjects);
                list10.Add(RegistryLocation.UserPerceivedType);
                list10.Add(RegistryLocation.UserExtension);
                list10.Add(RegistryLocation.UserProgID);
                list7 = list10;
            } else if (list6.DefaultVerb == null) {
                List<RegistryLocation> list12 = new List<RegistryLocation>();
                list12.Add(RegistryLocation.UserAllFSObjects);
                list12.Add(RegistryLocation.UserBaseClass);
                list12.Add(RegistryLocation.UserPerceivedType);
                list12.Add(RegistryLocation.UserExtension);
                list12.Add(RegistryLocation.UserProgID);
                list7 = list12;
            } else {
                list.DefaultVerbKeyName = list6.DefaultVerbKeyName;
                List<RegistryLocation> list11 = new List<RegistryLocation>();
                list11.Add(RegistryLocation.UserAllFSObjects);
                list11.Add(RegistryLocation.UserBaseClass);
                list11.Add(RegistryLocation.UserPerceivedType);
                list11.Add(RegistryLocation.UserExtension);
                list11.Add(RegistryLocation.UserProgID);
                list7 = list11;
            }
            foreach (RegistryLocation location in list7) {
                switch (location) {
                    case RegistryLocation.UserExtension: {
                            list.Verbs.AddRange(list3.Verbs);
                            continue;
                        }
                    case RegistryLocation.UserProgID: {
                            list.Verbs.AddRange(list2.Verbs);
                            continue;
                        }
                    case RegistryLocation.UserPerceivedType: {
                            list.Verbs.AddRange(list4.Verbs);
                            continue;
                        }
                    case RegistryLocation.UserBaseClass: {
                            list.Verbs.AddRange(list5.Verbs);
                            continue;
                        }
                    case RegistryLocation.UserAllFSObjects: {
                            list.Verbs.AddRange(list6.Verbs);
                            continue;
                        }
                }
            }
            list.Verbs = list.Verbs.Except<Verb>(VerticalMerge(list.Verbs)).ToList<Verb>();
            return list;
        }


        public static List<Verb> VerticalMerge(List<Verb> FullList) {
            foreach (Verb verb in FullList) {
                verb.LesserPrecedenceVertical = null;
                if (verb.LesserPrecedenceHorizontal != null) {
                    verb.LesserPrecedenceHorizontal.LesserPrecedenceVertical = null;
                }
            }
            List<Verb> list = new List<Verb>();
            int num = 0;
            while (num < FullList.Count) {
                int num2 = num + 1;
                while (true) {
                    if (num2 < FullList.Count) {
                        if (!string.Equals(FullList[num].KeyName, FullList[num2].KeyName, StringComparison.OrdinalIgnoreCase)) {
                            num2++;
                            continue;
                        }
                        if (FullList[num].LesserPrecedenceVerb == null) {
                            FullList[num].LesserPrecedenceVertical = FullList[num2];
                        } else if ((FullList[num].LesserPrecedenceVerb.LesserPrecedenceVerb == null) && (FullList[num].LesserPrecedenceVerb.Location != FullList[num2].Location)) {
                            FullList[num].LesserPrecedenceVerb.LesserPrecedenceVertical = FullList[num2];
                        }
                        list.Add(FullList[num2]);
                    }
                    num++;
                    break;
                }
            }
            return list;
        }





        public ExtensionInfo GetExtensionInfo(string Extension) {
            ExtensionInfo extension = new ExtensionInfo {
                Extension = Extension,
                ProgID = GetProgID(Extension)
            };
            extension.DescriptionInfo = GetTypeDescriptionView(extension);
            extension.IconResourcePath = this.GetIconView(extension);
            extension.ContextMenuVerbs = GetContextMenu(extension);
            return extension;
        }
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


