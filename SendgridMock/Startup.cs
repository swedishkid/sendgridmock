using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nancy.Bootstrapper;
using Newtonsoft.Json;
using Owin;

namespace SendgridMock
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseHandlerAsync(async (request, response, next) =>
            {
                var baseFolder = AppDomain.CurrentDomain.BaseDirectory;
                var appDataFolder = Path.GetFullPath(Path.Combine(baseFolder, "App_Data"));

                using (StreamReader reader = new StreamReader(request.Body))
                {
                    if (request.Method == "POST")
                    {
                        var body = await reader.ReadToEndAsync();
                        Mail mail = JsonConvert.DeserializeObject<Mail>(body);
                        mail.Id = Guid.NewGuid().ToString();
                        mail.ReceivedAt = DateTime.Now;
                        var dateFolderName = DateTime.Now.ToString("dd-MM-yyyy");
                        var dateFolder = Path.GetFullPath(Path.Combine(appDataFolder, dateFolderName));
                        var dateFolderInfo = new DirectoryInfo(dateFolder);

                        if (!dateFolderInfo.Exists)
                        {
                            dateFolderInfo.Create();
                        }

                        var fileName = mail.Id + ".json";
                        var filePath = Path.GetFullPath(Path.Combine(dateFolder, fileName));
                        File.WriteAllText(filePath, JsonConvert.SerializeObject(mail));
                    }
                    else
                    {
                        await next();
                    }
                }
            });

            app.UseNancy(options =>
            {
                options.Bootstrapper = new Bootstrapper();
            });

        }
    }
}