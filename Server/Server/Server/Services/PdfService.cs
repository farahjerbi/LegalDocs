using Server.DTOs;
using System.Text;

namespace Server.Services
{
    public class PdfService: IPdfService
    {

        public string RenderStandardTemplate(TemplateAddDto templateDto)
        {
            var htmlContent = new StringBuilder();

                                 htmlContent.AppendLine(@"
                        <style>
                            body { font-family: Arial, sans-serif; }
                            h1, h2 { text-align: center; color: #574ca0; }
                            .group { margin: 20px 0; padding: 10px; border: 1px solid #ccc; }
                            .alias { margin: 10px 0; padding: 5px; border: 1px solid #eee; }
                            table { width: 100%; border-collapse: collapse; margin-top: 10px; }
                            th, td { border: 1px solid #ccc; padding: 8px; text-align: left; }
                            tr:nth-child(even) { background-color: #f2f2f2; }
                        </style>");

                                    htmlContent.AppendLine($"<h1>{templateDto.Name}</h1>");

                                    foreach (var group in templateDto.Groups)
                                    {
                                        htmlContent.AppendLine(@$"
                        <div class='group'>
                            <h2>{group.TitleSection}</h2>
                            <table>
                                <tr>
                                    <th style='background-color: #574ca0; color: white; font-weight: bold;'>Alias's Title</th>
                                    <th style='background-color: #574ca0; color: white; font-weight: bold;'>Value</th>
                                </tr>");

                                        bool isEvenRow = false; // To alternate row colors
                                        foreach (var alias in group.Aliases)
                                        {
                                            // Toggle row color
                                            isEvenRow = !isEvenRow;
                                            string rowColor = isEvenRow ? "#f2f2f2" : "white";

                                            htmlContent.AppendLine(@$"
                            <tr style='background-color: {rowColor};'>
                                <td>{alias.Title}</td>
                                <td>{alias.TypeSetting.DefaultValue}</td>
                            </tr>");
                                        }

                                        htmlContent.AppendLine(@"
                            </table>
                        </div>");
                                    }

                                    return htmlContent.ToString();
                                }

    }
}
