using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Newtonsoft.Json;
using SendgridMock.Features.Mail;

namespace SendgridMock
{
    public class MailModule : NancyModule
    {
        public MailModule()
        {

            var baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            var appDataFolder = Path.GetFullPath(Path.Combine(baseFolder, "App_Data"));

            Get["/"] = _ =>
            {
                var dateFolders = Directory.EnumerateDirectories(appDataFolder);

                List<string> datesList = dateFolders.Select(dateFolder => new DirectoryInfo(dateFolder).Name).ToList();


                return Negotiate.WithView("Index").WithModel(datesList);
            };

            Get["/{date}"] = _ =>
            {
                string dateFolderName = _.date;
                var dateFolder = Path.GetFullPath(Path.Combine(appDataFolder, dateFolderName));
                var files = Directory.EnumerateFiles(dateFolder);

                var mailModels =
                    files.Select(x => new { FileName = x, Content = File.ReadAllText(x) }).Select(x => new
                    {
                        x.FileName,
                        x.Content,
                        Model = JsonConvert.DeserializeObject<Mail>(x.Content)
                    });

                List<Mail> mails = mailModels.Select(x => x.Model).OrderByDescending(x => x.ReceivedAt).ToList();
                var model = new List.Model();
                model.Date = dateFolderName;
                model.Mails = mails;

                return Negotiate.WithView("List").WithModel(model);
            };

            Get["/{date}/{id}"] = _ =>
            {
                string dateFolderName = _.date;
                string id = _.id;
                var fileName = Path.GetFullPath(Path.Combine(appDataFolder, dateFolderName, id)) + ".json";
                var content = File.ReadAllText(fileName);
                var mail = JsonConvert.DeserializeObject<Mail>(content);
                var model = new Details.Model();
                model.Body = mail.Content.FirstOrDefault().Value;
                return Negotiate.WithView("Details").WithModel(model);
            };
        }
    }
}
