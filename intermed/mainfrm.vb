Option Strict On 'A must be
Imports InterMed.doit
Public Class mainfrm
    Public path As String = My.Computer.FileSystem.SpecialDirectories.Temp & "\intermed\" 'Must be
    Dim extract As Boolean 'To know if dir or dmg decrypt
    Dim outdir As String = "" 'Outdirectory if needed
    Dim outfile As String = "" 'yea

    Private Sub openipsw_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles openipsw.FileOk
        inipsw = openipsw.FileName
        getpaths() 'Not multithreading at this time
    End Sub

    Sub getpaths()
        GroupBox2.Enabled = False
        extract = RadioButton1.Checked
        If extract = True Then
            Using outdirdg As New FolderBrowserDialog
                Do
                    outdirdg.ShowDialog()
                    If outdirdg.SelectedPath.Length > 1 Then
                        outdir = outdirdg.SelectedPath
                        Exit Do
                    Else
                        cleanup()
                    End If
                Loop
            End Using
        Else
            Using outdmgdg As New SaveFileDialog
                outdmgdg.Filter = "DMG File|*.dmg"
                outdmgdg.Title = "Save place for DMG"

                Do
                    outdmgdg.ShowDialog()
                    If outdmgdg.FileName.Length > 1 Then
                        outfile = outdmgdg.FileName
                        Exit Do
                    Else
                        cleanup()
                    End If
                Loop
            End Using
        End If
        If CheckBox1.Checked = True Then
            not_entcrpyted = True
        End If
        doit1.RunWorkerAsync()
    End Sub

    Private Sub doit1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles doit1.DoWork
        'Unzip
        'Check device & version
        'Decrypt RootFS via dmg
        'extract all files via hfsplus.exe
        'Copy
        Try
            My.Computer.FileSystem.CreateDirectory(path)
        Catch ex As Exception
            MsgBox("Error #1001")
            cleanup()
            Exit Sub
        End Try
        Label2.Text = "Extracting Resource"
        System.IO.File.WriteAllBytes(path & "\dmg.exe", My.Resources.dmg)
        System.IO.File.WriteAllBytes(path & "\hfsplus.exe", My.Resources.hfsplus)
        System.IO.File.WriteAllBytes(path & "\zlib1.dll", My.Resources.zlib1)

        Label2.Text = "Unzip..."
        Dim zip As New Ionic.Zip.ZipFile(inipsw)
        zip.ExtractSelectedEntries("Restore.plist", "/", path, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently)

        Label2.Text = "Check iPSW"
        If isipswready() = False Then
            cleanup()
            Exit Sub
        End If
        Label2.Text = mmodel & " " & mversion & " " & rootfs
        Delay(3)

        Label2.Text = "Extract RootFS from iPSW"
        zip.ExtractSelectedEntries(rootfs, "/", path, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently)

        Label2.Text = "Decrypting RootFS"
        Dim perc As New Process
        perc.StartInfo.WorkingDirectory = path
        perc.StartInfo.FileName = "dmg.exe"
        If not_entcrpyted = False Then
            If extract = True Then
                perc.StartInfo.Arguments = "extract " & rootfs & " decrypt.dmg " & "-k " & vfkey
            Else
                perc.StartInfo.Arguments = "extract " & rootfs & " " & outfile & " -k " & vfkey
            End If
        Else
            If extract = True Then
                perc.StartInfo.Arguments = "extract " & rootfs & " decrypt.dmg "
            Else
                perc.StartInfo.Arguments = "extract " & rootfs & " " & outfile
            End If
        End If
        perc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        perc.Start()
        perc.WaitForExit()

        If extract = True Then
            Label2.Text = "Extracting RootFS"
            perc.StartInfo.FileName = "hfsplus.exe"
            perc.StartInfo.Arguments = "decrypt.dmg extractall / " & outdir
            perc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            perc.Start()
            perc.WaitForExit()
        End If
        cleanup()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        openipsw.ShowDialog()
    End Sub

    Private Sub Panel1_DragDrop_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Panel1.DragDrop
        If e.Data.GetDataPresent("FileDrop", True) = True Then
            Dim Wert As String() = CType(CType(CType(e.Data.GetData("FileDrop"), String()), Object), String())
            If Wert(0).ToString.Contains(".ipsw") Then
                inipsw = Wert(0).ToString
                getpaths()
            End If
        End If
    End Sub

    Private Sub Panel1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Panel1.DragEnter
        e.Effect = DragDropEffects.All
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start("http://twitter.com/validati0n")
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MessageBox.Show("Decrypt rootfs for" & vbNewLine & _
                        "Supported devices:" & vbNewLine & _
                        "iPhone 4 GSM (5.0/5.0.1/4.3.5)" & vbNewLine & _
                        "iPhone 4 CDMA (5.0/5.0.1)" & vbNewLine & _
                        "iPhone 3G[S] (5.0/5.0.1/4.3.5/4.3.4/4.3.3)" & vbNewLine & _
                        "iPad 1G (5.0/5.0.1/4.3.5/4.3.4/4.3.3)" & vbNewLine & _
                        "iPod 3G (5.0/5.0.1/4.3.5)" & vbNewLine & _
                        "iPod 4G (5.0/5.0.1)" & vbNewLine & _
                        "WARNING: DON'T USE A PWND (Sn0wbreeze/Redns0w/iFaith) iPSW" & vbNewLine & _
                        "More firmware/device support coming soon...", "Intermed", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub mainfrm_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        CheckForIllegalCrossThreadCalls = False 'I can't write good Multithreading without this
        showworker.RunWorkerAsync()
    End Sub

    Private Sub showworker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles showworker.DoWork
        If isupdateavaible() = True Then
            Label2.Text = "There are Updates avaible (" & newversion & ")!"
            Label6.Text = "You have to update before you use InterMed..."
        Else
            Label2.Text = "By validati0n (@validati0n) RC2"
            GroupBox2.Enabled = True
        End If
    End Sub

    Private Sub mainfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IO.Directory.Exists(path) = True Then 'to delete old files
            My.Computer.FileSystem.DeleteDirectory(path, FileIO.DeleteDirectoryOption.DeleteAllContents)
        End If
        My.Computer.FileSystem.CreateDirectory(path)
    End Sub
End Class
