using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianificazioneFrm.Helpers
{
    public class ExcelHelper
    {
        public byte[] CreaExcelPianificazione(DataSet ds, string nomeTabella)
        {
            byte[] content;
            MemoryStream ms = new MemoryStream();
            //string filename = @"c:\temp\mancanti.xlsx";
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Adding style
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                Columns columns = new Columns();
                for(int i=0;i< ds.Tables[nomeTabella].Columns.Count;i++)
                {
                    Column c = new Column();
                    UInt32Value u = new UInt32Value((uint)(i + 1));
                    c.Min = u;
                    c.Max = u;
                    c.Width = 15;
                    c.CustomWidth = true;

                    columns.Append(c);
                }

                worksheetPart.Worksheet.AppendChild(columns);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Pianificazione" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();
                for (int i = 0; i < ds.Tables[nomeTabella].Columns.Count; i++)
                {
                    string etichetta = ds.Tables[nomeTabella].Columns[i].ColumnName;
                    row.Append(ConstructCell(etichetta, CellValues.String, 2));
                }

                // Insert the header row to the Sheet Data
                sheetData.AppendChild(row);

                foreach (DataRow riga in ds.Tables[nomeTabella].Rows)
                {
                    row = new Row();

                    for (int i = 0; i < ds.Tables[nomeTabella].Columns.Count; i++)
                    {
                        string etichetta = riga[i].ToString();
                        row.Append(ConstructCell(etichetta, CellValues.String, 1));
                    }

                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
                document.Save();
                document.Close();

                ms.Seek(0, SeekOrigin.Begin);
                content = ms.ToArray();
            }

            return content;
        }

        private Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }

        private Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 10 }

                ),
                new Font( // Index 1 - header
                    new FontSize() { Val = 10 },
                    new Bold(),
                    new Color() { Rgb = "FFFFFF" }

                ));

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } })
                    { PatternType = PatternValues.Solid }) // Index 2 - header
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true } // header
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }

    }
}
