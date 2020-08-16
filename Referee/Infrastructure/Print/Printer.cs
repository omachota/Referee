using System;
using System.Collections.Generic;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Properties;
using Referee.Infrastructure.SettingsFd;
using Referee.Models;
using Document = iText.Layout.Document;
using Paragraph = iText.Layout.Element.Paragraph;
using Path = System.IO.Path;

namespace Referee.Infrastructure.Print
{
	public class Printer
	{
		private readonly Settings _settings;

		public Printer(Settings settings)
		{
			_settings = settings;
		}

		public void RawPrint(int pagesCount)
		{
		}

		public void Print(List<Rozhodci> selectedRozhodci, int? sum)
		{
			Print(false, selectedRozhodci);
		}

		private void Print(bool isRaw, List<Rozhodci> selectedRozhodci, int rawpagesCount = 0)
		{
			var filePath = Path.Combine(Constants.WorkingDirectory, "vyplatni-listina-rozhodci.pdf");

			using (var writer = new PdfWriter(filePath))
			{
				using (var pdf = new PdfDocument(writer))
				{
					var doc = new Document(pdf, PageSize.A4.Rotate());
					doc.SetMargins(23f, 38f, 20f, 38f);
					int pagesCount = (int) Math.Ceiling((double) selectedRozhodci.Count / 10);
					if (pagesCount == 0)
						pagesCount = 1;
					if (isRaw)
						pagesCount = rawpagesCount;

					PdfFont font = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA, PdfEncodings.CP1250, false);
					PdfFont bold = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD, PdfEncodings.CP1250, false);

					// TODO : add margin
					Paragraph documentMainHeader = new Paragraph($"TJ, Sportovní klub, Atletický oddíl, Atletický klub: {_settings.ClubName}")
					                               .SetFont(bold).SetFontSize(16).SetTextAlignment(TextAlignment.CENTER);
					Paragraph vyplatniListinaHead = new Paragraph("VÝPLATNÍ LISTINA ODM\u011AN ROZHOD\u010CÍCH")
					                                .SetFont(bold).SetFontSize(16).SetTextAlignment(TextAlignment.CENTER).SetMarginTop(-2f);
					Paragraph rules = new Paragraph(
						                  "Níže podepsaní účastníci soutěže souhlasili s uvedením svých osobních údajů na této výplatní listině (jméno, příjmení, datum narození a adresa).")
					                  .SetFont(font).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER).SetMarginTop(-1f).SetMarginBottom(1f);
					Paragraph evidenceText =
						new Paragraph(
								$"Tyto údaje budou součástí evidence {new string('.', 123)} a budou jen pro vnitřní potřebu.")
							.SetFont(font).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER).SetMarginTop(0f);

					for (int i = 0; i < pagesCount; i++)
					{
						#region Pdf text

						doc.Add(documentMainHeader);
						doc.Add(vyplatniListinaHead);
						doc.Add(rules);
						doc.Add(evidenceText);
						doc.Close();

						#endregion
					}
				}
			}

			string url = "/select, \"" + filePath;
			ChromeLauncher.OpenLink(url);
		}
	}
}
