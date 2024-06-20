using CCDA_Parser.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDA_Parser
{
    public class CCDAFileParser
    {
        public async Task ReadCCDAFile(int PatientId,string FileName, string ccdaFile, string con)
        {
            CCDAParserDAL parserDAL = new CCDAParserDAL();
            await parserDAL.XmlDataParhing(PatientId, FileName, ccdaFile, con);
        }
    }
}
