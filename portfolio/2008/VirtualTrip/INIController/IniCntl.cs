/*==============================================================================================================
INI Class 
---------
Author:
				Adam Woods - http://www.aejw.com/
Modified:
				3rd August 2005
Ownership:
				Copyright 2005 Adam Woods
Build:
				0019 - see http://www.codeproject.com/csharp/aejw_ini_class.asp for more information and histroy
Source:
				http://www.aejw.com/
EULA:
				You disturbe and use this code / class in any envoriment you see fit.
				The header (this information) can not be modified or removed.
				LIMIT OF LIABILITY: IN NO EVENT WILL Adam Woods BE LIABLE TO YOU FOR ANY LOSS OF USE, 
				INTERRUPTION OF BUSINESS, OR ANY DIRECT, INDIRECT, SPECIAL, INCIDENTAL, OR CONSEQUENTIAL 
				DAMAGES OF ANY KIND (INCLUDING LOST PROFITS) REGARDLESS OF THE FORM OF ACTION WHETHER IN 
				CONTRACT, TORT (INCLUDING NEGLIGENCE), STRICT PRODUCT LIABILITY OR OTHERWISE, EVEN 
				IF Adam Woods HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES. 
==============================================================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace INIController
{
    public class IniCntl
    {
		[DllImport("kernel32", SetLastError=true)] private static extern int WritePrivateProfileString(string pSection, string pKey, string pValue, string pFile);
		[DllImport("kernel32", SetLastError=true)] private static extern int WritePrivateProfileStruct(string pSection, string pKey, string pValue, int pValueLen, string pFile);
		[DllImport("kernel32", SetLastError=true)] private static extern int GetPrivateProfileString(string pSection, string pKey, string pDefault, byte[] prReturn, int pBufferLen, string pFile);
		[DllImport("kernel32", SetLastError=true)] private static extern int GetPrivateProfileStruct(string pSection, string pKey, byte[] prReturn, int pBufferLen, string pFile);

		private string ls_IniFilename;
		private int li_BufferLen = 256;		

		/// <summary>
		/// cINI Constructor
		/// </summary>
        public IniCntl(string pIniFilename) {
			ls_IniFilename = pIniFilename;
		}

		/// <summary>
		/// INI filename (If no path is specifyed the function will look with-in the windows directory for the file)
		/// </summary>
		public string IniFile{
			get{return(ls_IniFilename);}
			set{ls_IniFilename=value;}
		}

		/// <summary>
		/// Max return length when reading data (Max: 32767)
		/// </summary>
		public int BufferLen{
			get{return li_BufferLen;}
			set{
				if(value>32767){li_BufferLen=32767;}
				else if(value<1){li_BufferLen=1;}
				else{li_BufferLen=value;}
			}
		}

		/// <summary>
		/// Read value from INI File
		/// </summary>
		public string ReadValue(string pSection, string pKey, string pDefault){
			
			return(z_GetString(pSection,pKey,pDefault));

		}

		/// <summary>
		/// Read value from INI File, default = ""
		/// </summary>
		public string ReadValue(string pSection, string pKey){
			
			return(z_GetString(pSection,pKey,""));
		
		}

		/// <summary>
		/// Write value to INI File
		/// </summary>
		public void WriteValue(string pSection, string pKey, string pValue){

			WritePrivateProfileString(pSection, pKey, pValue, this.ls_IniFilename);			

		}

		/// <summary>
		/// Remove value from INI File
		/// </summary>
		public void RemoveValue(string pSection, string pKey){

			WritePrivateProfileString(pSection, pKey, null, this.ls_IniFilename);	
		
		}

		/// <summary>
		/// Read values in a section from INI File
		/// </summary>
		public void ReadValues(string pSection, ref Array pValues){

			pValues = z_GetString(pSection,null,null).Split((char) 0);

		}

		/// <summary>
		/// Read sections from INI File
		/// </summary>
		public void ReadSections(ref Array pSections){

			pSections = z_GetString(null,null,null).Split((char) 0);

		}

		/// <summary>
		/// Remove section from INI File
		/// </summary>
		public void RemoveSection(string pSection){

			WritePrivateProfileString(pSection, null, null, this.ls_IniFilename);

		}

		/// <summary>
		/// Call GetPrivateProfileString / GetPrivateProfileStruct API
		/// </summary>
		private string z_GetString(string pSection, string pKey, string pDefault){

			string sRet=pDefault;byte[] bRet=new byte[li_BufferLen];
			int i=GetPrivateProfileString(pSection, pKey, pDefault, bRet, li_BufferLen, ls_IniFilename);
			sRet=System.Text.Encoding.GetEncoding(1252).GetString(bRet,0,i).TrimEnd((char)0);
			return(sRet);

		}
    }
}
