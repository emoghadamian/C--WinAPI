using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct LOCALGROUP_USERS_INFO_0
{
    [MarshalAs(UnmanagedType.LPWStr)] public string name;

}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct LOCALGROUP_MEMBERS_INFO_2
{
    public IntPtr lgrmi2_sid;
    public int lgrmi2_sidusage;
    public string lgrmi2_domainandname;
}

namespace NetLocalGroupEnum
{
    public class Program
    {
       [DllImport("Netapi32.dll")]
       public extern static int NetLocalGroupEnum([MarshalAs(UnmanagedType.LPWStr)]
       string servername,
       int level,
       ref IntPtr bufptr,
       int prefmaxlen,
       ref UInt32 entriesread,
       ref UInt32 totalentries,
       IntPtr resume_handle);

        [DllImport("NetAPI32.dll", CharSet = CharSet.Unicode)]
        public extern static int NetLocalGroupGetMembers(
            [MarshalAs(UnmanagedType.LPWStr)] string servername,
            [MarshalAs(UnmanagedType.LPWStr)] string localgroupname,
            int level,
            ref IntPtr bufptr,
            int prefmaxlen,
            ref UInt32 entriesread,
            ref UInt32 totalentries,
            IntPtr resume_handle);
        static void Main(string[] args)
        {
            IntPtr bufptr = IntPtr.Zero;
            UInt32 entriesread = 0, totalentries = 0;
            int result = NetLocalGroupEnum(null,0,ref bufptr, -1, ref entriesread,ref totalentries,IntPtr.Zero);
            Console.WriteLine("The LocalGroups Are blewo:");
            //Enum Every local groups
            Console.WriteLine("=======================================================");
            for (int i=0;i < totalentries;i++) {
                LOCALGROUP_USERS_INFO_0 lOCALGROUP_USERS = (LOCALGROUP_USERS_INFO_0)Marshal.PtrToStructure(bufptr, typeof(LOCALGROUP_USERS_INFO_0));
                Console.WriteLine("LocalGroup Name : {0}", lOCALGROUP_USERS.name);
                bufptr += Marshal.SizeOf(typeof(LOCALGROUP_USERS_INFO_0));
                IntPtr bufptr1 = IntPtr.Zero;                                                                                                                                                   
                UInt32 Gentriesread = 0, Gtotalentries = 0;
                int result1 = NetLocalGroupGetMembers(null,lOCALGROUP_USERS.name, 2,ref bufptr1,-1, ref Gentriesread,ref Gtotalentries,IntPtr.Zero);
                Console.WriteLine("Members :");
                //Enum group members
                for (int j = 0; j < Gtotalentries; j++)
                {

                    LOCALGROUP_MEMBERS_INFO_2 LOCALGROUPMembers = (LOCALGROUP_MEMBERS_INFO_2)Marshal.PtrToStructure(bufptr1, typeof(LOCALGROUP_MEMBERS_INFO_2));
                    Console.WriteLine("UserName: {0}",LOCALGROUPMembers.lgrmi2_domainandname);
                    bufptr1 += Marshal.SizeOf(typeof(LOCALGROUP_MEMBERS_INFO_2));

                    
                }
                Console.WriteLine("---------------------------------------------");


            }
            Console.ReadLine();
            
        }
    }
}
