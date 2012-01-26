Option Strict On
Module doit
    Public inipsw As String = ""
    Public vfkey As String = ""
    Public not_entcrpyted As Boolean = False
    Public mmodel As String = ""
    Public mversion As String = ""
    Public rootfs As String = ""
    Public newversion As String = ""

    Public Function isupdateavaible() As Boolean
        Dim aktuell As String = Application.ProductVersion
        Dim tempwc As New Net.WebClient
        Dim newv As String = tempwc.DownloadString(New Uri("http://tybo.bplaced.net/iversion.txt"))
        newversion = newv
        If newv > aktuell Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function isipswready() As Boolean
        'First get version
        'Code from me o.O
        Dim inhalt As String
        Try
            inhalt = My.Computer.FileSystem.ReadAllText(mainfrm.path & "\Restore.plist")
        Catch
            MsgBox("READ ERROR: Incorrect *.ipsw #1")
            inhalt = ""
            Return False
            Exit Function
        End Try

        Dim vpos As Integer = inhalt.IndexOf("ProductVersion") + 30
        Dim version As String = inhalt.Substring(vpos)
        Dim vpos2 As Integer = version.IndexOf("<")
        Dim version2 As String = version.Substring(0, vpos2)

        Dim mpos As Integer = inhalt.IndexOf("ProductType") + 27
        Dim model As String = inhalt.Substring(mpos)
        Dim mpos2 As Integer = model.IndexOf("<")
        Dim model2 As String = model.Substring(0, mpos2)

        Dim rpos As Integer = inhalt.IndexOf("MinimumSystemPartition") + 44
        Dim root As String = inhalt.Substring(rpos)
        Dim rpos2 As Integer = root.IndexOf("<")
        Dim root2 As String = root.Substring(0, rpos2)

        mversion = version2
        mmodel = model2
        rootfs = root2

        If mversion = "5.0" Then
            If model2 = "iPhone2,1" Then
                vfkey = "0827b7d632abf92f397471cd7f77c037817e56d0ab1bade692b29f311f0fbcdfd6fc3bef"
            ElseIf model2 = "iPhone3,1" Then
                vfkey = "5e5c52fd7e439936d89659b5aa4f79206cd64f09c9961e9d4712a0131075966e2271b354"
            ElseIf model2 = "iPhone3,3" Then
                vfkey = "cbb21346634c5754f3e956f09ca7c93542b87286d7b11de71f18c5d72da529746ab27094"
            ElseIf model2 = "iPod3,1" Then
                vfkey = "e77431d46dedd65cf73df82a823e32e131a76a7caa6d95112bcaede156eb566ce0e8a57d"
            ElseIf model2 = "iPod4,1" Then
                vfkey = "575bcb4f9290a28bc00451f7e444973fd8b0afc529d2d84db4ae227bdd779563f070eaea"
            ElseIf model2 = "iPad1,1" Then
                vfkey = "c7e01f3db404f325eee5062368fc6a795487d859518ee498b4d7f4950a281c5421ffbebf"
            Else
                MsgBox("Model currently not avaible, please wait while i have the keys")
                Return False
                Exit Function
            End If
        ElseIf mversion = "5.0.1" Then
            If model2 = "iPhone2,1" Then
                vfkey = "afde073c5b7a637f0a40b37c7b59f22836074d1f1ec8e85f05573da87da77629624179b4"
            ElseIf model2 = "iPhone3,1" Then
                vfkey = "a8c7fe8c4698684db2b315cdf9b0c569e6769ed721b83799bad4dadfaf6186c6fd6e0fb1"
            ElseIf model2 = "iPhone3,3" Then
                vfkey = "36895be7d36aa1415695ec3cd7d33ccf9b088bfc179d48f4d8fc5fd220a2c6f07c9b76d2"
            ElseIf model2 = "iPod3,1" Then
                vfkey = "9890bc603e37b7ef74bc211cc9bfa09362c251c10c5abe9dcac43716e104cb717419e271"
            ElseIf model2 = "iPod4,1" Then
                vfkey = "7ed37d8c051da8f8d31b0ccf0980fa5ffa54770c7e68ecb5ebf28abe683cadf21a4a99ed"
            ElseIf model2 = "iPad1,1" Then
                vfkey = "e4b60a747ab372bc047b8a7ca08b62f7524fa5888c76f0db709fd1aa9bcea79076952639"
            Else
                MsgBox("Model currently not avaible, please wait while i have the keys")
                Return False
                Exit Function
            End If
        ElseIf mversion = "4.3.5" Then
            If model2 = "iPhone2,1" Then
                vfkey = "8b04eb7e4c4c3bea36693fee2369d48c667083ae79ddea8c02f5ce9da30a74cb20707328"
            ElseIf model2 = "iPhone3,1" Then
                vfkey = "e5e061077217c4937e14d9c4ae1eeb8d69827aa4838168033dd5f1806ab485306a8aa3cf"
            ElseIf model2 = "iPod3,1" Then
                vfkey = "527d77b552fa1fa3708f5c3c2feff8641c7716a24df4dbb49613d0776a7afa3ab9cf95dd"
            ElseIf model2 = "iPad1,1" Then
                vfkey = "e002a32650a28f4ecd0793d2e36d8bc93bf4a60bb010dbe9ef2ed41821fc5463b24c791b"
            Else
                MsgBox("Model currently not avaible, please wait while i have the keys")
                Return False
                Exit Function
            End If
        ElseIf mversion = "4.3.4" Then
            If model2 = "iPhone2,1" Then
                vfkey = "fb9480e2b80a26cd75d923d7918539edb19caed5a72dfe7a78734cd2a82597869b9ceaf5"
            ElseIf model2 = "iPhone3,1" Then
                vfkey = "f3b2e5122cfd8b8215ed8271d83af0183f6d6634afd63444dfd7787e274b7520fc9d5c40"
            ElseIf model2 = "iPad1,1" Then
                vfkey = "aa3f737295c1d7a1e0539b8b1a02310b9ec7503be6ed05b88520e50a1a006f4b270b3e9f"
            Else
                MsgBox("Model currently not avaible, please wait while i have the keys")
                Return False
                Exit Function
            End If
        ElseIf mversion = "4.3.3" Then
            If model2 = "iPhone2,1" Then
                vfkey = "148f4fca734e973551fc8fa65a04883041854b060e3fe1e6c3ca4499a3204d1d97594a47"
            ElseIf model2 = "iPad1,1" Then
                vfkey = "765d0fecc4f714ca20fa6eceeabb454b04cd2998cc3ab3bba290866788a8c6cf555945ac"
            Else
                MsgBox("Model currently not avaible, please wait while i have the keys")
                Return False
                Exit Function
            End If
        ElseIf mversion = "4.3" Then
            If model2 = "iPhone2,1" Then
                vfkey = "afde073c5b7a637f0a40b37c7b59f22836074d1f1ec8e85f05573da87da77629624179b4"
            ElseIf model2 = "iPhone3,1" Then
                vfkey = "9873392c91743857cf5b35c9017c6683d5659c9358f35c742be27bfb03dee77c"
            ElseIf model2 = "iPod3,1" Then
                vfkey = "9890bc603e37b7ef74bc211cc9bfa09362c251c10c5abe9dcac43716e104cb717419e271"
            ElseIf model2 = "iPod4,1" Then
                vfkey = "7ed37d8c051da8f8d31b0ccf0980fa5ffa54770c7e68ecb5ebf28abe683cadf21a4a99ed"
            ElseIf model2 = "iPad1,1" Then
                vfkey = "e4b60a747ab372bc047b8a7ca08b62f7524fa5888c76f0db709fd1aa9bcea79076952639"
            Else
                MsgBox("Model currently not avaible, please wait while i have the keys")
                Return False
                Exit Function
            End If
        Else
            MsgBox("iOS Version not compatible, currently only 5.0 and 5.0.1 Error Code #2")
            Return False
            Exit Function
        End If
        Return True
    End Function

    Public Sub Delay(ByVal dblSecs As Double)
        'iH8sn0w's delay code...
        Const OneSec As Double = 1.0# / (1440.0# * 60.0#)
        Dim dblWaitTil As Date
        Now.AddSeconds(OneSec)
        dblWaitTil = Now.AddSeconds(OneSec).AddSeconds(dblSecs)
        Do Until Now > dblWaitTil
            Application.DoEvents() ' Allow windows messages to be processed
        Loop
    End Sub
End Module
