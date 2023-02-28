using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace BookWyrmWPF.Views;

public partial class MainWindow : Window
{
    bool Match { get; set; } = true;

    string FilePath { get; set; } = "";
    string TitleName { get; set; } = "Untitled";
    string DocumentContent { get; set; } = "";

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OpenMenu_Click(object sender, RoutedEventArgs e)
    {
        Open_FileDialog();
    }

    /// <summary>
    /// Open the OpenFileDialog
    /// </summary>
    private void Open_FileDialog()
    {
        var ofd = new OpenFileDialog();
        ofd.ShowDialog();

        // make sure ofd has value
        try
        {
            if (ofd.FileName != null)
            {
                // Path to doc
                FilePath = ofd.FileName;
                // Document name
                TitleName = ofd.SafeFileName;
                // If doc matches textbox
                Match = true;

                // Read Document
                var FileReader = new StreamReader(ofd.FileName);
                // Add content to DocumentContent
                DocumentContent = FileReader.ReadToEnd();
                // Add content to textbox
                TextBody.Text = DocumentContent;
                // close stream
                FileReader.Close();
            }
        }
        catch
        {
            
        }
    }

    private void SaveDocument()
    {
        
        if (FilePath != "")
        {
            // Save Stream
            var SaveStream = new StreamWriter(FilePath);
            // Assign TextBody tex to DocumentContent
            DocumentContent = TextBody.Text;
            // Write to file
            SaveStream.Write(DocumentContent);
            //Close Stream
            SaveStream.Close();

            Match = true;
        }
        else
        {
            SaveAs();
        }


        TitleManager();
    }

    private void SaveAs()
    {
        var sfd = new SaveFileDialog();
        sfd.ShowDialog();


        try
        {
            if (sfd.FileName != null)
            {
                FilePath = sfd.FileName;
                TitleName = sfd.SafeFileName;
                var sw = new StreamWriter(FilePath);
                DocumentContent = TextBody.Text;
                sw.Write(DocumentContent);
                sw.Close();

                Match = true;

            }

            TitleManager();
        }
        catch
        {
            MessageBox.Show("Save was cancelled", "Error!");
        }

        TitleManager();
    }

    private void Save_MenuItem_Click(object sender, RoutedEventArgs e)
    {
        SaveDocument();
    }

    private void TextBody_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(TextBody.Text != DocumentContent) 
        {
            // add astrix to beging of title
            Match = false;
        }
        else 
        {
            Match = true; 
        }

        TitleManager();
    }

    private void TitleManager()
    {
        if (Match)
        {
            Title = TitleName + " - Notepad Clone";
        }
        else
        {
            Title = "*" + TitleName + " - Notepad Clone";
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        TitleManager();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (!Match)
        {
            e.Cancel = true;
            if (MessageBox.Show(
                "Save Changes?",
                "Save Document",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                SaveDocument();
                Environment.Exit(0);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }

    private void NewWindow_MenuItem_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var mainWindow= new MainWindow();
            mainWindow.Show();

            // Code Below was commented out in order to use a a better and more readable code above.
            // Process.Start(Process.GetCurrentProcess().MainModule.FileName);   
        }
        catch (Exception ex)
        {

            MessageBox.Show(ex.Message);
        }
    }

    private void NewDocument()
    {
        TitleName = "Untitled";
        FilePath = "";
        DocumentContent = "";
        Match= true;
        TextBody.Text = "";
    }

    private void New_MenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (!Match) 
        {
            if (MessageBox.Show(
                "Save Changes?",
                "Save Document",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                SaveDocument();
            }
        }

        NewDocument();
        
    }
}




