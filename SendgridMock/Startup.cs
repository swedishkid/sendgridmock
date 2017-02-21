using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Owin;

namespace SendgridMock
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseHandlerAsync(async (request, response) =>
            {

                var baseFolder = AppDomain.CurrentDomain.BaseDirectory;
                var appDataFolder = Path.GetFullPath(Path.Combine(baseFolder, "App_Data"));

                using (StreamReader reader = new StreamReader(request.Body))
                {

                    if (request.Method == "GET")
                    {
                        if (request.Path == "/")
                        {
                            var dateFolders = Directory.EnumerateDirectories(appDataFolder);

                            StringBuilder builder = new StringBuilder();

                            foreach (var dateFolder in dateFolders)
                            {
                                var date = new DirectoryInfo(dateFolder).Name;
                                builder.AppendLine($"<a href='/{date}'>{date}</a> <br/>");
                            }

                            response.ContentType = "text/html";
                            response.StatusCode = 200;

                            using (StreamWriter writer = new StreamWriter(response.Body))
                            {
                                await writer.WriteAsync(builder.ToString());
                            }
                        }
                        else if (Regex.IsMatch(request.Path, @"^\/[0-9]+-[0-9]+-[0-9]+$"))
                        {
                            var dateFolderName = request.Path.Remove(0, 1);
                            var dateFolder = Path.GetFullPath(Path.Combine(appDataFolder, dateFolderName));
                            var files = Directory.EnumerateFiles(dateFolder);

                            var mailModels =
                                files.Select(x => new {FileName = x, Content = File.ReadAllText(x)}).Select(x => new
                                {
                                    x.FileName,
                                    x.Content,
                                    Model = JsonConvert.DeserializeObject<Mail>(x.Content)
                                });


                            StringBuilder builder = new StringBuilder();
                            builder.AppendLine("<table>");
                            builder.AppendLine("<tr>");
                            builder.AppendLine("<td><strong>Category</strong></td>");
                            builder.AppendLine("<td><strong>To</strong></td>");
                            builder.AppendLine("<td><strong>From</strong></td>");
                            builder.AppendLine("<td><strong>Subject</strong></td>");
                            builder.AppendLine("</tr>");

                            foreach (var mailModel in mailModels)
                            {
                                builder.AppendLine("<tr>");
                                builder.AppendLine($"<td>{string.Join(",", mailModel.Model.Categories)}</td>");
                                builder.AppendLine($"<td>-</td>");
                                builder.AppendLine($"<td>{mailModel.Model.From.Email}</td>");
                                builder.AppendLine($"<td>{mailModel.Model.Subject}</td>");
                                builder.AppendLine("</tr>");
                            }

                            builder.AppendLine("</table>");


                            response.ContentType = "text/html";
                            response.StatusCode = 200;

                            using (StreamWriter writer = new StreamWriter(response.Body))
                            {
                                await writer.WriteAsync(builder.ToString());
                            }
                        }

                        
                    }

                    if (request.Method == "POST")
                    {
                        var body = await reader.ReadToEndAsync();
                        var mail = JsonConvert.DeserializeObject<Mail>(body);

                        var dateFolderName = DateTime.Now.ToString("dd-MM-yyyy");
                        var dateFolder = Path.GetFullPath(Path.Combine(appDataFolder, dateFolderName));
                        var dateFolderInfo = new DirectoryInfo(dateFolder);
                        if (!dateFolderInfo.Exists)
                        {
                            dateFolderInfo.Create();
                        }

                        var fileName = Guid.NewGuid() + ".json";
                        var filePath = Path.GetFullPath(Path.Combine(dateFolder, fileName));
                        File.WriteAllText(filePath, body);


                    }
                }
            });
        }
    }
}