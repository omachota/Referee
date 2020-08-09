using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Dapper;
using System.Data.SQLite;
using System.Data;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Paragraph = iTextSharp.text.Paragraph;
using System.Collections;
using System.Reflection;
using Referee.Models;

namespace Referee
{
    /// <summary>
    /// Interakční logika pro DirectPrintUserControl.xaml
    /// </summary>
    public partial class DirectPrintUserControl : UserControl
    {
        public DirectPrintUserControl()
        {
            InitializeComponent();
            GetDTGDColumbs();
            pocetStranComboBox.ItemsSource = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        }

        private List<Rozhodci> ListRozhodci = new List<Rozhodci>();

        /// <summary>
        /// Načte vybrané sloupce pro datagrid
        /// </summary>
        private void GetDTGDColumbs()
        {
            Helper helper = new Helper();
            string DTGDColumbsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DTGDColumbs.json";
            string text = File.ReadAllText(DTGDColumbsPath);
            if (text == "")
            {
                string zapis = helper.GenerateNewSetting();
                File.WriteAllText(DTGDColumbsPath, zapis);
                text = zapis;
            }
            List<OptionalColumn> list = (List<OptionalColumn>)JsonConvert.DeserializeObject(text, typeof(List<OptionalColumn>));
            var cellStyle = new Style
            {
                TargetType = typeof(TextBlock),
                Setters =
                {
                    new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center),
                    new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center),
                    new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Left)
                }
            };
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsSelected)
                {
                    DataGridTextColumn gridColumn = new DataGridTextColumn
                    {
                        Header = list[i].Header, Binding = new Binding(list[i].Binding), ElementStyle = cellStyle
                    };
                    RozhodciDataGrid.Columns.Add(gridColumn);
                }
            }
            DataGridTemplateColumn gridDeleteColumn = new DataGridTemplateColumn();
            DataTemplate dtm = new DataTemplate();
            gridDeleteColumn.Header = "";
            FrameworkElementFactory btnReset = new FrameworkElementFactory(typeof(Button));
            btnReset.SetValue(Button.ContentProperty, "Smazat");
            btnReset.SetValue(Button.DataContextProperty, new Binding("TableName"));
            btnReset.AddHandler(Button.ClickEvent, new RoutedEventHandler(DeleteButton_Click));
            dtm.VisualTree = btnReset;
            gridDeleteColumn.CellTemplate = dtm;
            RozhodciDataGrid.Columns.Add(gridDeleteColumn);
            ListRozhodci = GetRozhodci();
            RozhodciDataGrid.ItemsSource = ListRozhodci;
            pocetRozhodcichTextBox.Text = "Celkem: " + ListRozhodci.Count;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            PrvkyVDialogHost(Visibility.Collapsed, 350, Visibility.Visible);
            OtevriDialogHostMode(Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible);
        }

        private void NactiDataGridSRozhodcimiZnovu()
        {
            ListRozhodci.Clear();
            ListRozhodci = GetRozhodci();
            RozhodciDataGrid.ItemsSource = null;
            RozhodciDataGrid.ItemsSource = ListRozhodci;
            pocetRozhodcichTextBox.Text = $"Celkem: " + ListRozhodci.Count.ToString();
            CheckBoxPouzit = false;
            pocetVybranychRozhodci = 0;
            pocetVybranychRozhodcichTextBox.Text = "Vybraní: 0";
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        private List<Rozhodci> GetRozhodci()
        {
            ListRozhodci.Clear();
            try
            {
                using (IDbConnection pripojeni = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = pripojeni.Query<Rozhodci>("SELECT * FROM Rozhodci", new DynamicParameters());
                    return output.ToList();
                }
            }
            catch (Exception e)
            {
               return new List<Rozhodci>(new []{ new Rozhodci("d", "f", DateTime.Now, "d", "C", 0)});
            }
        }

        private void AddNovyRozhodci_Click(object sender, RoutedEventArgs e)
        {
            JmenoTextBox.Text = "";
            PrijmeniTextBox.Text = "";
            AdresaTextBox.Text = "";
            MestoTextBox.Text = "";
            DatumNarozeniDatePicker.Text = "";
            PrvkyVDialogHost(Visibility.Visible, 350, Visibility.Collapsed);
            OtevriDialogHostMode(Visibility.Collapsed, Visibility.Visible, Visibility.Collapsed);
        }

        /// <summary>
        /// Visibility políček s údaji v dialogHostu
        /// </summary>
        /// <param name="udaje">Visibility pro políčka s informacemi o  rozhodčím</param>
        /// <param name="dialogHostHeight">výška dialogHostu</param>
        /// <param name="smazani">Visibility prvků pro mazání</param>
        private void PrvkyVDialogHost(Visibility udaje, int dialogHostHeight, Visibility smazani)
        {
            JmenoTextBox.Visibility = udaje;
            PrijmeniTextBox.Visibility = udaje;
            DatumNarozeniDatePicker.Visibility = udaje;
            AdresaTextBox.Visibility = udaje;
            MestoTextBox.Visibility = udaje;
            DialogHostContentGrid.Height = dialogHostHeight; // 100 pro smazání
            popisekSmazatRozhodcihoTextBlock.Visibility = smazani;
            SmazRozhodcihoButton.Visibility = smazani;
        }

        private void VytvorNovehorozhodcihoButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateNewRozhodci())
            {
                Rozhodci rozhodci = new Rozhodci(JmenoTextBox.Text, PrijmeniTextBox.Text, DateTime.Parse(DatumNarozeniDatePicker.Text), AdresaTextBox.Text, MestoTextBox.Text, 0);
                using (IDbConnection pripojeni = new SQLiteConnection(LoadConnectionString()))
                {
                    pripojeni.Execute("INSERT INTO Rozhodci(Header,Prijmeni, DatumNarozeni, AdresaBydliste, Mesto) VALUES (@Header,@Prijmeni, @DatumNarozeni, @AdresaBydliste, @Mesto)", rozhodci);
                }
                VytvorNovehorozhodcihoButton.IsEnabled = false;
                AddNewRozhodciDialogHost.IsOpen = false;
                NactiDataGridSRozhodcimiZnovu();
            }
        }

        private bool ValidateNewRozhodci()
        {
            if (JmenoTextBox.Text == "")
            {
                return false;
            }
            else if (PrijmeniTextBox.Text == "")
            {
                return false;
            }
            else if (DatumNarozeniDatePicker.Text == "")
            {
                return false;
            }
            else if (AdresaTextBox.Text == "")
            {
                return false;
            }
            else if (MestoTextBox.Text == "")
            {
                return false;
            }
            else return true;
        }

        private void ZavriDialogHostButton_Click(object sender, RoutedEventArgs e)
        {
            if (DialogHostContentGrid.Height == 100)
            {
                PrvkyVDialogHost(Visibility.Collapsed, 100, Visibility.Collapsed);
            }
            else
            {
                PrvkyVDialogHost(Visibility.Collapsed, 350, Visibility.Collapsed);
            }
            AddNewRozhodciDialogHost.IsOpen = false;
        }

        private void EditovatRozhodcihoButton_Click(object sender, RoutedEventArgs e)
        {
            PrvkyVDialogHost(Visibility.Visible, 350, Visibility.Collapsed);
            OtevriDialogHostMode(Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed);

            foreach (Rozhodci rozhodci in RozhodciDataGrid.SelectedItems)
            {
                JmenoTextBox.Text = rozhodci.FirstName;
                PrijmeniTextBox.Text = rozhodci.LastName;
                AdresaTextBox.Text = rozhodci.Address;
                MestoTextBox.Text = rozhodci.City;
                DatumNarozeniDatePicker.Text = rozhodci.BirthDate.ToShortDateString();
                editovanyRozhodci = rozhodci.Id;
            }
        }

        /// <summary>
        /// Visibility tlačítek v dialogHostu
        /// </summary>
        /// <param name="edit">EditujRozhodcihoButton.Visibility</param>
        /// <param name="novy">VytvorNovehorozhodcihoButton.Visibility</param>
        /// <param name="smazat">SmazRozhodcihoButton.Visibility</param>
        private void OtevriDialogHostMode(Visibility edit, Visibility novy, Visibility smazat)
        {
            EditujRozhodcihoButton.Visibility = edit;
            EditujRozhodcihoButton.IsEnabled = true;
            VytvorNovehorozhodcihoButton.Visibility = novy;
            VytvorNovehorozhodcihoButton.IsEnabled = true;
            SmazRozhodcihoButton.Visibility = smazat;
            AddNewRozhodciDialogHost.IsOpen = true;
        }

        private long editovanyRozhodci;

        private void EditujRozhodcihoButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateNewRozhodci())
            {
                using (SQLiteConnection pripojeni = new SQLiteConnection(LoadConnectionString()))
                {
                    pripojeni.Open();
                    SQLiteCommand prikaz = new SQLiteCommand("UPDATE Rozhodci SET Header = @jmeno, Prijmeni = @prijmeni, DatumNarozeni = @datumNarozeni, AdresaBydliste = @adresaBydliste, Mesto = @mesto WHERE Id = @editovanyRozhodci", pripojeni);
                    prikaz.Parameters.AddWithValue("@jmeno", JmenoTextBox.Text);
                    prikaz.Parameters.AddWithValue("@prijmeni", PrijmeniTextBox.Text);
                    prikaz.Parameters.AddWithValue("@datumNarozeni", DatumNarozeniDatePicker.Text);
                    prikaz.Parameters.AddWithValue("@adresaBydliste", AdresaTextBox.Text);
                    prikaz.Parameters.AddWithValue("@mesto", MestoTextBox.Text);
                    prikaz.Parameters.AddWithValue("@editovanyRozhodci", editovanyRozhodci);

                    prikaz.ExecuteNonQuery();
                }
                AddNewRozhodciDialogHost.IsOpen = false;
                EditujRozhodcihoButton.IsEnabled = false;
                NactiDataGridSRozhodcimiZnovu();
            }
        }

        private List<int> vybraniRozhodci = new List<int>();

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            var rows = GetDataGridRows(RozhodciDataGrid);
            int i = 0;
            foreach (DataGridRow r in rows)
            {
                foreach (DataGridColumn column in RozhodciDataGrid.Columns)
                {
                    if (column.GetCellContent(r) is CheckBox)
                    {
                        CheckBox cellContent = column.GetCellContent(r) as CheckBox;
                        if (cellContent.IsChecked == true)
                        {
                            vybraniRozhodci.Add(i);
                        }
                    }
                }
                i++;
            }
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                Print(true, 0);
            }
            vybraniRozhodci.Clear();
        }

        private void ClearPrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                Print(false, Convert.ToInt32(pocetStranComboBox.SelectedValue));
            }
        }

        //string FONT = "c:/windows/fonts/arial.ttf";
        //string CursiveFONT = "c:/windows/fonts/arialbd.ttf";
        /// <summary>
        /// Vygeneruje pdf pro tisk
        /// </summary>
        /// <param name="clear">Určuje, zda budou do výpisu přidání rozhodčí</param>
        private void Print(bool clear, int pocetStranPreNast)
        {
            Document document = new Document(PageSize.A4.Rotate(), 38f, 38f, 20f, 20f);
            try
            {
                string PdfPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Rozhodci.pdf";
                PdfWriter.GetInstance(document, new FileStream(PdfPath, FileMode.Create));
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED); // (FONT, BaseFont.IDENTITY_H, BaseFont.EMBEDDED)
                BaseFont bfCur = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
                int pocetStran = (int)Math.Ceiling((double)vybraniRozhodci.Count / 10);
                if (pocetStran == 0)
                    pocetStran = 1;
                if (clear == false)
                {
                    pocetStran = pocetStranPreNast;
                }
                for (int i = 0; i < pocetStran; i++)
                {
                    #region Pdf text
                    Paragraph popisek1Para = new Paragraph("TJ, Sportovní klub, Atletický oddíl, Atletický klub: ATLETIKA STARÁ BOLESLAV, z.s. ", new Font(bfCur, 16));
                    popisek1Para.Alignment = Element.ALIGN_CENTER;
                    document.Add(popisek1Para);
                    document.Add(new iTextSharp.text.Paragraph("        ", new Font(bf, 1)));
                    Paragraph vyplatniListinaHead = new Paragraph("VÝPLATNÍ LISTINA ODMĚN ROZHODČÍCH", new Font(bfCur, 16));
                    vyplatniListinaHead.Alignment = Element.ALIGN_CENTER;
                    document.Add(vyplatniListinaHead);
                    document.Add(new Paragraph("        ", new Font(bf, 4)));
                    Paragraph pravidlaPara = new Paragraph("Níže podepsaní účastníci soutěže souhlasili s uvedením svých osobních údajů na této výplatní listině (jméno, příjmení, datum narození a adresa).", new Font(bf, 9));
                    pravidlaPara.Alignment = Element.ALIGN_CENTER;
                    document.Add(pravidlaPara);
                    Paragraph textPodPravidlyPara = new Paragraph("Tyto údaje budou součástí evidence ........................................................................................................................... a budou jen pro vnitřní potřebu.", new Font(bf, 9));
                    textPodPravidlyPara.Alignment = Element.ALIGN_CENTER;
                    document.Add(textPodPravidlyPara);
                    document.Add(new Paragraph("        ", new Font(bf, 2)));
                    Paragraph nazevADatumSoutezePara = new Paragraph("Název soutěže .........................................................................................    Datum konání soutěže .......................................................................", new Font(bfCur, 12));
                    popisek1Para.Alignment = Element.ALIGN_LEFT;
                    document.Add(nazevADatumSoutezePara);
                    document.Add(new Paragraph("        ", new Font(bf, 2)));
                    Paragraph dobaAMistoPara = new Paragraph("Doba konání soutěže ..............................................................................    Místo konání soutěže: Houštka, Stará Boleslav", new Font(bfCur, 12));
                    popisek1Para.Alignment = Element.ALIGN_LEFT;
                    document.Add(dobaAMistoPara);
                    document.Add(new Paragraph("        ", new Font(bf, 8)));
                    #endregion

                    #region TabulkaHeader
                    PdfPTable tablukaSRozhodcimi = new PdfPTable(5);
                    tablukaSRozhodcimi.TotalWidth = 600f;
                    tablukaSRozhodcimi.LockedWidth = true;
                    float[] sirky = { 65, 353, 100, 80, 170 };
                    tablukaSRozhodcimi.SetTotalWidth(sirky);
                    int headerFontTableSize = 12;

                    PdfPCell poradoveCisloCell = new PdfPCell(new Phrase("Pořadové\nčíslo", new Font(bf, headerFontTableSize)));
                    poradoveCisloCell.Rowspan = 2;
                    poradoveCisloCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    poradoveCisloCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    poradoveCisloCell.CalculatedHeight = 34;
                    tablukaSRozhodcimi.AddCell(poradoveCisloCell);

                    PdfPCell jmenoCell = new PdfPCell(new Phrase("    Jméno a příjmení", new Font(bf, headerFontTableSize)));
                    jmenoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    jmenoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    jmenoCell.BorderWidthBottom = 0.2f;
                    tablukaSRozhodcimi.AddCell(jmenoCell);

                    PdfPCell datumNarozeniCell = new PdfPCell(new Phrase("Datum\nnarození", new Font(bf, headerFontTableSize)));
                    datumNarozeniCell.Rowspan = 2;
                    datumNarozeniCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    datumNarozeniCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tablukaSRozhodcimi.AddCell(datumNarozeniCell);

                    PdfPCell odmenaKcCell = new PdfPCell(new Phrase("Odměna\nKč", new Font(bf, headerFontTableSize)));
                    odmenaKcCell.Rowspan = 2;
                    odmenaKcCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    odmenaKcCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tablukaSRozhodcimi.AddCell(odmenaKcCell);

                    PdfPCell potvrzeniCell = new PdfPCell(new Phrase("Potvrzení o přijetí odměny\nPodpis", new Font(bf, headerFontTableSize)));
                    potvrzeniCell.Rowspan = 2;
                    potvrzeniCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    potvrzeniCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tablukaSRozhodcimi.AddCell(potvrzeniCell);

                    PdfPCell adresaCell = new PdfPCell(new Phrase("    Přesná adresa", new Font(bf, headerFontTableSize)));
                    adresaCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    jmenoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    adresaCell.BorderWidthTop = 0f;
                    tablukaSRozhodcimi.AddCell(adresaCell);
                    #endregion

                    #region Tabulka Content
                    for (int j = 0; j < 10; j++)
                    {
                        int index;
                        try
                        {
                            index = vybraniRozhodci[j + i * 10];
                        }
                        catch
                        {
                            index = -1;
                        }
                        PdfPCell poradoveCisloCellCykl = new PdfPCell(new Phrase((j + 1).ToString(), new Font(bf, 15)));
                        poradoveCisloCellCykl.Rowspan = 2;
                        poradoveCisloCellCykl.VerticalAlignment = Element.ALIGN_MIDDLE;
                        poradoveCisloCellCykl.HorizontalAlignment = Element.ALIGN_CENTER;
                        poradoveCisloCellCykl.CalculatedHeight = 34;
                        tablukaSRozhodcimi.AddCell(poradoveCisloCellCykl);

                        string celeJmeno;
                        try { celeJmeno = $"    { ListRozhodci[index].FirstName } { ListRozhodci[index].LastName }"; } catch { celeJmeno = " "; }
                        if (clear == false)
                            celeJmeno = " ";
                        PdfPCell jmenoCellCykl = new PdfPCell(new Phrase(celeJmeno, new Font(bf, headerFontTableSize)));
                        jmenoCellCykl.VerticalAlignment = Element.ALIGN_MIDDLE;
                        jmenoCellCykl.HorizontalAlignment = Element.ALIGN_LEFT;
                        jmenoCellCykl.BorderWidthBottom = 0.2f;
                        tablukaSRozhodcimi.AddCell(jmenoCellCykl);

                        string datumNarozeni;
                        try { datumNarozeni = $"{ ListRozhodci[index].BirthDate.ToShortDateString() }"; } catch { datumNarozeni = " "; }
                        if (clear == false)
                            datumNarozeni = " ";
                        PdfPCell datumNarozeniCellCykl = new PdfPCell(new Phrase(datumNarozeni, new Font(bf, headerFontTableSize)));
                        datumNarozeniCellCykl.Rowspan = 2;
                        datumNarozeniCellCykl.VerticalAlignment = Element.ALIGN_MIDDLE;
                        datumNarozeniCellCykl.HorizontalAlignment = Element.ALIGN_CENTER;
                        tablukaSRozhodcimi.AddCell(datumNarozeniCellCykl);

                        PdfPCell odmenaKcCellCykl = new PdfPCell(new Phrase(" ", new Font(bf, headerFontTableSize)));
                        odmenaKcCellCykl.Rowspan = 2;
                        odmenaKcCellCykl.VerticalAlignment = Element.ALIGN_MIDDLE;
                        odmenaKcCellCykl.HorizontalAlignment = Element.ALIGN_CENTER;
                        tablukaSRozhodcimi.AddCell(odmenaKcCellCykl);

                        PdfPCell potvrzeniCellCykl = new PdfPCell(new Phrase(" ", new Font(bf, headerFontTableSize)));
                        potvrzeniCellCykl.Rowspan = 2;
                        potvrzeniCellCykl.VerticalAlignment = Element.ALIGN_MIDDLE;
                        potvrzeniCellCykl.HorizontalAlignment = Element.ALIGN_CENTER;
                        tablukaSRozhodcimi.AddCell(potvrzeniCellCykl);

                        string presnaAdresa;
                        try { presnaAdresa = $"    { ListRozhodci[index].Address }, { ListRozhodci[index].City }"; } catch { presnaAdresa = " "; }
                        if (clear == false)
                            presnaAdresa = " ";
                        PdfPCell adresaCellCykl;
                        if (presnaAdresa.Length > 58)
                        {
                            int alternaviveheaderFontTableSize = 8;
                            adresaCellCykl = new PdfPCell(new Phrase(presnaAdresa, new Font(bf, alternaviveheaderFontTableSize)));
                        }
                        else
                        {
                            adresaCellCykl = new PdfPCell(new Phrase(presnaAdresa, new Font(bf, headerFontTableSize)));
                        }
                        adresaCellCykl.VerticalAlignment = Element.ALIGN_MIDDLE;
                        adresaCellCykl.HorizontalAlignment = Element.ALIGN_LEFT;
                        adresaCellCykl.BorderWidthTop = 0f;
                        tablukaSRozhodcimi.AddCell(adresaCellCykl);
                    }
                    #endregion

                    #region Tabulka Bottom
                    PdfPCell prazdnaCell = new PdfPCell();
                    prazdnaCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    tablukaSRozhodcimi.AddCell(prazdnaCell);

                    PdfPCell celkemVyplacenoCell = new PdfPCell(new Phrase("CELKEM VYPLACENO: ", new Font(bfCur, 15)));
                    celkemVyplacenoCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    celkemVyplacenoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celkemVyplacenoCell.Colspan = 2;
                    celkemVyplacenoCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    tablukaSRozhodcimi.AddCell(celkemVyplacenoCell);

                    PdfPCell vyplacenoCell = new PdfPCell();
                    vyplacenoCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    tablukaSRozhodcimi.AddCell(vyplacenoCell);

                    PdfPCell prazdnaCell2 = new PdfPCell();
                    prazdnaCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    tablukaSRozhodcimi.AddCell(prazdnaCell2);

                    document.Add(tablukaSRozhodcimi);

                    document.Add(new Paragraph("  ", new Font(bf, 6)));
                    document.Add(new Paragraph("Vyplatil...............................................     Dne...............................................     Podpis...............................................", new Font(bf, 12)));
                    document.Add(new Paragraph("  ", new Font(bf, 12)));
                    #endregion
                }
            }
            catch (DocumentException de)
            {
                MessageBox.Show(de.Message);
            }
            catch (IOException ioe)
            {
                MessageBox.Show(ioe.Message);
            }
            document.Close();
        }

        private void TisknoutRozhodcihoCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SetDataGridCheckBoxColumn(true);
            CheckBoxPouzit = true;
            int vybrani = ListCheckBoxes.Count((x) => x.IsChecked == true);
            pocetVybranychRozhodcichTextBox.Text = "Vybraní: " + vybrani;
        }

        private void TisknoutRozhodcihoCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SetDataGridCheckBoxColumn(false);
            CheckBoxPouzit = true;
            pocetVybranychRozhodcichTextBox.Text = "Vybraní: 0";
        }

        private List<CheckBox> ListCheckBoxes = new List<CheckBox>();

        private void SetDataGridCheckBoxColumn(bool CheckBoxChecked)
        {
            ListCheckBoxes.Clear();
            var rows = GetDataGridRows(RozhodciDataGrid);
            foreach (DataGridRow r in rows)
            {
                foreach (DataGridColumn column in RozhodciDataGrid.Columns)
                {
                    if (column.GetCellContent(r) is CheckBox)
                    {
                        try
                        {
                            CheckBox cellContentCheckBox = column.GetCellContent(r) as CheckBox;
                            cellContentCheckBox.Checked += CellContentCheckBox_Checked;
                            cellContentCheckBox.Unchecked += CellContentCheckBox_Unchecked;
                            ListCheckBoxes.Add(cellContentCheckBox);
                            cellContentCheckBox.IsChecked = CheckBoxChecked;
                        }
                        catch { }
                    }
                }
            }
        }
        private int pocetVybranychRozhodci = 0;
        private bool CheckBoxPouzit = false;
        private void CellContentCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxPouzit)
            {
                int vybrani = ListCheckBoxes.Count((x) => x.IsChecked == true);
                pocetVybranychRozhodcichTextBox.Text = "Vybraní: " + vybrani;
            }
            else
            {
                pocetVybranychRozhodci--;
                pocetVybranychRozhodcichTextBox.Text = "Vybraní: " + pocetVybranychRozhodci;
            }
        }

        private void CellContentCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxPouzit)
            {
                int vybrani = ListCheckBoxes.Count((x) => x.IsChecked == true);
                pocetVybranychRozhodcichTextBox.Text = "Vybraní: " + vybrani;
            }
            else
            {
                pocetVybranychRozhodci++;
                pocetVybranychRozhodcichTextBox.Text = "Vybraní: " + pocetVybranychRozhodci;
            }
        }

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        private void SmazRozhodcihoButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Rozhodci rozhodci in RozhodciDataGrid.SelectedItems)
            {
                using (SQLiteConnection pripojeni = new SQLiteConnection(LoadConnectionString()))
                {
                    pripojeni.Open();
                    SQLiteCommand prikaz = new SQLiteCommand("DELETE FROM Rozhodci Where Id=@id", pripojeni);
                    prikaz.Parameters.AddWithValue("@id", rozhodci.Id);
                    prikaz.ExecuteNonQuery();
                    pripojeni.Close();
                }
            }
            NactiDataGridSRozhodcimiZnovu();
            AddNewRozhodciDialogHost.IsOpen = false;
        }
    }
}
