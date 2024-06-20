using CCDA_Parser.DAL;
using CCDA_Parser.Service;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using Topshelf;

namespace CCDAParser
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.Logger(lc => lc
                        .WriteTo.File("log.txt"))
                        .CreateLogger();


            //string FileName = @"D:/WombaCare/IDOC(Documents)/Sample.xml";
            //string FileName = @"D:/Project/IDoc/IDOC(Documents)/CCDA_1_63b9e6b3d0295b477e09f607.xml";
            //string FileName = @"D:/Project/IDoc/IDOC(Documents)/CCDA_1_63b159abd0295b7610de11bd.xml";
            //string FileName = @"D:/Project Doc/Project/CCDA.xml";
            string FileName = @"D:/CCMG DLL's/170.315_b1_toc_inp_r11_sample1_v8.xml";

            CCDAParserDAL parserDAL = new CCDAParserDAL();
            int PatientId=0;
            string con = "";
            string ccdaFile = "";
            string ccdaFiles = await File.ReadAllTextAsync(FileName); ;
            await parserDAL.XmlDataParhing(PatientId, FileName, ccdaFiles, con);

            var exitCode = HostFactory.Run(x =>
            {
                x.Service<CCDAParserService>(s =>
                {
                    s.ConstructUsing(CCDAParserService => new CCDAParserService());

                    s.WhenStarted(CCDAParserService => CCDAParserService.start());

                    s.WhenStopped(CCDAParserService => CCDAParserService.stop());
                });

                x.RunAsLocalService();
                x.SetServiceName("CCDAParserService");
                x.SetDisplayName("CCDA Parser Service");
                x.SetDescription("CCDA Parser Background Job Service");
            });
            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());

            Environment.ExitCode = exitCodeValue;

        }
    }
}