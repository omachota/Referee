using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Referee.Infrastructure.SettingsFd;
using Referee.Models;
using Document = iText.Layout.Document;
using Paragraph = iText.Layout.Element.Paragraph;
using Path = System.IO.Path;

namespace Referee.Infrastructure
{
	public class Printer
	{
		private readonly Settings _settings;
		private const float FontSize = 11;

		private bool _isCreatingPdf;

		public Printer(Settings settings)
		{
			_settings = settings;
		}

		public async Task RawPrint(int pagesCount)
		{
			if (!_isCreatingPdf)
			{
				_isCreatingPdf = true;
				await Task.Run(() => Print(true, new List<Rozhodci>(), pagesCount));
				await Task.Delay(100);
				_isCreatingPdf = false;
			}
		}

		public async Task Print<T>(List<T> selectedPersons) where T : IPerson
		{
			if (!_isCreatingPdf)
			{
				_isCreatingPdf = true;
				var sortedPersons = selectedPersons.OrderBy(x => x.LastName).ToList();
				await Task.Run(() => Print(false, sortedPersons));
				await Task.Delay(100);
				_isCreatingPdf = false;
			}
		}

		private void Print<T>(bool isRaw, List<T> selectedPersons, int rawpagesCount = 0) where T : IPerson
		{
			var filePath = Path.Combine(Constants.WorkingDirectory, typeof(T) == typeof(Rozhodci) ? "vyplatni-listina-rozhodci.pdf" : "vyplatni-listina-ceta.pdf");

			using (var writer = new PdfWriter(filePath))
			{
				using (var pdf = new PdfDocument(writer))
				{
					var doc = new Document(pdf, PageSize.A4.Rotate());
					doc.SetMargins(23, 38, 20, 38);
					int pagesCount = (int) Math.Ceiling((double) selectedPersons.Count / 10);
					if (pagesCount == 0)
						pagesCount = 1;
					if (isRaw)
						pagesCount = rawpagesCount;

					PdfFont font = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA, PdfEncodings.CP1250, false);
					PdfFont bold = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD, PdfEncodings.CP1250, false);

					Paragraph documentMainHeader = new Paragraph($"TJ, Sportovní klub, Atletický oddíl, Atletický klub: {_settings.ClubName}")
					                               .SetFont(bold).SetFontSize(16).SetTextAlignment(TextAlignment.CENTER);
					string listinaHeadText = "VÝPLATNÍ LISTINA ODMĚN ";
					listinaHeadText += typeof(T) == typeof(Rozhodci) ? "ROZHODČÍCH" : "TECHNICKÉ ČETY";
					Paragraph vyplatniListinaHead = new Paragraph(listinaHeadText)
					                                .SetFont(bold).SetFontSize(16).SetTextAlignment(TextAlignment.CENTER).SetMarginTop(-3);
					Paragraph rules = new Paragraph(
						                  "Níže podepsaní účastníci soutěže souhlasili s uvedením svých osobních údajů na této výplatní listině (jméno, příjmení, datum narození a adresa).")
					                  .SetFont(font).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER).SetMarginTop(-1).SetMarginBottom(1);
					Paragraph evidenceText =
						new Paragraph(
								$"Tyto údaje budou součástí evidence {new string('.', 123)} a budou jen pro vnitřní potřebu.")
							.SetFont(font).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER).SetMarginTop(-1);

					Table aboutcompetition = new Table(UnitValue.CreatePercentArray(new[] {52.15f, 47.85f})).SetMarginTop(1).UseAllAvailableWidth();
					if (_settings.IsCompetitionNameEnabled)
						aboutcompetition.AddCell(AboutCompetitionCell($"Název soutěže: {_settings.CompetitionName}", bold));
					else
						aboutcompetition.AddCell(AboutCompetitionCell($"Název soutěže {new string('.', 89)}", bold));

					if (_settings.IsCompetitionDateEnabled)
						if (_settings.CompetitionStartDate.HasValue && _settings.CompetitionEndDate == null)
							aboutcompetition.AddCell(
								AboutCompetitionCell($"Datum konání soutěže: {_settings.CompetitionStartDate.Value:dd.MM.yyyy}", bold));
						else if (_settings.CompetitionStartDate.HasValue && _settings.CompetitionEndDate.HasValue)
							aboutcompetition.AddCell(AboutCompetitionCell(
								$"Datum konání soutěže: {_settings.CompetitionStartDate.Value:dd.MM.yyyy} - {_settings.CompetitionEndDate.Value:dd.MM.yyyy}",
								bold));
						else
							aboutcompetition.AddCell(AboutCompetitionCell($"Datum konání soutěže {new string('.', 70)}", bold));
					else
						aboutcompetition.AddCell(AboutCompetitionCell($"Datum konání soutěže {new string('.', 70)}", bold));

					Table aboutcompetition2 = new Table(UnitValue.CreatePercentArray(new[] {52.15f, 47.85f})).SetMarginTop(3).UseAllAvailableWidth();
					if (_settings.IsCompetitionTimeEnabled)
					{
						if (_settings.CompetitionStartTime.HasValue && _settings.CompetitionEndTime == null)
							aboutcompetition2.AddCell(
								AboutCompetitionCell($"Doba konání soutěže: {_settings.CompetitionStartTime.Value:HH:mm} - ", bold));
						else if (_settings.CompetitionStartTime.HasValue && _settings.CompetitionEndTime.HasValue)
							aboutcompetition2.AddCell(AboutCompetitionCell(
								$"Doba konání soutěže: {_settings.CompetitionStartTime.Value:HH:mm} - {_settings.CompetitionEndTime.Value:HH:mm}",
								bold));
						else
							aboutcompetition2.AddCell(AboutCompetitionCell($"Doba konání soutěže {new string('.', 78)}", bold));
					}
					else
						aboutcompetition2.AddCell(AboutCompetitionCell($"Doba konání soutěže {new string('.', 78)}", bold));

					if (_settings.IsCompetitionPlaceEnabled)
						aboutcompetition2.AddCell(AboutCompetitionCell($"Místo konání soutěže: {_settings.CompetitionPlace}", bold));
					else
						aboutcompetition2.AddCell(AboutCompetitionCell($"Místo konání soutěže {new string('.', 72)}", bold));

					#region TableHead

					Cell pdfNumber = new Cell(2, 1).SetTextAlignment(TextAlignment.CENTER)
					                               .SetVerticalAlignment(VerticalAlignment.MIDDLE)
					                               .SetFont(font)
					                               .SetPadding(0)
					                               .SetBorder(new SolidBorder(1));
					pdfNumber.Add(new Paragraph("Pořadové"));
					pdfNumber.Add(new Paragraph("číslo"));

					Cell nameCell = new Cell().SetTextAlignment(TextAlignment.LEFT)
					                          .SetVerticalAlignment(VerticalAlignment.MIDDLE)
					                          .SetFont(font)
					                          .SetHeight(17)
					                          .SetBorder(new SolidBorder(1.2f))
					                          .SetBorderBottom(new GrooveBorder(DeviceCmyk.BLACK, 1, 0.5f))
					                          .SetPadding(0);
					nameCell.Add(new Paragraph("Jméno a příjmení")).SetPaddingLeft(17);
					Cell addressCell = new Cell().SetTextAlignment(TextAlignment.LEFT)
					                             .SetVerticalAlignment(VerticalAlignment.MIDDLE)
					                             .SetFont(font)
					                             .SetHeight(17)
					                             .SetBorder(new SolidBorder(1.2f))
					                             .SetBorderTop(Border.NO_BORDER)
					                             .SetPadding(0);
					addressCell.Add(new Paragraph("Přesná adresa")).SetPaddingLeft(17);

					Cell birthDate = new Cell(2, 1).SetTextAlignment(TextAlignment.CENTER)
					                               .SetVerticalAlignment(VerticalAlignment.MIDDLE)
					                               .SetFont(font)
					                               .SetPadding(0);
					birthDate.Add(new Paragraph("Datum"));
					birthDate.Add(new Paragraph("narození"));

					Cell awardCell = new Cell(2, 1).SetTextAlignment(TextAlignment.CENTER)
					                               .SetVerticalAlignment(VerticalAlignment.MIDDLE)
					                               .SetFont(font)
					                               .SetPadding(0);
					awardCell.Add(new Paragraph("Odměna"));
					awardCell.Add(new Paragraph("Kč"));

					Cell signCell = new Cell(2, 1).SetTextAlignment(TextAlignment.CENTER)
					                              .SetVerticalAlignment(VerticalAlignment.MIDDLE)
					                              .SetFont(font)
					                              .SetPadding(0);
					signCell.Add(new Paragraph("Potvrzení o přijetí odměny"));
					signCell.Add(new Paragraph("Podpis"));

					#endregion

					#region Tabulka Bottom

					Cell emptyCell = new Cell().SetBorder(Border.NO_BORDER).SetPadding(0f);

					Cell sumtextCell = new Cell(1, 2).SetFont(bold).SetFontSize(15).SetTextAlignment(TextAlignment.RIGHT)
					                                 .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(0);
					sumtextCell.Add(new Paragraph("CELKEM VYPLACENO: ")).SetPaddingRight(2);

					Cell sumCell = new Cell().SetBackgroundColor(ColorConstants.LIGHT_GRAY)
					                         .SetPadding(0)
					                         .SetFont(bold)
					                         .SetFontSize(15)
					                         .SetTextAlignment(TextAlignment.CENTER)
					                         .SetVerticalAlignment(VerticalAlignment.MIDDLE);
					sumCell.Add(new Paragraph(!isRaw ? CountSum(selectedPersons) : ""));

					Paragraph last = new Paragraph(
						                 "Vyplatil...............................................     Dne...............................................     Podpis...............................................")
					                 .SetFont(font)
					                 .SetFontSize(12)
					                 .SetMarginTop(8);

					#endregion

					for (int i = 0; i < pagesCount; i++)
					{
						doc.Add(documentMainHeader);
						doc.Add(vyplatniListinaHead);
						doc.Add(rules);
						doc.Add(evidenceText);
						doc.Add(aboutcompetition);
						doc.Add(aboutcompetition2);

						#region Table

						Table rozhodciTable = new Table(new float[] {65, 355, 100, 80, 170})
						                      .UseAllAvailableWidth()
						                      .SetMarginTop(6f)
						                      .SetFontSize(FontSize);

						rozhodciTable.SetWidth(765f);
						rozhodciTable.SetBorder(Border.NO_BORDER);
						rozhodciTable.AddCell(pdfNumber);
						rozhodciTable.AddCell(nameCell);
						rozhodciTable.AddCell(birthDate);
						rozhodciTable.AddCell(awardCell);
						rozhodciTable.AddCell(signCell);
						rozhodciTable.AddCell(addressCell);

						for (int j = 0; j < 10; j++)
						{
							int index = j + i * 10;
							Cell pdfNumberData = new Cell(2, 1).SetTextAlignment(TextAlignment.CENTER)
							                                   .SetVerticalAlignment(VerticalAlignment.MIDDLE)
							                                   .SetFont(font)
							                                   .SetFontSize(15)
							                                   .SetHeight(30)
							                                   .SetPadding(0)
							                                   .SetBorder(new SolidBorder(1))
							                                   .SetBorderLeft(new SolidBorder(1));
							pdfNumberData.Add(new Paragraph((j + 1).ToString()));

							Cell nameCellData = new Cell().SetTextAlignment(TextAlignment.LEFT)
							                              .SetFont(font)
							                              .SetHeight(15)
							                              .SetBorder(new SolidBorder(1.2f))
							                              .SetBorderBottom(new GrooveBorder(DeviceCmyk.BLACK, 1, 0.5f))
							                              .SetPadding(0);

							Cell addressCellData = new Cell().SetTextAlignment(TextAlignment.LEFT)
							                                 .SetFont(font)
							                                 .SetHeight(15)
							                                 .SetBorder(new SolidBorder(1.2f))
							                                 .SetBorderTop(Border.NO_BORDER)
							                                 .SetPadding(0);

							Cell birthDateData = new Cell(2, 1).SetTextAlignment(TextAlignment.CENTER)
							                                   .SetVerticalAlignment(VerticalAlignment.MIDDLE)
							                                   .SetFont(font)
							                                   .SetPadding(0)
							                                   .SetHeight(30);

							Cell awardCellData = new Cell(2, 1).SetTextAlignment(TextAlignment.CENTER)
							                                   .SetVerticalAlignment(VerticalAlignment.MIDDLE)
							                                   .SetFont(font)
							                                   .SetPadding(0)
							                                   .SetHeight(30);

							Cell signCellData = new Cell(2, 1).SetTextAlignment(TextAlignment.CENTER)
							                                  .SetVerticalAlignment(VerticalAlignment.MIDDLE)
							                                  .SetFont(font)
							                                  .SetPadding(0)
							                                  .SetHeight(30);
							signCellData.Add(new Paragraph(""));

							if (!isRaw && selectedPersons.Count > j + i * 10)
							{
								nameCellData.Add(new Paragraph(selectedPersons[index].FullName)).SetPaddingLeft(17);
								addressCellData.Add(new Paragraph($"{selectedPersons[index].Address}, {selectedPersons[index].City}"))
								               .SetPaddingLeft(17);
								birthDateData.Add(new Paragraph(selectedPersons[index].BirthDate.ToShortDateString()));
								awardCellData.Add(new Paragraph(selectedPersons[index].Reward.HasValue
									? selectedPersons[index].Reward.Value.ToString()
									: ""));
							}
							else
							{
								nameCellData.Add(new Paragraph("")).SetPaddingLeft(17);
								addressCellData.Add(new Paragraph("")).SetPaddingLeft(17);
								birthDateData.Add(new Paragraph(""));
								awardCellData.Add(new Paragraph(""));
							}

							rozhodciTable.AddCell(pdfNumberData);
							rozhodciTable.AddCell(nameCellData);
							rozhodciTable.AddCell(birthDateData);
							rozhodciTable.AddCell(awardCellData);
							rozhodciTable.AddCell(signCellData);
							rozhodciTable.AddCell(addressCellData);
						}

						#endregion

						rozhodciTable.AddCell(emptyCell);
						rozhodciTable.AddCell(sumtextCell);
						rozhodciTable.AddCell(sumCell);
						rozhodciTable.AddCell(emptyCell);
						doc.Add(rozhodciTable);

						doc.Add(last);
					}

					doc.Close();
				}
			}

			ChromeLauncher.OpenLink(filePath, true);
		}

		private Cell AboutCompetitionCell(string text, PdfFont pdfFont)
		{
			Cell cell = new Cell().Add(new Paragraph(text).SetFontSize(12).SetFont(pdfFont));
			cell.SetPadding(0);
			cell.SetTextAlignment(TextAlignment.LEFT);
			cell.SetBorder(Border.NO_BORDER);
			return cell;
		}

		private string CountSum<T>(List<T> selectedPersons) where T : IPerson
		{
			for (int i = 0; i < selectedPersons.Count; i++)
			{
				if (!selectedPersons[i].Reward.HasValue)
				{
					return "";
				}
			}

			int? sum = 0;
			selectedPersons.ForEach(x => sum += x.Reward);
			if (sum.HasValue)
				// ReSharper disable once PossibleInvalidOperationException
				return sum.Value == 0 ? "" : sum?.ToString();
			return "";
		}
	}
}
