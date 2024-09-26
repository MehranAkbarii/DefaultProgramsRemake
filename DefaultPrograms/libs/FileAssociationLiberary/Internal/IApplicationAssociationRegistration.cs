using System;
using System.Runtime.InteropServices;

namespace FileAssociationLibrary {
    [Guid("4e530b0a-e611-4c77-a3ac-9031d022281b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IApplicationAssociationRegistration
    {
        [Obsolete]
        internal void QueryCurrentDefault();

        [Obsolete]
        internal void QueryAppIsDefault();

        [Obsolete]
        internal void GetTooltipInfo();

        [PreserveSig]
        int SetAppAsDefault([MarshalAs(UnmanagedType.LPWStr)] string appId, [MarshalAs(UnmanagedType.LPWStr)] string association, AssociationType flags);

        [PreserveSig]
        int SetAppAsDefaultAll([MarshalAs(UnmanagedType.LPWStr)] string appId, bool flags);


        [PreserveSig]
        int ClearUserAssociations();
    }
}
